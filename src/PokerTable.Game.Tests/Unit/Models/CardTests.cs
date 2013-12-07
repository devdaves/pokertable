using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit.Models
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void IfCardIsAHeart_Name_ShouldEndWith_H()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Hearts, Value = 1 }, "H");
        }

        [TestMethod]
        public void IfCardIsADiamond_Name_ShouldEndWith_D()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Diamonds, Value = 1 }, "D");
        }

        [TestMethod]
        public void IfCardIsASpade_Name_ShouldEndWith_S()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Spades, Value = 1 }, "S");
        }

        [TestMethod]
        public void IfCardIsAClub_Name_ShouldEndWith_C()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Clubs, Value = 1 }, "C");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfCardSuiteIsNotDefined_Should_Throw_ArgumentException()
        {
            var card = new Card();
            var result = card.Name();
        }

        [TestMethod]
        public void CardValue1_Name_ShouldBeginWith_A()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 1 }, "A");
        }

        [TestMethod]
        public void CardValue11_Name_ShouldBeginWith_J()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 11 }, "J");
        }

        [TestMethod]
        public void CardValue12_Name_ShouldBeginWith_Q()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 12 }, "Q");
        }

        [TestMethod]
        public void CardValue13_Name_ShouldBeginWith_K()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 13 }, "K");
        }

        [TestMethod]
        public void CardValue2_Name_ShouldBeginWith_2()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 2 }, "2");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CardValueLessThen1_Name_Should_Throw_ArgumentException()
        {
            var card = new Card() { Suite = Card.Suites.Spades, Value = 0 };
            var result = card.Name();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CardValueGreaterThen13_Name_Should_Throw_ArgumentException()
        {
            var card = new Card() { Suite = Card.Suites.Spades, Value = 14 };
            var result = card.Name();
        }

        private void TestCardSuit(Card card, string expectedSuffix)
        {
            Assert.IsTrue(card.Name().EndsWith(expectedSuffix));
        }

        private void TestCardValue(Card card, string expectedPrefix)
        {
            Assert.IsTrue(card.Name().StartsWith(expectedPrefix));
        }
    }
}
