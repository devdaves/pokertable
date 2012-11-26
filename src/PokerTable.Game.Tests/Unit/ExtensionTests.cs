using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Extensions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    /// <summary>
    /// Extension Tests
    /// </summary>
    [TestClass]
    public class ExtensionTests
    {
        /// <summary>
        /// ToSeatEntity should take a seat with a null player id and convert it to a SeatEntity with
        /// the correct values
        /// </summary>
        [TestMethod]
        public void ToSeatEntity_With_Null_PlayerID_Should_Return_SeatEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat() 
            {
                Id = 1,
                IsDealer = true,
                PlayerId = null
            };
            var result = seat.ToSeatEntity(tableId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatEntity));
            Assert.AreEqual(tableId.ToString(), result.PartitionKey);
            Assert.AreEqual(string.Format("Seat-{0}", seat.Id), result.RowKey);
            Assert.AreEqual(seat.Id, result.SeatId);
            Assert.AreEqual(seat.IsDealer, result.IsDealer);
            Assert.AreEqual(string.Empty, result.PlayerId);
        }

        /// <summary>
        /// ToSeatEntity should take a seat with a valid player id and convert it to a SeatEntity with
        /// the correct values
        /// </summary>
        [TestMethod]
        public void ToSeatEntity_With_ValidPlayerID_Should_Return_SeatEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat()
            {
                Id = 1,
                IsDealer = true,
                PlayerId = Guid.NewGuid()
            };
            var result = seat.ToSeatEntity(tableId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatEntity));
            Assert.AreEqual(tableId.ToString(), result.PartitionKey);
            Assert.AreEqual(string.Format("Seat-{0}", seat.Id), result.RowKey);
            Assert.AreEqual(seat.Id, result.SeatId);
            Assert.AreEqual(seat.IsDealer, result.IsDealer);
            Assert.AreEqual(seat.PlayerId.Value.ToString(), result.PlayerId);
        }

        /// <summary>
        /// ToPlayerEntity should take a player and convert it to a PlayerEntity with
        /// the correct values
        /// </summary>
        [TestMethod]
        public void ToPlayerEntity_Should_Return_PlayerEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            player.Cards.Add(new Card()
            {
                Color = Card.Colors.Black,
                State = Card.States.Available,
                Suite = Card.Suites.Clubs,
                Value = 2
            });
            var result = player.ToPlayerEntity(tableId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PlayerEntity));
            Assert.AreEqual(tableId.ToString(), result.PartitionKey);
            Assert.AreEqual(string.Format("Player-{0}", player.ID), result.RowKey);
            Assert.AreEqual(player.ID.ToString(), result.PlayerId);
            Assert.AreEqual(player.Name, result.Name);
            Assert.AreEqual((int)player.State, result.State);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Cards));
        }

        /// <summary>
        /// ToPokerTableEntity should take a table and convert it to a ToPokerTableEntity with
        /// the correct values
        /// </summary>
        [TestMethod]
        public void ToPokerTableEntity_Should_Return_PokerTableEntity_With_Valid_Values()
        {
            var table = new Table("Test", "TestPassword");
            var card = new Card()
            {
                Color = Card.Colors.Black,
                State = Card.States.Available,
                Suite = Card.Suites.Clubs,
                Value = 2
            };
            table.Deck.Cards.Add(card);
            table.Burn.Add(card);
            table.PublicCards.Add(card);

            var result = table.ToPokerTableEntity();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerTableEntity));
            Assert.AreEqual(table.Id.ToString(), result.PartitionKey);
            Assert.AreEqual("pokertable", result.RowKey);
            Assert.AreEqual(table.Name, result.Name);
            Assert.AreEqual(table.Password, result.Password);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Deck));
            Assert.IsTrue(!string.IsNullOrEmpty(result.BurnCards));
            Assert.IsTrue(!string.IsNullOrEmpty(result.PublicCards));
        }

        /// <summary>
        /// ToSeatModel with a null player id should return a seat with valid values
        /// </summary>
        [TestMethod]
        public void ToSeatModel_With_Null_PlayerId_Should_Return_Seat_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat()
            {
                Id = 1,
                IsDealer = true,
                PlayerId = null
            };
            var seatEntity = seat.ToSeatEntity(tableId);
            var seatModel = seatEntity.ToSeatModel();

            Assert.IsNotNull(seatModel);
            Assert.IsInstanceOfType(seatModel, typeof(Seat));
            Assert.AreEqual(seat.Id, seatModel.Id);
            Assert.AreEqual(seat.IsDealer, seatModel.IsDealer);
            Assert.AreEqual(seat.PlayerId, seatModel.PlayerId);
        }

        /// <summary>
        /// ToSeatModel with a player id should return a seat with valid values
        /// </summary>
        [TestMethod]
        public void ToSeatModel_With_PlayerId_Should_Return_Seat_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat()
            {
                Id = 1,
                IsDealer = true,
                PlayerId = Guid.NewGuid()
            };
            var seatEntity = seat.ToSeatEntity(tableId);
            var seatModel = seatEntity.ToSeatModel();

            Assert.IsNotNull(seatModel);
            Assert.IsInstanceOfType(seatModel, typeof(Seat));
            Assert.AreEqual(seat.Id, seatModel.Id);
            Assert.AreEqual(seat.IsDealer, seatModel.IsDealer);
            Assert.AreEqual(seat.PlayerId, seatModel.PlayerId);
        }
    }
}
