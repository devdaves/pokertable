using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Models
{
    /// <summary>
    /// Table Object
    /// </summary>
    public class Table : ITable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="tablePassword">The table password.</param>
        public Table(string tableName, string tablePassword)
        {
            this.Id = Guid.NewGuid();
            this.Name = tableName;
            this.Password = tablePassword;
            this.Deck = new Deck();
            this.Seats = new List<Seat>();
            this.Players = new List<Player>();
            this.Burn = new List<Card>();
            this.PublicCards = new List<Card>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the deck.
        /// </summary>
        /// <value>
        /// The deck.
        /// </value>
        public Deck Deck { get; set; }

        /// <summary>
        /// Gets or sets the seats.
        /// </summary>
        /// <value>
        /// The seats.
        /// </value>
        public List<Seat> Seats { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Gets or sets the burn.
        /// </summary>
        /// <value>
        /// The burn.
        /// </value>
        public List<Card> Burn { get; set; }

        /// <summary>
        /// Gets or sets the public cards.
        /// </summary>
        /// <value>
        /// The public cards.
        /// </value>
        public List<Card> PublicCards { get; set; }

        /// <summary>
        /// Gets a value indicating whether [reset table available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reset table available]; otherwise, <c>false</c>.
        /// </value>
        public bool ResetTableAvailable
        {
            get { return this.CalculateResetTableAvailable(); }
        }

        /// <summary>
        /// Gets a value indicating whether [deal players available].
        /// </summary>
        /// <value>
        /// <c>true</c> if [deal players available]; otherwise, <c>false</c>.
        /// </value>
        public bool DealPlayersAvailable
        {
            get { return this.CalculateDealPlayersAvailable(); }
        }

        /// <summary>
        /// Gets a value indicating whether [deal flop available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal flop available]; otherwise, <c>false</c>.
        /// </value>
        public bool DealFlopAvailable
        {
            get { return this.CalculateDealFlopAvailable(); }
        }

        /// <summary>
        /// Gets a value indicating whether [deal turn available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal turn available]; otherwise, <c>false</c>.
        /// </value>
        public bool DealTurnAvailable
        {
            get { return this.CalculateDealTurnAvailable(); }
        }

        /// <summary>
        /// Gets a value indicating whether [deal river available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [deal river available]; otherwise, <c>false</c>.
        /// </value>
        public bool DealRiverAvailable
        {
            get { return this.CalculateDealRiverAvailable(); }
        }

        /// <summary>
        /// Calculates the reset table available.
        /// </summary>
        /// <returns>returns true or false if available</returns>
        private bool CalculateResetTableAvailable()
        {
            return true;
        }

        /// <summary>
        /// Calculates the deal players available.
        /// </summary>
        /// <returns>returns true or false if available</returns>
        private bool CalculateDealPlayersAvailable()
        {
            return !this.Players.Any(x => x.Cards.Count() > 0);
        }

        /// <summary>
        /// Calculates the deal flop available.
        /// </summary>
        /// <returns>returns true or false if available</returns>
        private bool CalculateDealFlopAvailable()
        {
            return this.PublicCards.Count() == 0 && this.DealPlayersAvailable == false;
        }

        /// <summary>
        /// Calculates the deal turn available.
        /// </summary>
        /// <returns>returns true or false if available</returns>
        private bool CalculateDealTurnAvailable()
        {
            return this.PublicCards.Count() == 3;
        }

        /// <summary>
        /// Calculates the deal river available.
        /// </summary>
        /// <returns>returns true or false if available</returns>
        private bool CalculateDealRiverAvailable()
        {
            return this.PublicCards.Count() == 4;
        }
    }
}
