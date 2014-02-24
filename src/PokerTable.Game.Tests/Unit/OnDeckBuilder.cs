using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class OnDeckBuilder
    {
        [TestClass]
        public class WhenBuild
        {
            private IDeckBuilder deckBuilder;

            [TestInitialize]
            public void Setup()
            {
                deckBuilder = new DeckBuilder();
            }

            [TestMethod]
            public void ShouldReturn52Cards()
            {
                int expectedCardCount = 52;
                var result = this.deckBuilder.Build();
                Assert.AreEqual(expectedCardCount, result.Cards.Count);
            }

            [TestMethod]
            public void ShouldHave13CardsOfEachSuite()
            {
                var result = this.deckBuilder.Build();

                Assert.AreEqual(13, result.Cards.Count(x => x.Suite == Card.Suites.Spades), "Not enough Spades");
                Assert.AreEqual(13, result.Cards.Count(x => x.Suite == Card.Suites.Clubs), "Not enough Clubs");
                Assert.AreEqual(13, result.Cards.Count(x => x.Suite == Card.Suites.Hearts), "Not enough Hearts");
                Assert.AreEqual(13, result.Cards.Count(x => x.Suite == Card.Suites.Diamonds), "Not enough Diamonds");
            }

            [TestMethod]
            public void ShouldHave26CardsOfEachColor()
            {
                var result = this.deckBuilder.Build();

                Assert.AreEqual(26, result.Cards.Count(x => x.Color == Card.Colors.Black), "Not enough Black cards");
                Assert.AreEqual(26, result.Cards.Count(x => x.Color == Card.Colors.Red), "Not enough Red cards");
            }

            [TestMethod]
            public void ShouldHave26BlackCardsOnlySpadesAndClubs()
            {
                var allowedSuites = new List<Card.Suites> { Card.Suites.Clubs, Card.Suites.Spades };
                var result = this.deckBuilder.Build();

                Assert.AreEqual(26, result.Cards.Count(x => x.Color == Card.Colors.Black && allowedSuites.Contains(x.Suite)), "There must be 26 black spades and clubs combined");
            }

            [TestMethod]
            public void ShouldHave26RedCardsOnlyHeartsAndDiamonds()
            {
                var allowedSuites = new List<Card.Suites> { Card.Suites.Hearts, Card.Suites.Diamonds };
                var result = this.deckBuilder.Build();

                Assert.AreEqual(26, result.Cards.Count(x => x.Color == Card.Colors.Red && allowedSuites.Contains(x.Suite)), "There must be 26 red hearts and diamonds combined");
            }

            [TestMethod]
            public void Deck_Cards_Should_Have_Correct_Values_For_Suites()
            {
                var result = this.deckBuilder.Build();

                this.TestDeckCardsValues(result, Card.Suites.Clubs);
                this.TestDeckCardsValues(result, Card.Suites.Spades);
                this.TestDeckCardsValues(result, Card.Suites.Hearts);
                this.TestDeckCardsValues(result, Card.Suites.Diamonds);
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
}
