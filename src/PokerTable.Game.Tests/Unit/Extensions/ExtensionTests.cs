using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.AzureEntities;
using PokerTable.Game.Extensions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit.Extensions
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void ToSeatEntity_With_Null_PlayerID_Should_Return_SeatEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat
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

        [TestMethod]
        public void ToSeatEntity_With_ValidPlayerID_Should_Return_SeatEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat
            {
                Id = 1,
                IsDealer = true,
                PlayerId = Guid.NewGuid()
            };
            var result = seat.ToSeatEntity(tableId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatEntity));
            Assert.IsNotNull(seat.PlayerId);
            Assert.AreEqual(tableId.ToString(), result.PartitionKey);
            Assert.AreEqual(string.Format("Seat-{0}", seat.Id), result.RowKey);
            Assert.AreEqual(seat.Id, result.SeatId);
            Assert.AreEqual(seat.IsDealer, result.IsDealer);
            Assert.AreEqual(seat.PlayerId.Value.ToString(), result.PlayerId);
        }

        [TestMethod]
        public void ToPlayerEntity_Should_Return_PlayerEntity_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            player.Cards.Add(new Card
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
            Assert.AreEqual(string.Format("Player-{0}", player.Id), result.RowKey);
            Assert.AreEqual(player.Id.ToString(), result.PlayerId);
            Assert.AreEqual(player.Name, result.Name);
            Assert.AreEqual((int)player.State, result.State);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Cards));
        }

        [TestMethod]
        public void ToPokerTableEntity_Should_Return_PokerTableEntity_With_Valid_Values()
        {
            var table = new Table("Test", "TestPassword");
            var card = new Card
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
            Assert.AreEqual(PokerTableEntity.Prefix, result.RowKey);
            Assert.AreEqual(table.Name, result.Name);
            Assert.AreEqual(table.Password, result.Password);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Deck));
            Assert.IsTrue(!string.IsNullOrEmpty(result.BurnCards));
            Assert.IsTrue(!string.IsNullOrEmpty(result.PublicCards));
        }

        [TestMethod]
        public void ToSeatModel_With_Null_PlayerId_Should_Return_Seat_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat
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

        [TestMethod]
        public void ToSeatModel_With_PlayerId_Should_Return_Seat_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var seat = new Seat
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

        [TestMethod]
        public void ToSeatModelList_Should_Return_List_Of_Seats()
        {
            var tableId = Guid.NewGuid();
            var seatEntities = new List<SeatEntity>
            {
                new SeatEntity(tableId, 1) { SeatId = 1 },
                new SeatEntity(tableId, 2) { SeatId = 2 },
            };

            var results = seatEntities.ToSeatModelList();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(1, results[0].Id);
            Assert.AreEqual(2, results[1].Id);
        }

        [TestMethod]
        public void ToPlayerModel_Should_Return_PlayerModel_With_Valid_Values()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");
            player.Cards.Add(new Card
            {
                Color = Card.Colors.Black,
                State = Card.States.Available,
                Suite = Card.Suites.Clubs,
                Value = 2
            });
            var playerEntity = player.ToPlayerEntity(tableId);
            var playerModel = playerEntity.ToPlayerModel();

            Assert.IsNotNull(playerModel);
            Assert.IsInstanceOfType(playerModel, typeof(Player));
            Assert.AreEqual(player.Id, playerModel.Id);
            Assert.AreEqual(player.Name, playerModel.Name);
            Assert.AreEqual(player.Cards.Count(), playerModel.Cards.Count());
            Assert.AreEqual(player.Cards[0].Color, playerModel.Cards[0].Color);
            Assert.AreEqual(player.Cards[0].State, playerModel.Cards[0].State);
            Assert.AreEqual(player.Cards[0].Suite, playerModel.Cards[0].Suite);
            Assert.AreEqual(player.Cards[0].Value, playerModel.Cards[0].Value);
        }

        [TestMethod]
        public void ToPlayerModel_EmptyCards_Should_Return_PlayerModel_With_Empty_Cards()
        {
            var tableId = Guid.NewGuid();
            var player = new Player("Test");

            var playerEntity = player.ToPlayerEntity(tableId);
            var playerModel = playerEntity.ToPlayerModel();

            Assert.AreEqual(0, playerModel.Cards.Count());
        }

        [TestMethod]
        public void ToPlayerModelList_Should_Return_List_Of_Players()
        {
            var tableId = Guid.NewGuid();
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();
            var playerEntities = new List<PlayerEntity>
            {
                new PlayerEntity(tableId, player1Id) { PlayerId = player1Id.ToString(), Name = "player1" },
                new PlayerEntity(tableId, player2Id) { PlayerId = player2Id.ToString(), Name = "player2" }
            };

            var results = playerEntities.ToPlayerModelList();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("player1", results[0].Name);
            Assert.AreEqual("player2", results[1].Name);
        }

        [TestMethod]
        public void ToTableModel_Should_Return_Table_With_Valid_Values()
        {
            var table = new Table("Test", "TestPassword");
            var card = new Card
            {
                Color = Card.Colors.Black,
                State = Card.States.Available,
                Suite = Card.Suites.Clubs,
                Value = 2
            };
            table.Deck.Cards.Add(card);
            table.Burn.Add(card);
            table.PublicCards.Add(card);

            var tableEntity = table.ToPokerTableEntity();
            var tableModel = tableEntity.ToTableModel();

            Assert.IsNotNull(tableModel);
            Assert.IsInstanceOfType(tableModel, typeof(Table));
            Assert.AreEqual(table.Id, tableModel.Id);
            Assert.AreEqual(table.Name, tableModel.Name);
            Assert.AreEqual(table.Password, tableModel.Password);

            Assert.AreEqual(table.Deck.Cards[0].Color, tableModel.Deck.Cards[0].Color);
            Assert.AreEqual(table.Deck.Cards[0].State, tableModel.Deck.Cards[0].State);
            Assert.AreEqual(table.Deck.Cards[0].Suite, tableModel.Deck.Cards[0].Suite);
            Assert.AreEqual(table.Deck.Cards[0].Value, tableModel.Deck.Cards[0].Value);

            Assert.AreEqual(table.Burn[0].Color, tableModel.Burn[0].Color);
            Assert.AreEqual(table.Burn[0].State, tableModel.Burn[0].State);
            Assert.AreEqual(table.Burn[0].Suite, tableModel.Burn[0].Suite);
            Assert.AreEqual(table.Burn[0].Value, tableModel.Burn[0].Value);

            Assert.AreEqual(table.PublicCards[0].Color, tableModel.PublicCards[0].Color);
            Assert.AreEqual(table.PublicCards[0].State, tableModel.PublicCards[0].State);
            Assert.AreEqual(table.PublicCards[0].Suite, tableModel.PublicCards[0].Suite);
            Assert.AreEqual(table.PublicCards[0].Value, tableModel.PublicCards[0].Value);
        }
    }
}
