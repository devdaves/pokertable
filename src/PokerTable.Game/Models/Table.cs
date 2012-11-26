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
    }
}
