using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    /// <summary>
    /// Utility Tests
    /// </summary>
    [TestClass]
    public class UtilTests
    {
        /// <summary>
        /// Serialize a deck with cards and deserialize
        /// </summary>
        [TestMethod]
        public void Serialize_Deck_And_Deserialize()
        {
            var deck = new Deck();
            var card = new Card()
            {
                Color = Card.Colors.Black,
                Suite = Card.Suites.Clubs,
                State = Card.States.Available,
                Value = 2
            };
            deck.Cards = new List<Card>();
            deck.Cards.Add(card);

            var serializedDeck = Util.Serialize<Deck>(deck);
            var deserializedDeck = Util.DeSerialize<Deck>(serializedDeck);

            Assert.IsNotNull(deserializedDeck);
            Assert.IsNotNull(deserializedDeck.Cards);
            Assert.AreEqual(1, deserializedDeck.Cards.Count());
            Assert.AreEqual(card.Color, deserializedDeck.Cards[0].Color);
            Assert.AreEqual(card.Suite, deserializedDeck.Cards[0].Suite);
            Assert.AreEqual(card.State, deserializedDeck.Cards[0].State);
            Assert.AreEqual(card.Value, deserializedDeck.Cards[0].Value);
        }

        /// <summary>
        /// Serialize a list of cards and deserialize
        /// </summary>
        [TestMethod]
        public void Serialize_ListCards_And_Deserialize()
        {
            var card = new Card()
            {
                Color = Card.Colors.Black,
                Suite = Card.Suites.Clubs,
                State = Card.States.Available,
                Value = 2
            };
            var cards = new List<Card>();
            cards.Add(card);

            var serialized = Util.Serialize<List<Card>>(cards);
            var deserialized = Util.DeSerialize<List<Card>>(serialized);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.Count());
            Assert.AreEqual(card.Color, deserialized[0].Color);
            Assert.AreEqual(card.Suite, deserialized[0].Suite);
            Assert.AreEqual(card.State, deserialized[0].State);
            Assert.AreEqual(card.Value, deserialized[0].Value);
        }

        /// <summary>
        /// trying to deserialize a list of cards with a null string should return a empty list of cards
        /// </summary>
        [TestMethod]
        public void Deserilaize_ToListCards_Empty_String_Should_Return_EmptyListCards()
        {
            var deserialized = Util.DeSerialize<List<Card>>(null);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(0, deserialized.Count());
        }
    }
}
