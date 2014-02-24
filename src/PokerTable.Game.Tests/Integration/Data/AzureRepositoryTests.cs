using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Data;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Integration.Data
{
    [TestClass]
    public class AzureRepositoryTests
    {
        private CloudTableClient client;

        private CloudTable cloudTable;

        private IRepository repository;

        [TestInitialize]
        public void Setup()
        {
            try
            {
                var connectionString = ConfigurationManager.AppSettings[AzureRepository.AzureStorageConnectionStringKey];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                this.client = storageAccount.CreateCloudTableClient();
                this.cloudTable = this.client.GetTableReference(AzureRepository.AzureTableName);
                this.cloudTable.CreateIfNotExists();
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

        [TestCleanup]
        public void Cleanup()
        {
            this.cloudTable.DeleteIfExists();
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveTable_NoExisitingTableRecord_Should_InsertPokerTableEntity()
        {
            var table = new Table("Test", "TestPassword");
            this.repository.SaveTable(table);

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), PokerTableEntity.Prefix);

            Assert.AreEqual(table.Id.ToString(), entities[0].PartitionKey);
            Assert.AreEqual(table.Name, entities[0].Name);
            Assert.AreEqual(table.Password, entities[0].Password);
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveTable_WithExistingTableRecord_Should_UpdateValues()
        {
            // orginal cloudTable save
            var table = new Table("Test", "TestPassword");
            this.repository.SaveTable(table);

            // change something and tell it to create again, should update the cloudTable
            table.Password = "TestPassword2";
            this.repository.SaveTable(table);

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), PokerTableEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(table.Password, entities[0].Password);
        }

        [TestMethod, TestCategory("Integration")]
        public void SavePlayer_NoExistingPlayerRecord_Should_InsertPlayerEntity()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(player.Name, entities[0].Name);
        }

        [TestMethod, TestCategory("Integration")]
        public void SavePlayer_WithExistingPlayerRecord_ShouldUpdateValues()
        {
            // orginal cloudTable save
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);

            // change something and tell it to create again, should update the cloudTable
            player.Name = "Test2";
            this.repository.SavePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(player.Name, entities[0].Name);
        }

        [TestMethod, TestCategory("Integration")]
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
            Assert.AreEqual(players[0].Name, entities.Single(x => x.PlayerId == players[0].Id.ToString()).Name);
            Assert.AreEqual(players[1].Name, entities.Single(x => x.PlayerId == players[1].Id.ToString()).Name);
        }

        [TestMethod, TestCategory("Integration")]
        public void SavePlayerAll_AllPlayersExist_Should_UpdatePlayerEntities()
        {
            // orginal cloudTable save
            var tableId = Guid.NewGuid();
            var players = new List<Player>
            {
                new Player("Player1"),
                new Player("Player2")
            };

            this.repository.SavePlayerAll(tableId, players);

            // change something and tell it to create again, should update the cloudTable
            players[0].Name = "Player1-1";
            players[1].Name = "Player2-1";

            this.repository.SavePlayerAll(tableId, players);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(2, entities.Count());
            Assert.AreEqual(players[0].Name, entities.Single(x => x.PlayerId == players[0].Id.ToString()).Name);
            Assert.AreEqual(players[1].Name, entities.Single(x => x.PlayerId == players[1].Id.ToString()).Name);
        }

        [TestMethod, TestCategory("Integration")]
        public void RemovePlayer_Should_RemoveFromStorage()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            this.repository.SavePlayer(tableId, player);
            this.repository.RemovePlayer(tableId, player);

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            Assert.AreEqual(0, entities.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveSeat_NoExisitingSeat_Should_InsertSeatEntity()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat { Id = 1 };
            this.repository.SaveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(seat.Id, entities[0].SeatId);
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveSeat_WithExisitingSeat_Should_UpdateValues()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat { Id = 1 };
            this.repository.SaveSeat(tableId, seat);

            seat.PlayerId = Guid.NewGuid();
            this.repository.SaveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(1, entities.Count());
            Assert.AreEqual(seat.PlayerId.Value.ToString(), entities[0].PlayerId);
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveSeatAll_NoExisitingSeatRecords_Should_InsertSeatEntities()
        {
            var tableId = Guid.NewGuid();
            var seats = new List<Seat>
            {
                new Seat { Id = 1, PlayerId = Guid.NewGuid() },
                new Seat { Id = 2, PlayerId = Guid.NewGuid() },
            };
            this.repository.SaveSeatAll(tableId, seats);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);
            Assert.IsNotNull(seats[0].PlayerId);
            Assert.IsNotNull(seats[1].PlayerId);
            Assert.AreEqual(seats[0].PlayerId.Value.ToString(), entities.Single(x => x.SeatId == seats[0].Id).PlayerId);
            Assert.AreEqual(seats[1].PlayerId.Value.ToString(), entities.Single(x => x.SeatId == seats[1].Id).PlayerId);
        }

        [TestMethod, TestCategory("Integration")]
        public void SaveSeatAll_AllSeatsExist_Should_UpdatePlayerEntities()
        {
            var tableId = Guid.NewGuid();
            var seats = new List<Seat>
            {
                new Seat { Id = 1, PlayerId = Guid.NewGuid() },
                new Seat { Id = 2, PlayerId = Guid.NewGuid() },
            };
            this.repository.SaveSeatAll(tableId, seats);

            seats[0].IsDealer = true;
            seats[1].IsDealer = true;
            this.repository.SaveSeatAll(tableId, seats);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(seats[0].IsDealer, entities.Single(x => x.SeatId == seats[0].Id).IsDealer);
            Assert.AreEqual(seats[1].IsDealer, entities.Single(x => x.SeatId == seats[1].Id).IsDealer);
        }

        [TestMethod, TestCategory("Integration")]
        public void RemoveSeat_Should_RemoveFromStorage()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat { Id = 1 };
            this.repository.SaveSeat(tableId, seat);
            this.repository.RemoveSeat(tableId, seat);

            var entities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);

            Assert.AreEqual(0, entities.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public void DeleteOldTables_Should_Delete_The_OldTables_And_Thier_Players_And_Seats()
        {
            // create cloudTable, seat and player
            var tableId = Guid.NewGuid();
            var pokerTableEntity = new PokerTableEntity(tableId) { LastUpdatedUtc = DateTime.UtcNow.AddDays(-2) };
            var seatEntity = new SeatEntity(tableId, 1);
            var playerEntity = new PlayerEntity(tableId, Guid.NewGuid());

            // manually insert cloudTable, seat and player
            this.cloudTable.Execute(TableOperation.InsertOrMerge(pokerTableEntity));
            this.cloudTable.Execute(TableOperation.InsertOrMerge(seatEntity));
            this.cloudTable.Execute(TableOperation.InsertOrMerge(playerEntity));

            this.repository.DeleteOldTables();

            Assert.AreEqual(0, this.GetEntities<PokerTableEntity>(tableId.ToString(), PokerTableEntity.Prefix).Count());
            Assert.AreEqual(0, this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix).Count());
            Assert.AreEqual(0, this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix).Count());
        }

        [TestMethod, TestCategory("Integration")]
        [ExpectedException(typeof(TableDoesNotExistException))]
        public void LoadTable_Invalid_TableId_Should_Throw_TableDoesNotExistExcpetion()
        {
            this.repository.LoadTable(Guid.NewGuid());
        }

        [TestMethod, TestCategory("Integration")]
        public void LoadTable_TableExists_Should_Load_Table()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.IsNotNull(table);
            Assert.AreEqual(tableId, table.Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void LoadTable_TableExists_Should_Load_Seats()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.AreEqual(5, table.Seats.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public void LoadTable_TableExists_Should_Load_Players()
        {
            var tableId = this.CreateTableInStorage();
            var table = this.repository.LoadTable(tableId);

            Assert.AreEqual(5, table.Players.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public void TablePasswordExists_NotInTable_Should_Return_False()
        {
            const string code = "abcxy";
            var result = this.repository.TablePasswordExists(code);
            Assert.IsFalse(result);
        }

        [TestMethod, TestCategory("Integration")]
        public void TablePasswordExists_InTable_Should_Return_True()
        {
            const string code = "abcxy";
            var table = new Table("Test", code);
            this.repository.SaveTable(table);

            var result = this.repository.TablePasswordExists(code);

            Assert.IsTrue(result);
        }

        [TestMethod, TestCategory("Integration")]
        public void GetTableIdByTablePassword_NotInTable_Should_Return_Null()
        {
            const string code = "abcxy";
            var result = this.repository.GetTableIdByTablePassword(code);
            Assert.IsNull(result);
        }

        [TestMethod, TestCategory("Integration")]
        public void GetTableIdByTablePassword_InTable_Should_Return_CorrectTableId()
        {
            const string code = "abcxy";
            var table = new Table("Test", code);
            this.repository.SaveTable(table);

            var result = this.repository.GetTableIdByTablePassword(code);

            Assert.IsNotNull(result);
            Assert.AreEqual(table.Id, result.Value);
        }

        private Guid CreateTableInStorage()
        {
            var engine = new Engine(new AzureRepository(), new DeckBuilder(), new Dealer());
            engine.CreateNewTable(5, "Test");
            for (int i = 0; i < 5; i++)
            {
                engine.AddPlayer(new Player(string.Format("Dave{0}", i + 1)));
            }

            return engine.Table.Id;
        }

        private List<T> GetEntities<T>(string partitionKey, string rowKeyPrefix)
            where T : TableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            var entities = this.cloudTable.ExecuteQuery(query).ToList();

            return entities.Where(x => x.RowKey.StartsWith(rowKeyPrefix)).ToList();
        }
    }
}
