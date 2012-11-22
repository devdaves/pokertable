using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game
{
    /// <summary>
    /// Deck Object
    /// </summary>
    public class Deck : IDeck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Deck" /> class.
        /// </summary>
        public Deck()
        {
            this.BuildDeck();
            this.ShuffleDeck();
        }

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
        /// <value>
        /// The cards.
        /// </value>
        public List<ICard> Cards { get; set; }

        /// <summary>
        /// Deals the card.
        /// </summary>
        /// <returns>returns the card dealt from the deck</returns>
        public ICard DealCard()
        {
            var c = this.Cards.FirstOrDefault(x => x.State == Card.States.Available);
            if (c == null)
            {
                throw new NoAvailableCardsException();
            }

            c.State = Card.States.Dealt;
            return c;
        }

        /// <summary>
        /// Builds the deck.
        /// </summary>
        public void BuildDeck()
        {
            this.Cards = new List<ICard>();

            for (int s = 1; s < 5; s++)
            {
                for (int v = 1; v < 14; v++)
                {
                    this.Cards.Add(new Card() { Suite = (Card.Suites)s, Color = (Card.Colors)(s % 2), State = Card.States.Available, Value = v });
                }
            }
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void ShuffleDeck()
        {
            this.Cards = this.Cards.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
