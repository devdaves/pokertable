using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Data;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Extensions;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Integration
{
    /// <summary>
    /// Repository Tests
    /// These tests require the Azure Storage Emulator be running.
    /// </summary>
    [TestClass]
    public class AzureRepositoryTests
    {
        /// <summary>
        /// azure client field
        /// </summary>
        private CloudTableClient client;

        /// <summary>
        /// azure table field
        /// </summary>
        private CloudTable table;

        /// <summary>
        /// repository field
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Runs before every test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            try
            {
                var connectionString = ConfigurationManager.AppSettings[AzureRepository.AzureStorageConnectionStringKey];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                this.client = storageAccount.CreateCloudTableClient();
                this.table = this.client.GetTableReference(AzureRepository.AzureTableName);
                this.table.CreateIfNotExists();
                this.repository = new AzureRepository();
            }
            catch (StorageException storageException)
            {
                if (storageException.Message == "Unable to connect to the remote server")
                {
                    throw new Exception("Turn on the Storage Emulator please...");
                }

                throw;
            }
        }

        /// <summary>
        /// Runs after every test
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            this.table.DeleteIfExists();
        }

        /// <summary>
        /// SaveTable should convert a table model into a poker table entity
        /// and insert it in table storage if it doesn't exist
        /// </summary>
        [TestMethod]
        public void SaveTable_NoExisitingTableRecord_Should_InsertPokerTableEntity()
        {
            var table = new Table("Test", "TestPassword");
            this.repository.SaveTable(table);

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), PokerTableEntity.Prefix);

            Assert.AreEqual(table.Id.ToString(), entities[0].PartitionKey);
            Assert.AreEqual(table.Name, entities[0].Name);
            Assert.AreEqual(table.Password, entities[0].Password);
        }

        /// <summary>
        /// SaveTable - if saving a table when the table already exists in storage
        /// the values should be updated instead of duplicating the record or causing
        /// an exception
        /// </summary>
        [TestMethod]
        public void SaveTable_WithExistingTableRecord_Should_UpdateValues()
        {
            // orginal table save
            var table = new Table("Test", "TestPassword");
            this.repository.SaveTable(table);

            // change something and tell it to create again, should update the table
            table.Password = "TestPassword2";
            this.repository.SaveTable(table);

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), PokerTableEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(table.Password, entities[0].Password);
        }

        /// <summary>
        /// SavePlayer should convert a player model into a player entity
        /// and insert it in table storage if it doesn't exist
        /// </summary>
        [TestMethod]
        public void SavePlayer_NoExistingPlayerRecord_Should_InsertPlayerEntity()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(player.Name, entities[0].Name);
        }

        /// <summary>
        /// SavePlayer - if save a player when the player already exists in storage
        /// the values should be updated instead of duplicating the record or causing
        /// an exception
        /// </summary>
        [TestMethod]
        public void SavePlayer_WithExistingPlayerRecord_ShouldUpdateValues()
        {
            // orginal table save
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);

            // change something and tell it to create again, should update the table
            player.Name = "Test2";
            this.repository.SavePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(player.Name, entities[0].Name);
        }

        /// <summary>
        /// SavePlayerAll should convert a player model list into a player entity list
        /// and insert it in table storage if it doesn't exist
        /// </summary>
        [TestMethod]
        public void SavePlayerAll_NoExistingPlayerRecords_Should_InsertPlayerEntities()
        {
            var tableId = Guid.NewGuid();
            var players = new List<Player>
            {
                new Player("Player1"),
                new Player("Player2")
            };

            this.repository.SavePlayerAll(tableId, players);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(2, entities.Count());
            Assert.AreEqual(players[0].Name, entities.Single(x => x.PlayerId == players[0].ID.ToString()).Name);
            Assert.AreEqual(players[1].Name, entities.Single(x => x.PlayerId == players[1].ID.ToString()).Name);
        }

        /// <summary>
        /// SavePlayerAll - all the players already exist in storage
        /// the values should be updated instead of duplicating the records
        /// or causing an exception
        /// </summary>
        [TestMethod]
        public void SavePlayerAll_AllPlayersExist_Should_UpdatePlayerEntities()
        {
            // orginal table save
            var tableId = Guid.NewGuid();
            var players = new List<Player>
            {
                new Player("Player1"),
                new Player("Player2")
            };

            this.repository.SavePlayerAll(tableId, players);

            // change something and tell it to create again, should update the table
            players[0].Name = "Player1-1";
            players[1].Name = "Player2-1";

            this.repository.SavePlayerAll(tableId, players);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(2, entities.Count());
            Assert.AreEqual(players[0].Name, entities.Single(x => x.PlayerId == players[0].ID.ToString()).Name);
            Assert.AreEqual(players[1].Name, entities.Single(x => x.PlayerId == players[1].ID.ToString()).Name);
        }

        /// <summary>
        /// Removing a player should remove the player entity from table storage
        /// </summary>
        [TestMethod]
        public void RemovePlayer_Should_RemoveFromStorage()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);
            this.repository.RemovePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(0, entities.Count());
        }

        /// <summary>
        /// SaveSeat when the seat does not exist it should be inserted
        /// </summary>
        [TestMethod]
        public void SaveSeat_NoExisitingSeat_Should_InsertSeatEntity()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat() { Id = 1 };
            this.repository.SaveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(seat.Id, entities[0].SeatId);
        }

        /// <summary>
        /// SaveSeat and the seat exists it should update the seat
        /// </summary>
        [TestMethod]
        public void SaveSeat_WithExisitingSeat_Should_UpdateValues()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat() { Id = 1 };
            this.repository.SaveSeat(tableId, seat);

            seat.PlayerId = Guid.NewGuid();
            this.repository.SaveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(seat.PlayerId.Value.ToString(), entities[0].PlayerId);
        }

        /// <summary>
        /// SaveSeatAll and none of the seats exist it should insert all of them
        /// </summary>
        [TestMethod]
        public void SaveSeatAll_NoExisitingSeatRecords_Should_InsertSeatEntities()
        {
            var tableId = Guid.NewGuid();
            List<Seat> seats = new List<Seat>
            {
                new Seat() { Id = 1, PlayerId = Guid.NewGuid() },
                new Seat() { Id = 2, PlayerId = Guid.NewGuid() },
            };
            this.repository.SaveSeatAll(tableId, seats);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(seats[0].PlayerId.Value.ToString(), entities.Single(x => x.SeatId == seats[0].Id).PlayerId);
            Assert.AreEqual(seats[1].PlayerId.Value.ToString(), entities.Single(x => x.SeatId == seats[1].Id).PlayerId); 
        }

        /// <summary>
        /// SaveSeatAll and all the seats exist all of them should be updated
        /// </summary>
        [TestMethod]
        public void SaveSeatAll_AllSeatsExist_Should_UpdatePlayerEntities()
        {
            var tableId = Guid.NewGuid();
            List<Seat> seats = new List<Seat>
            {
                new Seat() { Id = 1, PlayerId = Guid.NewGuid() },
                new Seat() { Id = 2, PlayerId = Guid.NewGuid() },
            };
            this.repository.SaveSeatAll(tableId, seats);

            seats[0].IsDealer = true;
            seats[1].IsDealer = true;
            this.repository.SaveSeatAll(tableId, seats);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(seats[0].IsDealer, entities.Single(x => x.SeatId == seats[0].Id).IsDealer);
            Assert.AreEqual(seats[1].IsDealer, entities.Single(x => x.SeatId == seats[1].Id).IsDealer); 
        }

        /// <summary>
        /// RemoveSeat should remove the seat from table storage
        /// </summary>
        [TestMethod]
        public void RemoveSeat_Should_RemoveFromStorage()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat() { Id = 1 };
            this.repository.SaveSeat(tableId, seat);
            this.repository.RemoveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(0, entities.Count());
        }

        /// <summary>
        /// Load Table, table id does not exist, should throw TableDoesNotExistException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TableDoesNotExistException))]
        public void LoadTable_Invalid_TableId_Should_Throw_TableDoesNotExistExcpetion()
        {
            this.repository.LoadTable(Guid.NewGuid());
        }

        /// <summary>
        /// Load Table, table exists, should load table
        /// </summary>
        [TestMethod]
        public void LoadTable_TableExists_Should_Load_Table()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.IsNotNull(table);
            Assert.AreEqual(tableId, table.Id);
        }

        /// <summary>
        /// Load Table, table exists, should load seats
        /// </summary>
        [TestMethod]
        public void LoadTable_TableExists_Should_Load_Seats()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.AreEqual(5, table.Seats.Count());
        }

        /// <summary>
        /// Load Table, table exists, should load players
        /// </summary>
        [TestMethod]
        public void LoadTable_TableExists_Should_Load_Players()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.AreEqual(5, table.Players.Count());
        }

        /// <summary>
        /// Creates a new table with 5 seats and 5 players.  Saves table to storage
        /// </summary>
        /// <returns>returns GUID of table in storage</returns>
        private Guid CreateTableInStorage()
        {
            var engine = new Engine(new AzureRepository());
            engine.CreateNewTable(5, "Test", "TestPassword");
            for (int i = 0; i < 5; i++)
            {
                engine.AddPlayer(new Player(string.Format("Dave{0}", i + 1)));
            }

            return engine.Table.Id;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="T">the type of TableEntity</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKeyPrefix">The row key prefix.</param>
        /// <returns>returns all entities of T</returns>
        private List<T> GetEntities<T>(string partitionKey, string rowKeyPrefix)
            where T : TableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            var entities = this.table.ExecuteQuery(query).ToList();

            return entities.Where(x => x.RowKey.StartsWith(rowKeyPrefix)).ToList();
        }
    }
}
