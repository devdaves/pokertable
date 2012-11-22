using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Exceptions;

namespace PokerTable.Game.Tests
{
    /// <summary>
    /// Deck Tests
    /// </summary>
    [TestClass]
    public class DeckTests
    {
        /// <summary>
        /// When creating a new deck it should have more then 0 cards
        /// </summary>
        [TestMethod]
        public void New_Deck_Should_Have_Cards()
        {
            var deck = new Deck();
            Assert.IsNotNull(deck);
            Assert.IsNotNull(deck.Cards);
            Assert.IsTrue(deck.Cards.Count() > 0);
        }

        /// <summary>
        /// A deck should have 52 cards
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_52_Cards()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.Cards.Count());
        }

        /// <summary>
        /// A deck should have 13 of each suite (Spades, Clubs, Hearts and Diamonds)
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_13_Cards_Of_Each_Suite()
        {
            var deck = new Deck();
            Assert.AreEqual(13, deck.Cards.Count(x => x.Suite == Card.Suites.Spades), "Not enough Spades");
            Assert.AreEqual(13, deck.Cards.Count(x => x.Suite == Card.Suites.Clubs), "Not enough Clubs");
            Assert.AreEqual(13, deck.Cards.Count(x => x.Suite == Card.Suites.Hearts), "Not enough Hearts");
            Assert.AreEqual(13, deck.Cards.Count(x => x.Suite == Card.Suites.Diamonds), "Not enough Diamonds");
        }

        /// <summary>
        /// A deck should have 26 of each color (Black and Red)
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Cards_Of_Each_Color()
        {
            var deck = new Deck();
            Assert.AreEqual(26, deck.Cards.Count(x => x.Color == Card.Colors.Black), "Not enough Black cards");
            Assert.AreEqual(26, deck.Cards.Count(x => x.Color == Card.Colors.Red), "Not enough Red cards");
        }

        /// <summary>
        /// A deck should have 26 black cards that are only spades and clubs
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Black_Cards_Only_SpadesAndClubs()
        {
            var deck = new Deck();
            List<Card.Suites> allowedSuites = new List<Card.Suites>() { Card.Suites.Clubs, Card.Suites.Spades };
            Assert.AreEqual(26, deck.Cards.Count(x => x.Color == Card.Colors.Black && allowedSuites.Contains(x.Suite)), "There must be 26 black spades and clubs combined");
        }

        /// <summary>
        /// A deck should have 26 red cards that are only hearts and diamonds
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Red_Cards_Only_HeartsAndDiamonds()
        {
            var deck = new Deck();
            List<Card.Suites> allowedSuites = new List<Card.Suites>() { Card.Suites.Hearts, Card.Suites.Diamonds };
            Assert.AreEqual(26, deck.Cards.Count(x => x.Color == Card.Colors.Red && allowedSuites.Contains(x.Suite)), "There must be 26 red hearts and diamonds combined");
        }

        /// <summary>
        /// A deck should have the correct values of cards for each suite.
        /// </summary>
        [TestMethod]
        public void Deck_Cards_Should_Have_Correct_Values_For_Suites()
        {
            var deck = new Deck();
            this.TestDeckCardsValues(deck, Card.Suites.Clubs);
            this.TestDeckCardsValues(deck, Card.Suites.Spades);
            this.TestDeckCardsValues(deck, Card.Suites.Hearts);
            this.TestDeckCardsValues(deck, Card.Suites.Diamonds);
        }

        /// <summary>
        /// When dealing a card make sure the first available card in order is dealt
        /// </summary>
        [TestMethod]
        public void DealCard_Should_Return_First_Available_Card()
        {
            var deck = new Deck();
            var thirdCard = deck.Cards[2];
            var fourthCard = deck.Cards[3];
            deck.Cards.ForEach(x => 
            {
                if (x != thirdCard && x != fourthCard)
                {
                    x.State = Card.States.Dealt; 
                }
            });

            var result = deck.DealCard();

            Assert.IsNotNull(result);
            Assert.AreEqual(thirdCard.Color, result.Color);
            Assert.AreEqual(thirdCard.Suite, result.Suite);
            Assert.AreEqual(thirdCard.Value, result.Value);
        }

        /// <summary>
        /// When dealing a card and there are no available cards return a NoAvailableCardException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NoAvailableCardsException))]
        public void DealCard_NoneAvailable_Should_Throw_NoAvailableCardException()
        {
            var deck = new Deck();
            deck.Cards.ForEach(x => x.State = Card.States.Dealt);
            var result = deck.DealCard();
        }

        /// <summary>
        /// Shuffle the deck 500 times and make sure none of the iterations are the same
        /// </summary>
        [TestMethod]
        public void ShuffleDeck_500_Iterations_Not_the_Same()
        {
            var deck = new Deck();
            var shuffleResults = new List<string>();
            for (int i = 0; i < 500; i++)
            {
                deck.ShuffleDeck();
                shuffleResults.Add(this.DeckCardsToString(deck));        
            }

            var duplicatesGroup = shuffleResults.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);

            Assert.AreEqual(0, duplicatesGroup.Count());
        }
        
        /// <summary>
        /// Loop through the allowed values of the cards and make sure they are there.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <param name="suite">The suite.</param>
        private void TestDeckCardsValues(Deck deck, Card.Suites suite)
        {
            for (int v = 1; v < 14; v++)
            {
                Assert.IsTrue(deck.Cards.Any(x => x.Suite == suite && x.Value == v), string.Format("Card value {0}, suite: {1} does not exist.", v, suite));  
            }
        }

        /// <summary>
        /// Decks the cards to string.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <returns>returns a string representation of the cards in the deck.</returns>
        private string DeckCardsToString(Deck deck)
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
