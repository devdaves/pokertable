using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Models;

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
        Deck Deck { get; set; }

        /// <summary>
        /// Gets or sets the seats.
        /// </summary>
        /// <value>
        /// The seats.
        /// </value>
        List<Seat> Seats { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        List<Player> Players { get; set; }

        /// <summary>
        /// Gets or sets the burn.
        /// </summary>
        /// <value>
        /// The burn.
        /// </value>
        List<Card> Burn { get; set; }

        /// <summary>
        /// Gets or sets the public cards.
        /// </summary>
        /// <value>
        /// The public cards.
        /// </value>
        List<Card> PublicCards { get; set; }

        /// <summary>
        /// Gets a value indicating whether [reset table available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reset table available]; otherwise, <c>false</c>.
        /// </value>
        bool ResetTableAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether [deal players available].
        /// </summary>
        /// <value>
        /// <c>true</c> if [deal players available]; otherwise, <c>false</c>.
        /// </value>
        bool DealPlayersAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether [deal flop available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal flop available]; otherwise, <c>false</c>.
        /// </value>
        bool DealFlopAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether [deal turn available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal turn available]; otherwise, <c>false</c>.
        /// </value>
        bool DealTurnAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether [deal river available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal river available]; otherwise, <c>false</c>.
        /// </value>
        bool DealRiverAvailable { get; }
    }
}
