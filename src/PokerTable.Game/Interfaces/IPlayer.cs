﻿using System;
using System.Collections.Generic;
using PokerTable.Game.Models;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Player Object
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        List<Card> Cards { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>        
        Player.States State { get; set; }
    }
}
