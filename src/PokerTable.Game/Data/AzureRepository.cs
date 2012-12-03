using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Extensions;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Data
{
    /// <summary>
    /// Azure Repository
    /// </summary>
    internal class AzureRepository : IRepository
    {
        /// <summary>
        /// Azure Table Name
        /// </summary>
        internal const string AzureTableName = "PokerTable";

        /// <summary>
        /// Azure Storage Connection String Key
        /// </summary>
        internal const string AzureStorageConnectionStringKey = "AzureStorageConnectionString";

        /// <summary>
        /// azure client field
        /// </summary>
        private CloudTableClient client;

        /// <summary>
        /// azure table field
        /// </summary>
        private CloudTable table;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureRepository" /> class.
        /// </summary>
        public AzureRepository()
        {
            var connectionString = ConfigurationManager.AppSettings[AzureStorageConnectionStringKey];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            this.client = storageAccount.CreateCloudTableClient();
            this.table = this.client.GetTableReference(AzureTableName);
            this.table.CreateIfNotExists();
        }

        /// <summary>
        /// Saves the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        public void SavePlayer(Guid tableId, Player player)
        {
            var entity = player.ToPlayerEntity(tableId);
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.table.Execute(insertOperation);
        }

        /// <summary>
        /// Saves the player all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="players">The players.</param>
        public void SavePlayerAll(Guid tableId, List<Player> players)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var player in players)
            {
                var entity = player.ToPlayerEntity(tableId);
                batchOperation.InsertOrMerge(entity);
            }

            this.table.ExecuteBatch(batchOperation);
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        public void RemovePlayer(Guid tableId, Player player)
        {
            var entity = player.ToPlayerEntity(tableId);
            entity.ETag = "*";
            TableOperation deleteOperation = TableOperation.Delete(entity);
            this.table.Execute(deleteOperation);
        }

        /// <summary>
        /// Saves the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        public void SaveSeat(Guid tableId, Seat seat)
        {
            var entity = seat.ToSeatEntity(tableId);
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.table.Execute(insertOperation);
        }

        /// <summary>
        /// Saves the seat all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seats">The seats.</param>
        public void SaveSeatAll(Guid tableId, List<Seat> seats)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var seat in seats)
            {
                var entity = seat.ToSeatEntity(tableId);
                batchOperation.InsertOrMerge(entity);
            }

            this.table.ExecuteBatch(batchOperation);
        }

        /// <summary>
        /// Removes the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        public void RemoveSeat(Guid tableId, Seat seat)
        {
            var entity = seat.ToSeatEntity(tableId);
            entity.ETag = "*";
            TableOperation deleteOperation = TableOperation.Delete(entity);
            this.table.Execute(deleteOperation);
        }

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns the table
        /// </returns>
        public Table LoadTable(Guid tableId)
        {
            var tableEntities = this.GetEntities<PokerTableEntity>(tableId.ToString(), PokerTableEntity.Prefix);
            var seatEntities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);
            var playerEntites = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            if (tableEntities.Count() == 0)
            {
                throw new TableDoesNotExistException();
            }

            var pokerTable = tableEntities[0].ToTableModel();
            pokerTable.Seats = seatEntities.ToSeatModelList();
            pokerTable.Players = playerEntites.ToPlayerModelList();
            return pokerTable;
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void SaveTable(Table table)
        {
            var entity = table.ToPokerTableEntity();
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.table.Execute(insertOperation);
        }

        /// <summary>
        /// Deletes the old tables.
        /// </summary>
        public void DeleteOldTables()
        {
            // Get a list of tables that have not been updated in the last 1 days
            TableQuery<PokerTableEntity> pokerTableQuery = new TableQuery<PokerTableEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, PokerTableEntity.Prefix),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("LastUpdatedUTC", QueryComparisons.LessThan, DateTime.UtcNow.AddDays(-1))));
            var pokerTableEntities = this.table.ExecuteQuery(pokerTableQuery).ToList();

            if (pokerTableEntities.Count() > 0)
            {
                foreach (var pokerTableEntity in pokerTableEntities)
                {
                    TableBatchOperation batchOperations = new TableBatchOperation();

                    // get the seats for this table and put them in the list to delete
                    this.GetEntities<SeatEntity>(pokerTableEntity.PartitionKey, SeatEntity.Prefix).ForEach(x => batchOperations.Delete(x));

                    // get the players for this table and put them in the list to delete
                    this.GetEntities<PlayerEntity>(pokerTableEntity.PartitionKey, PlayerEntity.Prefix).ForEach(x => batchOperations.Delete(x));

                    // put the table in the list to delete
                    batchOperations.Delete(pokerTableEntity);

                    this.table.ExecuteBatch(batchOperations);
                }               
            }
        }

        /// <summary>
        /// Tables the password exists.
        /// </summary>
        /// <param name="tablePassword">The table password.</param>
        /// <returns>
        /// returns true if the password exists
        /// </returns>
        public bool TablePasswordExists(string tablePassword)
        {
            var query = new TableQuery<PokerTableEntity>().Where(
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, tablePassword));
            var entites = this.table.ExecuteQuery(query).ToList();

            return entites.Count() > 0;
        }

        /// <summary>
        /// Gets the table id by table password.
        /// </summary>
        /// <param name="tablePassword">The table password.</param>
        /// <returns>returns the GUID of the table null if it does not exist</returns>
        public Guid? GetTableIdByTablePassword(string tablePassword)
        {
            var query = new TableQuery<PokerTableEntity>().Where(
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, tablePassword));
            var entites = this.table.ExecuteQuery(query).ToList();

            if (entites.Count() > 0)
            {
                return Guid.Parse(entites[0].PartitionKey);
            }

            return null;
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
