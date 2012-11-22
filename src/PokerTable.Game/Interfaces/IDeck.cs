using System;
using System.Collections.Generic;
using PokerTable.Game.Interfaces;

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
        List<ICard> Cards { get; set; }

        /// <summary>
        /// Builds the deck.
        /// </summary>
        void BuildDeck();

        /// <summary>
        /// Deals the card.
        /// </summary>
        /// <returns>returns the card dealt from the deck</returns>
        ICard DealCard();

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        void ShuffleDeck();
    }
}
