using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Extensions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Data
{
    public class AzureRepository : IRepository
    {
        internal const string AzureTableName = "PokerTable";

        internal const string AzureStorageConnectionStringKey = "AzureStorageConnectionString";

        private readonly CloudTable cloudTable;

        public AzureRepository()
        {
            var connectionString = ConfigurationManager.AppSettings[AzureStorageConnectionStringKey];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudTableClient();
         
            this.cloudTable = client.GetTableReference(AzureTableName);
            this.cloudTable.CreateIfNotExists();
        }

        public void SavePlayer(Guid tableId, Player player)
        {
            var entity = player.ToPlayerEntity(tableId);
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.cloudTable.Execute(insertOperation);
        }

        public void SavePlayerAll(Guid tableId, List<Player> players)
        {
            var batchOperation = new TableBatchOperation();
            foreach (var player in players)
            {
                var entity = player.ToPlayerEntity(tableId);
                batchOperation.InsertOrMerge(entity);
            }

            this.cloudTable.ExecuteBatch(batchOperation);
        }

        public void RemovePlayer(Guid tableId, Player player)
        {
            var entity = player.ToPlayerEntity(tableId);
            entity.ETag = "*";
            TableOperation deleteOperation = TableOperation.Delete(entity);
            this.cloudTable.Execute(deleteOperation);
        }

        public void SaveSeat(Guid tableId, Seat seat)
        {
            var entity = seat.ToSeatEntity(tableId);
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.cloudTable.Execute(insertOperation);
        }

        public void SaveSeatAll(Guid tableId, List<Seat> seats)
        {
            var batchOperation = new TableBatchOperation();
            foreach (var seat in seats)
            {
                var entity = seat.ToSeatEntity(tableId);
                batchOperation.InsertOrMerge(entity);
            }

            this.cloudTable.ExecuteBatch(batchOperation);
        }

        public void RemoveSeat(Guid tableId, Seat seat)
        {
            var entity = seat.ToSeatEntity(tableId);
            entity.ETag = "*";
            TableOperation deleteOperation = TableOperation.Delete(entity);
            this.cloudTable.Execute(deleteOperation);
        }

        public Table LoadTable(Guid tableId)
        {
            var tableEntities = this.GetEntities<PokerTableEntity>(tableId.ToString(), PokerTableEntity.Prefix);
            var seatEntities = this.GetEntities<SeatEntity>(tableId.ToString(), SeatEntity.Prefix);
            var playerEntites = this.GetEntities<PlayerEntity>(tableId.ToString(), PlayerEntity.Prefix);

            if (!tableEntities.Any())
            {
                throw new TableDoesNotExistException();
            }

            var pokerTable = tableEntities[0].ToTableModel();
            pokerTable.Seats = seatEntities.ToSeatModelList();
            pokerTable.Players = playerEntites.ToPlayerModelList();
            return pokerTable;
        }

        public void SaveTable(Table table)
        {
            var entity = table.ToPokerTableEntity();
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            this.cloudTable.Execute(insertOperation);
        }

        public void DeleteOldTables()
        {
            // Get a list of tables that have not been updated in the last 1 days
            TableQuery<PokerTableEntity> pokerTableQuery = new TableQuery<PokerTableEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, PokerTableEntity.Prefix),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForDate("LastUpdatedUtc", QueryComparisons.LessThan, DateTime.UtcNow.AddDays(-1))));
            var pokerTableEntities = this.cloudTable.ExecuteQuery(pokerTableQuery).ToList();

            if (pokerTableEntities.Any())
            {
                foreach (var pokerTableEntity in pokerTableEntities)
                {
                    var batchOperations = new TableBatchOperation();

                    // get the seats for this cloudTable and put them in the list to delete
                    this.GetEntities<SeatEntity>(pokerTableEntity.PartitionKey, SeatEntity.Prefix).ForEach(batchOperations.Delete);

                    // get the players for this cloudTable and put them in the list to delete
                    this.GetEntities<PlayerEntity>(pokerTableEntity.PartitionKey, PlayerEntity.Prefix).ForEach(batchOperations.Delete);

                    // put the cloudTable in the list to delete
                    batchOperations.Delete(pokerTableEntity);

                    this.cloudTable.ExecuteBatch(batchOperations);
                }
            }
        }

        public bool TablePasswordExists(string tablePassword)
        {
            var query = new TableQuery<PokerTableEntity>().Where(
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, tablePassword));
            var entites = this.cloudTable.ExecuteQuery(query).ToList();

            return entites.Any();
        }

        public Guid? GetTableIdByTablePassword(string tablePassword)
        {
            var query = new TableQuery<PokerTableEntity>().Where(
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, tablePassword));
            var entites = this.cloudTable.ExecuteQuery(query).ToList();

            if (entites.Any())
            {
                return Guid.Parse(entites[0].PartitionKey);
            }

            return null;
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
