using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class TableTests
    {
        private Engine engine;

        private Mock<IRepository> repositoryMock;
        private IDeckBuilder deckBuilder;
        private IDealer dealer;
        private Mock<ISeatManager> seatManagerMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.deckBuilder = new DeckBuilder();
            this.dealer = new Dealer();
            this.seatManagerMock = new Mock<ISeatManager>();
            this.repositoryMock.Setup(x => x.TablePasswordExists(It.IsAny<string>())).Returns(false);
            this.engine = new Engine(this.repositoryMock.Object, this.deckBuilder, this.dealer, this.seatManagerMock.Object);
            this.engine.CreateNewTable(10, "TestName");
        }

        [TestMethod]
        public void CreatNewTable_Stores_TableName()
        {
            const string expectedTableName = "TestName";
            Assert.AreEqual(expectedTableName, this.engine.Table.Name);
        }

        [TestMethod]
        public void CreateNewTable_Stores_TablePassword()
        {
            Assert.AreEqual(5, this.engine.Table.Password.Length);
        }

        [TestMethod]
        public void CreateNewTable_5Seats_Confirm_Creation()
        {
            this.engine.CreateNewTable(5, string.Empty);
            Assert.AreEqual(5, this.engine.Table.Seats.Count());
        }

        [TestMethod]
        public void CreateNewTable_0Seats_Seats_Should_Not_Be_Null()
        {
            this.engine.CreateNewTable(0, string.Empty);
            Assert.IsNotNull(this.engine.Table.Seats);
        }

        [TestMethod]
        public void CreateNewTable_Negative5Seats_Seats_Count_Should_Be_0()
        {
            this.engine.CreateNewTable(-5, string.Empty);
            Assert.AreEqual(0, this.engine.Table.Seats.Count());
        }
        
        [TestMethod]
        public void DealerExists_Should_Be_False_When_CreateingNewTable()
        {
            var result = this.engine.DealerExists();
            Assert.IsFalse(result);
        }

        [TestMethod, Ignore]
        public void DealerExists_Should_Be_True_When_A_Seat_Is_Marked_As_Dealer()
        {
            this.engine.SetDealer(2);
            var result = this.engine.DealerExists();
            Assert.IsTrue(result);
        }

        [TestMethod, Ignore]
        public void SetDealer_NoSeatsDefined_Should_Do_Nothing()
        {
            this.engine.CreateNewTable(0, string.Empty);
            this.engine.SetDealer(1);
        }

        [TestMethod, Ignore]
        public void SetDealer_InvalidSeatId_DealerExists_Returns_False()
        {
            this.engine.CreateNewTable(5, string.Empty);
            this.engine.SetDealer(1);
            this.engine.SetDealer(6);
            var result = this.engine.DealerExists();
            Assert.IsFalse(result);
        }

        [TestMethod, Ignore]
        public void SetDealer_ShouldNoAllowMultipleDealers()
        {
            this.engine.SetDealer(1);
            this.engine.SetDealer(2);
            this.engine.SetDealer(3);
            Assert.AreEqual(1, this.engine.Table.Seats.Count(x => x.IsDealer));
        }

        [TestMethod]
        public void NextDealer_NoSeatsDefined_Should_Do_Nothing()
        {
            this.engine.CreateNewTable(0, string.Empty);
            this.engine.NextDealer();
        }

        [TestMethod]
        public void NextDealer_NoDealerDefined_NoPlayersInSeats_Should_Do_Nothing()
        {
            this.engine.NextDealer();
        }

        [TestMethod]
        public void NextDealer_NoDelearsDefined_AllSeatsWithPlayers_FirstSeat_Should_Be_Dealer()
        {
            this.engine.CreateNewTable(3, string.Empty);

            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].Id);

            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[0].IsDealer);
        }

        [TestMethod, Ignore]
        public void NextDealer_AllSeatsWtihPlayers_SeatOneIsDealer_NextDealer_Should_Be_SeatTwo()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[1].IsDealer);
        }

        [TestMethod, Ignore]
        public void NextDealer_Seat1Player_Seat2NoPlayer_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[1].Id);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[2].IsDealer);
        }

        [TestMethod, Ignore]
        public void NextDealer_Seat1Player_Seat2PlayerSittingOut_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.Table.Players[1].State = Player.States.SittingOut;

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.SetDealer(1);
            this.engine.NextDealer();
            Assert.IsTrue(this.engine.Table.Seats[2].IsDealer);
        }

        [TestMethod]
        public void DealFlop_Should_Burn1_Deal3_To_PublicCards()
        {
            this.engine.DealFlop();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(3, this.engine.Table.PublicCards.Count());
        }

        [TestMethod]
        public void DealFlop_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealFlop();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

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

        [TestMethod]
        public void DealTurn_Should_Burn1_Deal1_To_PublicCards()
        {
            this.engine.DealTurn();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(1, this.engine.Table.PublicCards.Count());
        }

        [TestMethod]
        public void DealTurn_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealTurn();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

        [TestMethod]
        public void DealTurn_Should_Deal_2nd_Card()
        {
            var card2 = this.engine.Table.Deck.Cards[1];
            this.engine.DealTurn();

            Assert.AreEqual(card2.Name(), this.engine.Table.PublicCards[0].Name());
        }

        [TestMethod]
        public void DealRiver_Should_Burn1_Deal1_To_PublicCards()
        {
            this.engine.DealRiver();
            Assert.AreEqual(1, this.engine.Table.Burn.Count());
            Assert.AreEqual(1, this.engine.Table.PublicCards.Count());
        }

        [TestMethod]
        public void DealRiver_Should_BurnFirstCard()
        {
            var topCard = this.engine.Table.Deck.Cards[0];
            this.engine.DealRiver();
            Assert.AreEqual(topCard.Name(), this.engine.Table.Burn[0].Name());
        }

        [TestMethod]
        public void DealRiver_Should_Deal_2nd_Card()
        {
            var card2 = this.engine.Table.Deck.Cards[1];
            this.engine.DealRiver();

            Assert.AreEqual(card2.Name(), this.engine.Table.PublicCards[0].Name());
        }

        [TestMethod]
        public void DealPlayers_EachAvailablePlayer_Should_Get_2_Cards()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.NextDealer();
            this.engine.DealPlayers();

            Assert.AreEqual(2, this.engine.Table.Players[0].Cards.Count());
            Assert.AreEqual(2, this.engine.Table.Players[1].Cards.Count());
            Assert.AreEqual(2, this.engine.Table.Players[2].Cards.Count());
        }

        [TestMethod]
        public void DealPlayer_NotAvailablePlayers_Should_Not_Get_Cards()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].Id);

            this.engine.Table.Players[1].State = Player.States.SittingOut;

            this.engine.NextDealer();
            this.engine.DealPlayers();

            Assert.AreEqual(0, this.engine.Table.Players[1].Cards.Count());
        }

        [TestMethod]
        public void DealPlayers_NoDealer_ShouldNotDealCards()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[2].Id);

            this.engine.DealPlayers();

            Assert.AreEqual(0, this.engine.Table.Players[0].Cards.Count());
            Assert.AreEqual(0, this.engine.Table.Players[1].Cards.Count());
            Assert.AreEqual(0, this.engine.Table.Players[2].Cards.Count());
        }

        [TestMethod, Ignore]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat1IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.SetDealer(1);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// first card in deck should be first card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// second card in deck should be first card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// third card in deck should be first card dealt to 1st player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// fourth card in deck should be second card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// fifth card in deck should be second card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// sixth card in deck should be second card dealt to 1st player
        }

        [TestMethod, Ignore]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat2IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.SetDealer(2);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// first card in deck should be first card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// second card in deck should be first card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// third card in deck should be first card dealt to 2nd player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// fourth card in deck should be second card dealt to 3rd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// fifth card in deck should be second card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// sixth card in deck should be second card dealt to 2nd player
        }

        [TestMethod, Ignore]
        public void DealPlayers_3SeatsWithAvailablePlayers_Seat3IsDealer_ConfirmDealOrder()
        {
            this.engine.CreateNewTable(3, string.Empty);
            this.engine.AddPlayer(new Player("test1"));
            this.engine.AddPlayer(new Player("test2"));
            this.engine.AddPlayer(new Player("test3"));

            this.engine.AssignSeatToPlayer(1, this.engine.Table.Players[0].Id);
            this.engine.AssignSeatToPlayer(2, this.engine.Table.Players[1].Id);
            this.engine.AssignSeatToPlayer(3, this.engine.Table.Players[2].Id);

            this.engine.SetDealer(3);
            this.engine.DealPlayers();

            Assert.AreEqual(this.engine.Table.Deck.Cards[0].Name(), this.engine.Table.Players[0].Cards[0].Name()); //// first card in deck should be first card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[1].Name(), this.engine.Table.Players[1].Cards[0].Name()); //// second card in deck should be first card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[2].Name(), this.engine.Table.Players[2].Cards[0].Name()); //// third card in deck should be first card dealt to 3rd player

            Assert.AreEqual(this.engine.Table.Deck.Cards[3].Name(), this.engine.Table.Players[0].Cards[1].Name()); //// fourth card in deck should be second card dealt to 1st player
            Assert.AreEqual(this.engine.Table.Deck.Cards[4].Name(), this.engine.Table.Players[1].Cards[1].Name()); //// fifth card in deck should be second card dealt to 2nd player
            Assert.AreEqual(this.engine.Table.Deck.Cards[5].Name(), this.engine.Table.Players[2].Cards[1].Name()); //// sixth card in deck should be second card dealt to 3rd player
        }

        [TestMethod]
        public void AddPlayer_PlayerDoesNotExist_Should_Add_Player()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            Assert.AreEqual(playerToAdd.Name, this.engine.Table.Players[0].Name);
        }

        [TestMethod]
        public void AddPlayer_PlayerDoesNotExist_Should_Return_True()
        {
            var playerToAdd = new Player("test");
            var result = this.engine.AddPlayer(playerToAdd);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddPlayer_PlayerExists_Should_Not_Add_Player_Again()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            this.engine.AddPlayer(playerToAdd);

            Assert.AreEqual(1, this.engine.Table.Players.Count());
        }

        [TestMethod]
        public void AddPlayer_PlayerExists_Should_Return_False()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            var result = this.engine.AddPlayer(playerToAdd);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemovePlayer_PlayerInSeat_PlayerRemoved_From_Seat()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);
            this.engine.AssignSeatToPlayer(1, playerToAdd.Id);

            this.engine.RemovePlayer(playerToAdd.Id);

            Assert.AreEqual(null, this.engine.Table.Seats[0].PlayerId);
        }

        [TestMethod]
        public void RemovePlayer_PlayerExists_Player_Is_Removed()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            this.engine.RemovePlayer(playerToAdd.Id);

            Assert.AreEqual(0, this.engine.Table.Players.Count());
        }

        [TestMethod]
        public void RemovePlayer_PlayerExists_Should_Return_True()
        {
            var playerToAdd = new Player("test");
            this.engine.AddPlayer(playerToAdd);

            var result = this.engine.RemovePlayer(playerToAdd.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemovePlayer_PlayerDoesNotExist_Should_Return_False()
        {
            var result = this.engine.RemovePlayer(Guid.NewGuid());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ResetTable_Should_Set_New_Deck()
        {
            var oldDeck = Utils.DeckCardsToString(this.engine.Table.Deck);
            this.engine.ResetTable();
            var newDeck = Utils.DeckCardsToString(this.engine.Table.Deck);

            Assert.AreNotEqual(oldDeck, newDeck);
        }

        [TestMethod]
        public void ResetTable_Should_Clear_Burn_List()
        {
            this.engine.DealFlop();
            this.engine.ResetTable();

            Assert.AreEqual(0, this.engine.Table.Burn.Count());
        }

        [TestMethod]
        public void ResetTable_Should_Clear_PublicCards_List()
        {
            this.engine.DealFlop();
            this.engine.ResetTable();

            Assert.AreEqual(0, this.engine.Table.PublicCards.Count());
        }

        [TestMethod]
        public void RemovePlayerFromSeat_By_PlayerId_PlayerInASeat_Should_Set_Seats_PlayerId_To_Null()
        {
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.AssignSeatToPlayer(1, player.Id);

            this.engine.RemovePlayerFromSeat(player.Id);

            Assert.IsNull(this.engine.Table.Seats[0].PlayerId);
        }

        [TestMethod]
        public void BuildRandomCode_Should_Return_String_With_Length_Of_5()
        {
            var result = this.engine.BuildRandomCode();
            Assert.AreEqual(5, result.Length);
        }
    }
}
