using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    /// <summary>
    /// Card Tests
    /// </summary>
    [TestClass]
    public class CardTests
    {
        /// <summary>
        /// if the card is a heart the name should end with H
        /// </summary>
        [TestMethod]
        public void IfCardIsAHeart_Name_ShouldEndWith_H()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Hearts, Value = 1 }, "H");
        }

        /// <summary>
        /// if the card is a diamond the name should end with D
        /// </summary>
        [TestMethod]
        public void IfCardIsADiamond_Name_ShouldEndWith_D()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Diamonds, Value = 1 }, "D");
        }

        /// <summary>
        /// if the card is a spade the name should end with S
        /// </summary>
        [TestMethod]
        public void IfCardIsASpade_Name_ShouldEndWith_S()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Spades, Value = 1 }, "S");
        }

        /// <summary>
        /// if the card is a club the name should end with C
        /// </summary>
        [TestMethod]
        public void IfCardIsAClub_Name_ShouldEndWith_C()
        {
            this.TestCardSuit(new Card() { Suite = Card.Suites.Clubs, Value = 1 }, "C");
        }

        /// <summary>
        /// if the card suite is not defined then an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IfCardSuiteIsNotDefined_Should_Throw_ArgumentException()
        {
            var card = new Card();
            var result = card.Name();
        }

        /// <summary>
        /// if the card value is 1 then the name should be prefixed with A
        /// </summary>
        [TestMethod]
        public void CardValue1_Name_ShouldBeginWith_A()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades,  Value = 1 }, "A");
        }

        /// <summary>
        /// if the card value is 11 then the name should be prefixed with J
        /// </summary>
        [TestMethod]
        public void CardValue11_Name_ShouldBeginWith_J()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 11 }, "J");
        }

        /// <summary>
        /// if the card value is 12 then the name should be prefixed with Q
        /// </summary>
        [TestMethod]
        public void CardValue12_Name_ShouldBeginWith_Q()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 12 }, "Q");
        }

        /// <summary>
        /// if the card value is 13 then the name should be prefixed with K
        /// </summary>
        [TestMethod]
        public void CardValue13_Name_ShouldBeginWith_K()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 13 }, "K");
        }

        /// <summary>
        /// if the card value is 2 then the name should be prefixed with 2
        /// </summary>
        [TestMethod]
        public void CardValue2_Name_ShouldBeginWith_2()
        {
            this.TestCardValue(new Card() { Suite = Card.Suites.Spades, Value = 2 }, "2");
        }

        /// <summary>
        /// If the card value is less then 1 throw exception when executing name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CardValueLessThen1_Name_Should_Throw_ArgumentException()
        {
            var card = new Card() { Suite = Card.Suites.Spades, Value = 0 };
            var result = card.Name();
        }

        /// <summary>
        /// If the card value is greater then 13 throw exception when executing name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CardValueGreaterThen13_Name_Should_Throw_ArgumentException()
        {
            var card = new Card() { Suite = Card.Suites.Spades, Value = 14 };
            var result = card.Name();
        }

        /// <summary>
        /// Tests the card suit.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="expectedSuffix">The expected suffix.</param>
        private void TestCardSuit(Card card, string expectedSuffix)
        {
            Assert.IsTrue(card.Name().EndsWith(expectedSuffix));
        }

        /// <summary>
        /// Tests the card value.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="expectedPrefix">The expected prefix.</param>
        private void TestCardValue(Card card, string expectedPrefix)
        {
            Assert.IsTrue(card.Name().StartsWith(expectedPrefix));
        }
    }
}
