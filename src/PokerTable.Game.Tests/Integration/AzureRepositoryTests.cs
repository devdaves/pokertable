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
        /// and store insert it in table storage if it doesn't exist
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
    }
}
