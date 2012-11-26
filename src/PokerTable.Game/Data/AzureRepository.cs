using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
        /// Adds the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        public void AddPlayer(Guid tableId, Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        public void SavePlayer(Guid tableId, Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the player all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="players">The players.</param>
        public void SavePlayerAll(Guid tableId, List<Player> players)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        public void RemovePlayer(Guid tableId, Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        public void AddSeat(Guid tableId, Seat seat)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        public void SaveSeat(Guid tableId, Seat seat)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the seat all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seats">The seats.</param>
        public void SaveSeatAll(Guid tableId, List<Seat> seats)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        public void DeleteSeat(Guid tableId, Seat seat)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the deck.
        /// </summary>
        /// <param name="table">The table.</param>
        public void SaveDeck(Table table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns the table
        /// </returns>
        public ITable LoadTable(Guid tableId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the new table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void SaveNewTable(Table table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void SaveTable(Table table)
        {
            throw new NotImplementedException();
        }
    }
}
