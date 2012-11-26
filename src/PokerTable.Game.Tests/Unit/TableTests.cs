using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    /// <summary>
    /// Table Tests
    /// </summary>
    [TestClass]
    public class TableTests
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
            this.engine.CreateNewTable(10, "TestName", "TestPassword");
        }

        /// <summary>
        /// When creating a new table make sure the table name passed to the constructor gets persisted to a property
        /// </summary>
        [TestMethod]
        public void CreatNewTable_Stores_TableName()
        {
            var expectedTableName = "TestName";
            Assert.AreEqual(expectedTableName, this.engine.Table.Name);
        }

        /// <summary>
        /// When creating a new table make sure the table password passed to the constructor gets persisted to a property
        /// </summary>
        [TestMethod]
        public void CreateNewTable_Stores_TablePassword()
        {
            var expectedTablePassword = "TestPassword";
            Assert.AreEqual(expectedTablePassword, this.engine.Table.Password);
        }        

        /// <summary>
        /// When creating a table, make sure the number of seats defined in the constructor get created.
        /// </summary>
        [TestMethod]
        public void CreateNewTable_5Seats_Confirm_Creation()
        {
            this.engine.CreateNewTable(5, string.Empty, string.Empty);
            Assert.AreEqual(5, this.engine.Table.Seats.Count());
        }

        /// <summary>
        /// When creating a table, passing in 0 for number of seats, seats property should not be null;
        /// </summary>
        [TestMethod]
        public void CreateNewTable_0Seats_Seats_Should_Not_Be_Null()
        {
           this.engine.CreateNewTable(0, string.Empty, string.Empty);
           Assert.IsNotNull(this.engine.Table.Seats);
        }

        /// <summary>
        /// When creating a table, passing in -5 for the number of seats, seats property should be at a count of 0
        /// </summary>
        [TestMethod]
        public void CreateNewTable_Negative5Seats_Seats_Count_Should_Be_0()
        {
            this.engine.CreateNewTable(-5, string.Empty, string.Empty);
            Assert.AreEqual(0, this.engine.Table.Seats.Count());
        }

        /// <summary>
        /// When adding a seat and the collection is empty, the first seat should have a seat id of 1
        /// </summary>
        [TestMethod]
        public void AddSeat_WhenSeatsEmpty_SeatId_Should_Be_1()
        {
            this.engine.CreateNewTable(0, string.Empty, string.Empty);
            this.engine.AddSeat();
            Assert.AreEqual(1, this.engine.Table.Seats[0].Id);
        }

        /// <summary>
        /// When adding a seat and the collection has 1, the second seat should have a seat id of 2
        /// </summary>
        [TestMethod]
        public void AddSeat_WhenSeatsHas1_SeatId_Should_Be_2()
        {
            this.engine.CreateNewTable(1, string.Empty, string.Empty);
            this.engine.AddSeat();
            Assert.AreEqual(2, this.engine.Table.Seats[1].Id);
        }

        /// <summary>
        /// removing a seat when the seat collection is empty should return false
        /// </summary>
        [TestMethod]
        public void RemoveSeat_WhenSeatsEmpty_Should_Return_False()
        {
            this.engine.CreateNewTable(0, string.Empty, string.Empty);
            var result = this.engine.RemoveSeat(0);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// removing a seat with an id that does not exist should return false
        /// </summary>
        [TestMethod]
        public void RemoveSeat_WhenSeatIdDoesNotExist_Should_Return_False()
        {
            this.engine.CreateNewTable(1, string.Empty, string.Empty);
            var result = this.engine.RemoveSeat(4);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// removing a seat with an id that exists, should return true
        /// </summary>
        [TestMethod]
        public void RemoveSeat_WhenSeatExists_Should_Return_True()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            var result = this.engine.RemoveSeat(3);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// removing a seat with an id that exists, should remove the seat
        /// </summary>
        [TestMethod]
        public void RemoveSeat_WhenSeatExists_Should_Remove_Seat()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            var result = this.engine.RemoveSeat(3);
            Assert.IsFalse(this.engine.Table.Seats.Any(x => x.Id == 3));                
        }

        /// <summary>
        /// removing a seat with an id of 2 when there are 3 seats, should reorder the seats
        /// </summary>
        [TestMethod]
        public void RemoveSeat_WhenSeatExists_Remove2ndOf3Seats_Should_ReorderSeats()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.RemoveSeat(2);
            Assert.AreEqual(1, this.engine.Table.Seats[0].Id);
            Assert.AreEqual(2, this.engine.Table.Seats[1].Id);
        }

        /// <summary>
        /// DealerExists should be false after creating a new table
        /// </summary>
        [TestMethod]
        public void DealerExists_Should_Be_False_When_CreateingNewTable()
        {
            var result = this.engine.DealerExists();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// DealerExists should be true if one of the seats is marked as the dealer
        /// </summary>
        [TestMethod]
        public void DealerExists_Should_Be_True_When_A_Seat_Is_Marked_As_Dealer()
        {
            this.engine.SetDealer(2);
            var result = this.engine.DealerExists();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Setting the dealer when no seats exists should do nothing
        /// </summary>
        [TestMethod]
        public void SetDealer_NoSeatsDefined_Should_Do_Nothing()
        {
            this.engine.CreateNewTable(0, string.Empty, string.Empty);
            this.engine.SetDealer(1);
        }

        /// <summary>
        /// setting the dealer to an invalid seat id, DealerExists should return false
        /// </summary>
        [TestMethod]
        public void SetDealer_InvalidSeatId_DealerExists_Returns_False()
        {
            this.engine.CreateNewTable(5, string.Empty, string.Empty);
            this.engine.SetDealer(1);
            this.engine.SetDealer(6);
            var result = this.engine.DealerExists();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// There can only be one dealer at any given time.
        /// </summary>
        [TestMethod]
        public void SetDealer_ShouldNoAllowMultipleDealers()
        {
            this.engine.SetDealer(1);
            this.engine.SetDealer(2);
            this.engine.SetDealer(3);
            Assert.AreEqual(1, this.engine.Table.Seats.Count(x => x.IsDealer == true));
        }

        /// <summary>
        /// Calling next dealer when no seats exists should do nothing
        /// </summary>
        [TestMethod]
        public void NextDealer_NoSeatsDefined_Should_Do_Nothing()
        {
            this.engine.CreateNewTable(0, string.Empty, string.Empty);
            this.engine.NextDealer();
        }

        /// <summary>
        /// Calling next dealer when no dealer is defined and no players are in seats should do nothing
        /// </summary>
        [TestMethod]
        public void NextDealer_NoDealerDefined_NoPlayersInSeats_Should_Do_Nothing()
        {
            this.engine.NextDealer();
        }

        /// <summary>
        /// Next dealer when no dealer is defined and all seats have players the first seat should be the dealer
        /// </summary>
        [TestMethod]
        public void NextDealer_NoDelearsDefined_AllSeatsWithPlayers_FirstSeat_Should_Be_Dealer()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);

            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].ID);

            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[0].IsDealer);
        }

        /// <summary>
        /// when all seats have players, seat 1 is dealer, NextDealer should be seat 2
        /// </summary>
        [TestMethod]
        public void NextDealer_AllSeatsWtihPlayers_SeatOneIsDealer_NextDealer_Should_Be_SeatTwo()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[1].IsDealer);
        }

        /// <summary>
        /// seat1 has a player, seat2 does not have a player, seat3 has a player.  seat1 is the dealer
        /// NextDealer should skip seat 2 and seat 3 should be dealer
        /// </summary>
        [TestMethod]
        public void NextDealer_Seat1Player_Seat2NoPlayer_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[1].ID);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[2].IsDealer);
        }

        /// <summary>
        /// seat1 has a player, seat2 has player sitting out, seat3 has a player.  seat1 is the dealer
        /// NextDealer should skip seat 2 and seat 3 should be dealer
        /// </summary>
        [TestMethod]
        public void NextDealer_Seat1Player_Seat2PlayerSittingOut_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.Table.Players[1].State = Player.States.SittingOut;

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[2].IsDealer);
        }

        /// <summary>
        /// Deal Flop should put 1 card in the burn list, 3 cards in the public cards list
        /// </summary>
        [TestMethod]
        public void DealFlop_Should_Burn1_Deal3_To_PublicCards()
        {
            this.engine.DealFlop();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(3, this.engine.Table.PublicCards.Count());
        }

        /// <summary>
        /// Deal Flop should take top card and put in burn list
        /// </summary>
        [TestMethod]
        public void DealFlop_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealFlop();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

        /// <summary>
        /// Deal Flop should take 2,3 and 4 cards and put in public cards list
        /// </summary>
        [TestMethod]
        public void DealFlop_Should_Deal_2nd_3rd_4th_Cards()
        {
            var card2 = this.engine.Table.Deck.Cards[1];
            var card3 = this.engine.Table.Deck.Cards[2];
            var card4 = this.engine.Table.Deck.Cards[3];
            this.engine.DealFlop();

            Assert.AreEqual(card2.Name(), this.engine.Table.PublicCards[0].Name());
            Assert.AreEqual(card3.Name(), this.engine.Table.PublicCards[1].Name());
            Assert.AreEqual(card4.Name(), this.engine.Table.PublicCards[2].Name());
        }

        /// <summary>
        /// Deal Turn should put 1 card in the burn list, 1 cards in the public cards list
        /// </summary>
        [TestMethod]
        public void DealTurn_Should_Burn1_Deal1_To_PublicCards()
        {
            this.engine.DealTurn();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(1, this.engine.Table.PublicCards.Count());
        }

        /// <summary>
        /// Deal turn should take top card and put in burn list
        /// </summary>
        [TestMethod]
        public void DealTurn_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealTurn();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

        /// <summary>
        /// Deal Turn should take 2 card and put in public cards list
        /// </summary>
        [TestMethod]
        public void DealTurn_Should_Deal_2nd_Card()
        {
            var card2 = this.engine.Table.Deck.Cards[1];
            this.engine.DealTurn();

            Assert.AreEqual(card2.Name(), this.engine.Table.PublicCards[0].Name());
        }

        /// <summary>
        /// Deal River should put 1 card in the burn list, 1 cards in the public cards list
        /// </summary>
        [TestMethod]
        public void DealRiver_Should_Burn1_Deal1_To_PublicCards()
        {
            this.engine.DealRiver();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(1, this.engine.Table.PublicCards.Count());
        }

        /// <summary>
        /// Deal River should take top card and put in burn list
        /// </summary>
        [TestMethod]
        public void DealRiver_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealRiver();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

        /// <summary>
        /// Deal River should take 2 card and put in public cards list
        /// </summary>
        [TestMethod]
        public void DealRiver_Should_Deal_2nd_Card()
        {
            var card2 = this.engine.Table.Deck.Cards[1];
            this.engine.DealRiver();

            Assert.AreEqual(card2.Name(), this.engine.Table.PublicCards[0].Name());
        }

        /// <summary>
        /// Deal Players should give each available player 2 cards
        /// </summary>
        [TestMethod]
        public void DealPlayers_EachAvailablePlayer_Should_Get_2_Cards()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.NextDealer();
            this.engine.DealPlayers();

            Assert.AreEqual(2, this.engine.Table.Players[0].Cards.Count());
            Assert.AreEqual(2, this.engine.Table.Players[1].Cards.Count());
            Assert.AreEqual(2, this.engine.Table.Players[2].Cards.Count());
        }

        /// <summary>
        /// Deal players, players set to status other then available should not get cards
        /// </summary>
        [TestMethod]
        public void DealPlayer_NotAvailablePlayers_Should_Not_Get_Cards()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].ID);

            this.engine.Table.Players[1].State = Player.States.SittingOut;

            this.engine.NextDealer();
            this.engine.DealPlayers();

            Assert.AreEqual(0, this.engine.Table.Players[1].Cards.Count());
        }

        /// <summary>
        /// Trying to deal the players cards and no dealer is set should not deal cards
        /// </summary>
        [TestMethod]
        public void DealPlayers_NoDealer_ShouldNotDealCards()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].ID);

            this.engine.DealPlayers();

            Assert.AreEqual(0, this.engine.Table.Players[0].Cards.Count());
            Assert.AreEqual(0, this.engine.Table.Players[1].Cards.Count());
            Assert.AreEqual(0, this.engine.Table.Players[2].Cards.Count());
        }

        /// <summary>
        /// Deal Players with 3 seats filled with available players, seat 1 is dealer, make
        /// sure cards are dealt correctly.
        /// </summary>
        [TestMethod]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat1IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.SetDealer(1);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// first card in deck should be first card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// second card in deck should be first card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// third card in deck should be first card dealt to 1st player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// fourth card in deck should be second card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// fifth card in deck should be second card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// sixth card in deck should be second card dealt to 1st player
        }

        /// <summary>
        /// Deal Players with 3 seats filled with available players, seat 2 is dealer, make
        /// sure cards are dealt correctly.
        /// </summary>
        [TestMethod]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat2IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.SetDealer(2);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// first card in deck should be first card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// second card in deck should be first card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// third card in deck should be first card dealt to 2nd player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// fourth card in deck should be second card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// fifth card in deck should be second card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// sixth card in deck should be second card dealt to 2nd player
        }

        /// <summary>
        /// Deal Players with 3 seats filled with available players, seat 3 is dealer, make
        /// sure cards are dealt correctly.
        /// </summary>
        [TestMethod]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat3IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].ID);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].ID);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].ID);

            this.engine.SetDealer(3);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// first card in deck should be first card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// second card in deck should be first card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// third card in deck should be first card dealt to 3rd player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// fourth card in deck should be second card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// fifth card in deck should be second card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// sixth card in deck should be second card dealt to 3rd player
        }

        /// <summary>
        /// AddPlayer player not already on table, then player should be added
        /// </summary>
        [TestMethod]
        public void AddPlayer_PlayerDoesNotExist_Should_Add_Player()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            Assert.AreEqual(playerToAdd.Name, this.engine.Table.Players[0].Name);
        }

        /// <summary>
        /// AddPlayer player not already on table, should return true
        /// </summary>
        [TestMethod]
        public void AddPlayer_PlayerDoesNotExist_Should_Return_True()
        {
            var playerToAdd = new Player("test");
            var result = this.engine.AddPlayer(playerToAdd);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// AddPlayer player already on table, should not add player again.
        /// </summary>
        [TestMethod]
        public void AddPlayer_PlayerExists_Should_Not_Add_Player_Again()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            this.engine.AddPlayer(playerToAdd);

            Assert.AreEqual(1, this.engine.Table.Players.Count());
        }

        /// <summary>
        /// AddPlayer player already on table, should return false
        /// </summary>
        [TestMethod]
        public void AddPlayer_PlayerExists_Should_Return_False()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            var result = this.engine.AddPlayer(playerToAdd);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// RemovePlayer, if player is in seat, make sure they are removed.
        /// </summary>
        [TestMethod]
        public void RemovePlayer_PlayerInSeat_PlayerRemoved_From_Seat()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            this.engine.AssignSeatToPlayer(1, playerToAdd.ID);

            this.engine.RemovePlayer(playerToAdd.ID);

            Assert.AreEqual(null, this.engine.Table.Seats[0].PlayerId);
        }

        /// <summary>
        /// RemovePlayer player exists, player is removed
        /// </summary>
        [TestMethod]
        public void RemovePlayer_PlayerExists_Player_Is_Removed()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            this.engine.RemovePlayer(playerToAdd.ID);

            Assert.AreEqual(0, this.engine.Table.Players.Count());
        }

        /// <summary>
        /// RemovePlayer player exists, should return true
        /// </summary>
        [TestMethod]
        public void RemovePlayer_PlayerExists_Should_Return_True()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            var result = this.engine.RemovePlayer(playerToAdd.ID);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// RemovePlayer player does not exist should return false
        /// </summary>
        [TestMethod]
        public void RemovePlayer_PlayerDoesNotExist_Should_Return_False()
        {
            var result = this.engine.RemovePlayer(Guid.NewGuid());
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Reset Table should set a new deck
        /// </summary>
        [TestMethod]
        public void ResetTable_Should_Set_New_Deck()
        {
            var oldDeck = Utils.DeckCardsToString(this.engine.Table.Deck);
            this.engine.ResetTable();
            var newDeck = Utils.DeckCardsToString(this.engine.Table.Deck);

            Assert.AreNotEqual(oldDeck, newDeck);
        }

        /// <summary>
        /// Reset Table should clear the burn list
        /// </summary>
        [TestMethod]
        public void ResetTable_Should_Clear_Burn_List()
        {
            this.engine.DealFlop();
            this.engine.ResetTable();

            Assert.AreEqual(0, this.engine.Table.Burn.Count());
        }

        /// <summary>
        /// Reset Table should clear the public cards list
        /// </summary>
        [TestMethod]
        public void ResetTable_Should_Clear_PublicCards_List()
        {
            this.engine.DealFlop();
            this.engine.ResetTable();

            Assert.AreEqual(0, this.engine.Table.PublicCards.Count());
        }

        /// <summary>
        /// A player can not be in two seats, if player is in another seat on the table
        /// make sure they are moved.
        /// </summary>
        [TestMethod]
        public void AssignSeatToPlayer__PlayerAlreadyInSeat_Should_Be_Moved_To_New_Seat()
        {
            var player = new Player("test");

            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.ID);
            this.engine.AssignSeatToPlayer(2, player.ID);

            Assert.IsNull(this.engine.Table.Seats[0].PlayerId);
            Assert.AreEqual(player.ID, this.engine.Table.Seats[1].PlayerId);
        }

        /// <summary>
        /// Assigning a seat to a player and that seat is occupied return false
        /// </summary>
        [TestMethod]
        public void AssignSeatToPlayer_SeatOccupied_Should_Return_False()
        {
            var player1 = new Player("player1");
            var player2 = new Player("player2");

            this.engine.AddPlayer(player1);
            this.engine.AddPlayer(player2);
            this.engine.AssignSeatToPlayer(1, player1.ID);

            var result = this.engine.AssignSeatToPlayer(1, player2.ID);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Assigning a seat to a player and the seat is not occupied return true
        /// </summary>
        [TestMethod]
        public void AssignSeatToPlayer_SeatNotOccupied_Should_Return_True()
        {
            var player = new Player("test");

            this.engine.AddPlayer(player);

            var result = this.engine.AssignSeatToPlayer(1, player.ID);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Assigning a seat to a player and the seat is not occupied, make sure
        /// the seat is assigned to the player
        /// </summary>
        [TestMethod]
        public void AssignSeatToPlayer_SeatNotOccupied_Player_Should_Be_Assigned_To_Seat()
        {
            var player = new Player("test");

            this.engine.AddPlayer(player);

            this.engine.AssignSeatToPlayer(1, player.ID);

            Assert.AreEqual(player.ID, this.engine.Table.Seats[0].PlayerId);
        }

        /// <summary>
        /// Removing a player from the seat by SeatId and the seat is occupied return true
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_SeatId_SeatOccupied_Should_Return_True()
        {
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.ID);

            var result = this.engine.RemovePlayerFromSeat(1);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Removing a player from the seat by SeatId and the seat is occupied, make sure
        /// the playerID is set to null
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_SeatId_SeatOccupied_PlayerID_Should_Be_Null()
        {
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.ID);

            this.engine.RemovePlayerFromSeat(1);

            Assert.IsNull(this.engine.Table.Seats[0].PlayerId);
        }

        /// <summary>
        /// Removing a player from the seat by SeatId and the seat is not occupied return false
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_SeatId_SeatNotOccupied_Should_Return_False()
        {
            var result = this.engine.RemovePlayerFromSeat(1);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Remove a player from the seat by PlayerID and the player is in a seat return true
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_PlayerId_PlayerInASeat_Should_Return_True()
        {
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.ID);

            var result = this.engine.RemovePlayerFromSeat(player.ID);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Remove a player from the seat by PlayerId and the player is in a seat should set the seats
        /// playerID to null
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_PlayerId_PlayerInASeat_Should_Set_Seats_PlayerId_To_Null()
        {
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.ID);

            this.engine.RemovePlayerFromSeat(player.ID);

            Assert.IsNull(this.engine.Table.Seats[0].PlayerId);
        }

        /// <summary>
        /// Remove a player from the seat by PlayerId and the player is not in a seat return false
        /// </summary>
        [TestMethod]
        public void RemovePlayerFromSeat_By_PlayerId_PlayerNotInASeat_Should_Return_False()
        {
            var result = this.engine.RemovePlayerFromSeat(Guid.NewGuid());
            Assert.IsFalse(result);
        }
    }
}
