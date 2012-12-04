using System;
using PokerTable.Game.Models;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Engine Interface
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        Table Table { get; set; }

        /// <summary>
        /// Creates the new table.
        /// </summary>
        /// <param name="numberOfSeats">The number of seats.</param>
        /// <param name="name">The name.</param>
        void CreateNewTable(int numberOfSeats, string name);

        /// <summary>
        /// Loads the table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        void LoadTable(Guid tableId);

        /// <summary>
        /// Joins the table.
        /// </summary>
        /// <param name="tablePassword">The table password.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <returns>
        /// returns the GUID of the player
        /// </returns>
        Guid JoinTable(string tablePassword, string playerName);

        /// <summary>
        /// Assigns the seat to player.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns true if successful false if failed
        /// </returns>
        bool AssignSeatToPlayer(int seatId, Guid playerId);

        /// <summary>
        /// Removes the player from seat.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>returns true if successful false if failed</returns>
        bool RemovePlayerFromSeat(int seatId);

        /// <summary>
        /// Removes the player from seat.
        /// </summary>
        /// <param name="playerId">The player ID</param>
        /// <returns>
        /// returns true if successful false if failed
        /// </returns>
        bool RemovePlayerFromSeat(Guid playerId);

        /// <summary>
        /// Dealers the exists.
        /// </summary>
        /// <returns>
        /// returns true if dealer exists false if not
        /// </returns>
        bool DealerExists();

        /// <summary>
        /// Sets the dealer.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        void SetDealer(int seatId);

        /// <summary>
        /// Set dealer to next player
        /// </summary>
        void NextDealer();

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        void ShuffleDeck();

        /// <summary>
        /// Deals the players.
        /// </summary>
        void DealPlayers();

        /// <summary>
        /// Deals the flop.
        /// </summary>
        void DealFlop();

        /// <summary>
        /// Deals the turn.
        /// </summary>
        void DealTurn();

        /// <summary>
        /// Deals the river.
        /// </summary>
        void DealRiver();

        /// <summary>
        /// Folds the specified player id.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        void FoldPlayer(Guid playerId);

        /// <summary>
        /// Resets the deck, burn and public card lists, resets each player
        /// </summary>
        void ResetTable();

        /// <summary>
        /// Adds the seat to the table
        /// </summary>
        /// <param name="saveNow">if set to <c>true</c> [save now].</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        bool AddSeat(bool saveNow = true);

        /// <summary>
        /// Removes the seat from the table
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        bool RemoveSeat(int seatId);

        /// <summary>
        /// Adds the player to the table
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>
        /// returns true if player was added false if not
        /// </returns>
        bool AddPlayer(Player player);

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns true if player was removed false if not
        /// </returns>
        bool RemovePlayer(Guid playerId);    
    }
}
