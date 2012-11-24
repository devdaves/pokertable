using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Table Object
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the deck.
        /// </summary>
        /// <value>
        /// The deck.
        /// </value>
        IDeck Deck { get; set; }

        /// <summary>
        /// Gets or sets the seats.
        /// </summary>
        /// <value>
        /// The seats.
        /// </value>
        List<ISeat> Seats { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        List<IPlayer> Players { get; set; }

        /// <summary>
        /// Gets or sets the burn.
        /// </summary>
        /// <value>
        /// The burn.
        /// </value>
        List<ICard> Burn { get; set; }

        /// <summary>
        /// Gets or sets the public cards.
        /// </summary>
        /// <value>
        /// The public cards.
        /// </value>
        List<ICard> PublicCards { get; set; }

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
        /// Deals the players.
        /// </summary>
        void DealPlayers();

        /// <summary>
        /// Set dealer to next player
        /// </summary>
        void NextDealer();

        /// <summary>
        /// Assigns the seat to player.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        /// <param name="playerId">The player id.</param>
        /// <returns>returns true if successful false if failed</returns>
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
        /// <param name="playerId">The player id.</param>
        /// <returns>returns true if successful false if failed</returns>
        bool RemovePlayerFromSeat(Guid playerId);

        /// <summary>
        /// Dealers the exists.
        /// </summary>
        /// <returns>returns true if dealer exists false if not</returns>
        bool DealerExists();

        /// <summary>
        /// Sets the dealer.
        /// </summary>
        /// <param name="seatId">The seat id.</param>
        void SetDealer(int seatId);

        /// <summary>
        /// Adds the seat to the table
        /// </summary>
        /// <param name="persistChanges">if set to <c>true</c> [persist changes].</param>
        /// <returns>
        /// return true if successful, false if not
        /// </returns>
        bool AddSeat(bool persistChanges = true);

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
        bool AddPlayer(IPlayer player);

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns true if player was removed false if not
        /// </returns>
        bool RemovePlayer(Guid playerId);

        /// <summary>
        /// Resets the deck, burn and public card lists, resets each player
        /// </summary>
        void Reset();
    }
}
