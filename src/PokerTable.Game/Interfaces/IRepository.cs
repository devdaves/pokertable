using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Models;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Repository Object
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Saves the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        void SavePlayer(Guid tableId, Player player);

        /// <summary>
        /// Saves the player all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="players">The players.</param>
        void SavePlayerAll(Guid tableId, List<Player> players);

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        void RemovePlayer(Guid tableId, Player player);

        /// <summary>
        /// Saves the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        void SaveSeat(Guid tableId, Seat seat);

        /// <summary>
        /// Saves the seat all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seats">The seats.</param>
        void SaveSeatAll(Guid tableId, List<Seat> seats);

        /// <summary>
        /// Remove the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        void RemoveSeat(Guid tableId, Seat seat);

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns the table</returns>
        Table LoadTable(Guid tableId);

        /// <summary>
        /// Saves the table.
        /// </summary>
        /// <param name="table">The table.</param>
        void SaveTable(Table table);

        /// <summary>
        /// Deletes the old tables.
        /// </summary>
        void DeleteOldTables();
    }
}
