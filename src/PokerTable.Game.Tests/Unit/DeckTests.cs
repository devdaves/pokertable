using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class DeckTests
    {
        private Engine engine;

        private Mock<IRepository> repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.repositoryMock.Setup(x => x.TablePasswordExists(It.IsAny<string>())).Returns(false);
            this.engine = new Engine(this.repositoryMock.Object);
            this.engine.CreateNewTable(0, string.Empty);
        }

        [TestMethod]
        public void Deck_Should_Have_52_Cards()
        {
            Assert.AreEqual(52, this.engine.Table.Deck.Cards.Count());
        }

        [TestMethod]
        public void Deck_Should_Have_13_Cards_Of_Each_Suite()
        {
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Spades), "Not enough Spades");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Clubs), "Not enough Clubs");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Hearts), "Not enough Hearts");
            Assert.AreEqual(13, this.engine.Table.Deck.Cards.Count(x => x.Suite == Card.Suites.Diamonds), "Not enough Diamonds");
        }

        [TestMethod]
        public void Deck_Should_Have_26_Cards_Of_Each_Color()
        {
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Black), "Not enough Black cards");
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Red), "Not enough Red cards");
        }

        [TestMethod]
        public void Deck_Should_Have_26_Black_Cards_Only_SpadesAndClubs()
        {
            var allowedSuites = new List<Card.Suites> { Card.Suites.Clubs, Card.Suites.Spades };
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Black && allowedSuites.Contains(x.Suite)), "There must be 26 black spades and clubs combined");
        }

        [TestMethod]
        public void Deck_Should_Have_26_Red_Cards_Only_HeartsAndDiamonds()
        {
            var allowedSuites = new List<Card.Suites> { Card.Suites.Hearts, Card.Suites.Diamonds };
            Assert.AreEqual(26, this.engine.Table.Deck.Cards.Count(x => x.Color == Card.Colors.Red && allowedSuites.Contains(x.Suite)), "There must be 26 red hearts and diamonds combined");
        }

        [TestMethod]
        public void Deck_Cards_Should_Have_Correct_Values_For_Suites()
        {
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Clubs);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Spades);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Hearts);
            this.TestDeckCardsValues(this.engine.Table.Deck, Card.Suites.Diamonds);
        }

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

        [TestMethod]
        [ExpectedException(typeof(NoAvailableCardsException))]
        public void DealCard_NoneAvailable_Should_Throw_NoAvailableCardException()
        {
            this.engine.Table.Deck.Cards.ForEach(x => x.State = Card.States.Dealt);
            this.engine.DealCard();
        }

        [TestMethod]
        public void ShuffleDeck_500_Iterations_Not_the_Same()
        {
            var shuffleResults = new List<string>();
            for (var i = 0; i < 500; i++)
            {
                this.engine.ShuffleDeck();
                shuffleResults.Add(Utils.DeckCardsToString(this.engine.Table.Deck));
            }

            var duplicatesGroup = shuffleResults.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);

            Assert.AreEqual(0, duplicatesGroup.Count());
        }

        private void TestDeckCardsValues(Deck deck, Card.Suites suite)
        {
            for (var v = 1; v < 14; v++)
            {
                Assert.IsTrue(deck.Cards.Any(x => x.Suite == suite && x.Value == v), string.Format("Card value {0}, suite: {1} does not exist.", v, suite));
            }
        }
    }
}
