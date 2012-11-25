using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Repository Object
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        void AddPlayer(Guid tableId, IPlayer player);

        /// <summary>
        /// Saves the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        void SavePlayer(Guid tableId, IPlayer player);

        /// <summary>
        /// Saves the player all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="players">The players.</param>
        void SavePlayerAll(Guid tableId, List<IPlayer> players);

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="player">The player.</param>
        void RemovePlayer(Guid tableId, IPlayer player);

        /// <summary>
        /// Adds the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        void AddSeat(Guid tableId, ISeat seat);

        /// <summary>
        /// Saves the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        void SaveSeat(Guid tableId, ISeat seat);

        /// <summary>
        /// Saves the seat all.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seats">The seats.</param>
        void SaveSeatAll(Guid tableId, List<ISeat> seats);

        /// <summary>
        /// Deletes the seat.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seat">The seat.</param>
        void DeleteSeat(Guid tableId, ISeat seat);

        /// <summary>
        /// Saves the deck.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="deck">The deck.</param>
        void SaveDeck(Guid tableId, IDeck deck);

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns the table</returns>
        Table LoadTable(Guid tableId);

        /// <summary>
        /// Saves the new table.
        /// </summary>
        /// <param name="table">The table.</param>
        void SaveNewTable(ITable table);

        /// <summary>
        /// Saves the table.
        /// </summary>
        /// <param name="table">The table.</param>
        void SaveTable(ITable table);

        /// <summary>
        /// Updates the table last modified.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        void UpdateTableLastModified(Guid tableId);
    }
}
