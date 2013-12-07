using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        public void Serialize_Deck_And_Deserialize()
        {
            var deck = new Deck();
            var card = new Card
            {
                Color = Card.Colors.Black,
                Suite = Card.Suites.Clubs,
                State = Card.States.Available,
                Value = 2
            };
            deck.Cards = new List<Card> {card};

            var serializedDeck = Util.Serialize(deck);
            var deserializedDeck = Util.DeSerialize<Deck>(serializedDeck);

            Assert.IsNotNull(deserializedDeck);
            Assert.IsNotNull(deserializedDeck.Cards);
            Assert.AreEqual(1, deserializedDeck.Cards.Count());
            Assert.AreEqual(card.Color, deserializedDeck.Cards[0].Color);
            Assert.AreEqual(card.Suite, deserializedDeck.Cards[0].Suite);
            Assert.AreEqual(card.State, deserializedDeck.Cards[0].State);
            Assert.AreEqual(card.Value, deserializedDeck.Cards[0].Value);
        }

        [TestMethod]
        public void Serialize_ListCards_And_Deserialize()
        {
            var card = new Card
            {
                Color = Card.Colors.Black,
                Suite = Card.Suites.Clubs,
                State = Card.States.Available,
                Value = 2
            };
            var cards = new List<Card> {card};

            var serialized = Util.Serialize(cards);
            var deserialized = Util.DeSerialize<List<Card>>(serialized);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.Count());
            Assert.AreEqual(card.Color, deserialized[0].Color);
            Assert.AreEqual(card.Suite, deserialized[0].Suite);
            Assert.AreEqual(card.State, deserialized[0].State);
            Assert.AreEqual(card.Value, deserialized[0].Value);
        }

        [TestMethod]
        public void Deserilaize_ToListCards_Empty_String_Should_Return_EmptyListCards()
        {
            var deserialized = Util.DeSerialize<List<Card>>(null);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(0, deserialized.Count());
        }
    }
}
