﻿using System;
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
    }
}
