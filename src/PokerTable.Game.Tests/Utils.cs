using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Tests
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Decks the cards to string.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <returns>returns a string representation of the cards in the deck.</returns>
        public static string DeckCardsToString(IDeck deck)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var card in deck.Cards)
            {
                sb.Append(string.Format("{0},", card.Name()));
            }

            return sb.ToString();
        }
    }
}
