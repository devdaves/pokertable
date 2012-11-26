using System;
using System.Collections.Generic;
using PokerTable.Game.Models;

namespace PokerTable.Game.Interfaces
{
    /// <summary>
    /// Deck Object
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        List<Card> Cards { get; set; }
    }
}
