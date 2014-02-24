using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Models;

namespace PokerTable.Game
{
    public interface IDeckBuilder
    {
        Deck Build();
    }

    public class DeckBuilder : IDeckBuilder
    {
        public Deck Build()
        {
            var deck = new Deck();

            for (var s = 1; s < 5; s++)
            {
                for (var v = 1; v < 14; v++)
                {
                    deck.Cards.Add(new Card { Suite = (Card.Suites)s, Color = (Card.Colors)(s % 2), State = Card.States.Available, Value = v });
                }
            }

            return deck;
        }
    }
}
