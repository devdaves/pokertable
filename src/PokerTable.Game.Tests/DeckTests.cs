using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Tests
{
    /// <summary>
    /// Deck Tests
    /// </summary>
    [TestClass]
    public class DeckTests
    {
        /// <summary>
        /// engine field
        /// </summary>
        private Engine engine;

        /// <summary>
        /// repository mock
        /// </summary>
        private Mock<IRepository> repositoryMock;

        /// <summary>
        /// Run before every test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.engine = new Engine(this.repositoryMock.Object);
            this.engine.CreateNewTable(0, string.Empty, string.Empty);
        }

        /// <summary>
        /// A deck should have 52 cards
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_52_Cards()
        {
            Assert.AreEqual(52, this.engine.Table.Deck.Cards.Count());
        }

        /// <summary>
        /// A deck should have 13 of each suite (Spades, Clubs, Hearts and Diamonds)
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_13_Cards_Of_Each_Suite()
        {
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Spades), "Not enough Spades");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Clubs), "Not enough Clubs");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Hearts), "Not enough Hearts");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Diamonds), "Not enough Diamonds");
        }

        /// <summary>
        /// A deck should have 26 of each color (Black and Red)
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Cards_Of_Each_Color()
        {
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Black), "Not enough Black cards");
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Red), "Not enough Red cards");
        }

        /// <summary>
        /// A deck should have 26 black cards that are only spades and clubs
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Black_Cards_Only_SpadesAndClubs()
        {
            List<Card.Suites> allowedSuites = new List<Card.Suites>() { Card.Suites.Clubs, Card.Suites.Spades };
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Black && allowedSuites.Contains(x.Suite)), "There must be 26 black spades and clubs combined");
        }

        /// <summary>
        /// A deck should have 26 red cards that are only hearts and diamonds
        /// </summary>
        [TestMethod]
        public void Deck_Should_Have_26_Red_Cards_Only_HeartsAndDiamonds()
        {
            List<Card.Suites> allowedSuites = new List<Card.Suites>() { Card.Suites.Hearts, Card.Suites.Diamonds };
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Red && allowedSuites.Contains(x.Suite)), "There must be 26 red hearts and diamonds combined");
        }

        /// <summary>
        /// A deck should have the correct values of cards for each suite.
        /// </summary>
        [TestMethod]
        public void Deck_Cards_Should_Have_Correct_Values_For_Suites()
        {
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Clubs);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Spades);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Hearts);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Diamonds);
        }

        /// <summary>
        /// When dealing a card make sure the first available card in order is dealt
        /// </summary>
        [TestMethod]
        public void DealCard_Should_Return_First_Available_Card()
        {
            var thirdCard = this.engine.Table.Deck.Cards[2];
            var fourthCard = this.engine.Table.Deck.Cards[3];
            this.engine.Table.Deck.Cards.ForEach(x => 
            {
                if (x != thirdCard && x != fourthCard)
                {
                    x.State = Card.States.Dealt; 
                }
            });

            var result = this.engine.DealCard();

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
            this.engine.Table.Deck.Cards.ForEach(x => x.State = Card.States.Dealt);
            var result = this.engine.DealCard();
        }

        /// <summary>
        /// Shuffle the deck 500 times and make sure none of the iterations are the same
        /// </summary>
        [TestMethod]
        public void ShuffleDeck_500_Iterations_Not_the_Same()
        {
            var shuffleResults = new List<string>();
            for (int i = 0; i < 500; i++)
            {
                this.engine.ShuffleDeck();
                shuffleResults.Add(Utils.DeckCardsToString(this.engine.Table.Deck));        
            }

            var duplicatesGroup = shuffleResults.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);

            Assert.AreEqual(0, duplicatesGroup.Count());
        }
        
        /// <summary>
        /// Loop through the allowed values of the cards and make sure they are there.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <param name="suite">The suite.</param>
        private void TestDeckCardsValues(IDeck deck, Card.Suites suite)
        {
            for (int v = 1; v < 14; v++)
            {
                Assert.IsTrue(deck.Cards.Any(x => x.Suite == suite && x.Value == v), string.Format("Card value {0}, suite: {1} does not exist.", v, suite));  
            }
        }
    }
}
