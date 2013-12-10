using System.Text;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests
{
    public static class Utils
    {
        public static string DeckCardsToString(Deck deck)
        {
            var sb = new StringBuilder();
            foreach (var card in deck.Cards)
            {
                sb.Append(string.Format("{0},", card.Name()));
            }

            return sb.ToString();
        }
    }
}
