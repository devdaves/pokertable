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
        /// Azure Table Name
        /// </summary>
        private const string AzureTableName = "PokerTable";

        /// <summary>
        /// Azure Storage Connection String Key
        /// </summary>
        private const string AzureStorageConnectionStringKey = "AzureStorageConnectionString";

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
                var connectionString = ConfigurationManager.AppSettings[AzureStorageConnectionStringKey];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                this.client = storageAccount.CreateCloudTableClient();
                this.table = this.client.GetTableReference(AzureTableName);
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

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), "pokertable");

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

            var entities = this.GetEntities<PokerTableEntity>(table.Id.ToString(), "pokertable");

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

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), string.Format("Player-{0}", player.ID));

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

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), string.Format("Player-{0}", player.ID));

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

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString());

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

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString());

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

            var entities = this.GetEntities<PlayerEntity>(tableId.ToString(), string.Format("Player-{0}", player.ID));

            Assert.AreEqual(0, entities.Count());
        }

        /// <summary>
        /// SaveSeat when the seat does not exist it should be inserted
        /// </summary>
        [TestMethod]
        public void SaveSeat_NoExisitingSeat_Should_InsertSeatEntity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SaveSeat and the seat exists it should update the seat
        /// </summary>
        [TestMethod]
        public void SaveSeat_WithExisitingSeat_Should_UpdateValues()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SaveSeatAll and none of the seats exist it should insert all of them
        /// </summary>
        [TestMethod]
        public void SaveSeatAll_NoExisitingSeatRecords_Should_InsertSeatEntities()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SaveSeatAll and all the seats exist all of them should be updated
        /// </summary>
        [TestMethod]
        public void SaveSeatAll_AllSeatsExist_Should_UpdatePlayerEntities()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RemoveSeat should remove the seat from table storage
        /// </summary>
        [TestMethod]
        public void RemoveSeat_Should_RemoveFromStorage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entities from azure storage
        /// </summary>
        /// <typeparam name="T">the type of TableEntity</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>returns a List of T</returns>
        private List<T> GetEntities<T>(string partitionKey, string rowKey)
            where T : TableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            
            return this.table.ExecuteQuery(query).ToList();
        }

        /// <summary>
        /// Gets the entities from azure storage
        /// </summary>
        /// <typeparam name="T">the type of TableEntity</typeparam>
        /// <param name="partitionKey">The partition key.</param>
        /// <returns>returns all entities of T</returns>
        private List<T> GetEntities<T>(string partitionKey)
            where T : TableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>().Where(                    
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            return this.table.ExecuteQuery(query).ToList();
        }
    }
}
