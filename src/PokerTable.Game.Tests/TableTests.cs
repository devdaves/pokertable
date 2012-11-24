using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Interfaces;

namespace PokerTable.Game.Tests
{
    /// <summary>
    /// Table Tests
    /// </summary>
    [TestClass]
    public class TableTests
    {
        /// <summary>
        /// Property tests for the Table object
        /// </summary>
        [TestClass]
        public class PropertyTests
        {
            /// <summary>
            /// When creating a new table make sure the table name passed to the constructor gets persisted to a property
            /// </summary>
            [TestMethod]
            public void CreatNewTable_Stores_TableName()
            {
                var expectedTableName = "tablename";
                var table = new Table(0, expectedTableName, string.Empty);

                Assert.AreEqual(expectedTableName, table.Name);
            }

            /// <summary>
            /// When creating a new table make sure the table password passed to the constructor gets persisted to a property
            /// </summary>
            [TestMethod]
            public void CreateNewTable_Stores_TablePassword()
            {
                var expectedTablePassword = "password";
                var table = new Table(0, string.Empty, expectedTablePassword);

                Assert.AreEqual(expectedTablePassword, table.Password);
            }
        }

        /// <summary>
        /// Seat tests for the Table object
        /// </summary>
        [TestClass]
        public class SeatTests
        {
            /// <summary>
            /// When creating a table, make sure the number of seats defined in the constructor get created.
            /// </summary>
            [TestMethod]
            public void CreateNewTable_5Seats_Confirm_Creation()
            {
                var table = new Table(5, string.Empty, string.Empty);
                Assert.AreEqual(5, table.Seats.Count());
            }

            /// <summary>
            /// When creating a table, passing in 0 for number of seats, seats property should not be null;
            /// </summary>
            [TestMethod]
            public void CreateNewTable_0Seats_Seats_Should_Not_Be_Null()
            {
                var table = new Table(0, string.Empty, string.Empty);
                Assert.IsNotNull(table.Seats);
            }

            /// <summary>
            /// When creating a table, passing in -5 for the number of seats, seats property should be at a count of 0
            /// </summary>
            [TestMethod]
            public void CreateNewTable_Negative5Seats_Seats_Count_Should_Be_0()
            {
                var table = new Table(-5, string.Empty, string.Empty);
                Assert.AreEqual(0, table.Seats.Count());
            }

            /// <summary>
            /// When adding a seat and the collection is empty, the first seat should have a seat id of 1
            /// </summary>
            [TestMethod]
            public void AddSeat_WhenSeatsEmpty_SeatId_Should_Be_1()
            {
                var table = new Table(0, string.Empty, string.Empty);
                table.AddSeat();
                Assert.AreEqual(1, table.Seats[0].Id);
            }

            /// <summary>
            /// When adding a seat and the collection has 1, the second seat should have a seat id of 2
            /// </summary>
            [TestMethod]
            public void AddSeat_WhenSeatsHas1_SeatId_Should_Be_2()
            {
                var table = new Table(1, string.Empty, string.Empty);
                table.AddSeat();
                Assert.AreEqual(2, table.Seats[1].Id);
            }

            /// <summary>
            /// removing a seat when the seat collection is empty should return false
            /// </summary>
            [TestMethod]
            public void RemoveSeat_WhenSeatsEmpty_Should_Return_False()
            {
                var table = new Table(0, string.Empty, string.Empty);
                var result = table.RemoveSeat(0);
                Assert.IsFalse(result);
            }

            /// <summary>
            /// removing a seat with an id that does not exist should return false
            /// </summary>
            [TestMethod]
            public void RemoveSeat_WhenSeatIdDoesNotExist_Should_Return_False()
            {
                var table = new Table(1, string.Empty, string.Empty);
                var result = table.RemoveSeat(4);
                Assert.IsFalse(result);
            }

            /// <summary>
            /// removing a seat with an id that exists, should return true
            /// </summary>
            [TestMethod]
            public void RemoveSeat_WhenSeatExists_Should_Return_True()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var result = table.RemoveSeat(3);
                Assert.IsTrue(result);
            }

            /// <summary>
            /// removing a seat with an id that exists, should remove the seat
            /// </summary>
            [TestMethod]
            public void RemoveSeat_WhenSeatExists_Should_Remove_Seat()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var result = table.RemoveSeat(3);
                Assert.IsFalse(table.Seats.Any(x => x.Id == 3));                
            }

            /// <summary>
            /// removing a seat with an id of 2 when there are 3 seats, should reorder the seats
            /// </summary>
            [TestMethod]
            public void RemoveSeat_WhenSeatExists_Remove2ndOf3Seats_Should_ReorderSeats()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.RemoveSeat(2);
                Assert.AreEqual(1, table.Seats[0].Id);
                Assert.AreEqual(2, table.Seats[1].Id);
            }
        }

        /// <summary>
        /// Deal test for the Table object
        /// </summary>
        [TestClass]
        public class DealTests
        {
            /// <summary>
            /// DealerExists should be false after creating a new table
            /// </summary>
            [TestMethod]
            public void DealerExists_Should_Be_False_When_CreateingNewTable()
            {
                var table = new Table(5, string.Empty, string.Empty);
                var result = table.DealerExists();
                Assert.IsFalse(result);
            }

            /// <summary>
            /// DealerExists should be true if one of the seats is marked as the dealer
            /// </summary>
            [TestMethod]
            public void DealerExists_Should_Be_True_When_A_Seat_Is_Marked_As_Dealer()
            {
                var table = new Table(5, string.Empty, string.Empty);
                table.SetDealer(2);
                var result = table.DealerExists();
                Assert.IsTrue(result);
            }

            /// <summary>
            /// Setting the dealer when no seats exists should do nothing
            /// </summary>
            [TestMethod]
            public void SetDealer_NoSeatsDefined_Should_Do_Nothing()
            {
                var table = new Table(0, string.Empty, string.Empty);
                table.SetDealer(1);
            }

            /// <summary>
            /// setting the dealer to an invalid seat id, DealerExists should return false
            /// </summary>
            [TestMethod]
            public void SetDealer_InvalidSeatId_DealerExists_Returns_False()
            {
                var table = new Table(5, string.Empty, string.Empty);
                table.SetDealer(1);
                table.SetDealer(6);
                var result = table.DealerExists();
                Assert.IsFalse(result);
            }

            /// <summary>
            /// There can only be one dealer at any given time.
            /// </summary>
            [TestMethod]
            public void SetDealer_ShouldNoAllowMultipleDealers()
            {
                var table = new Table(5, string.Empty, string.Empty);
                table.SetDealer(1);
                table.SetDealer(2);
                table.SetDealer(3);
                Assert.AreEqual(1, table.Seats.Count(x => x.IsDealer == true));
            }

            /// <summary>
            /// Calling next dealer when no seats exists should do nothing
            /// </summary>
            [TestMethod]
            public void NextDealer_NoSeatsDefined_Should_Do_Nothing()
            {
                var table = new Table(0, string.Empty, string.Empty);
                table.NextDealer();
            }

            /// <summary>
            /// Calling next dealer when no dealer is defined and no players are in seats should do nothing
            /// </summary>
            [TestMethod]
            public void NextDealer_NoDealerDefined_NoPlayersInSeats_Should_Do_Nothing()
            {
                var table = new Table(5, string.Empty, string.Empty);
                table.NextDealer();
            }

            /// <summary>
            /// Next dealer when no dealer is defined and all seats have players the first seat should be the dealer
            /// </summary>
            [TestMethod]
            public void NextDealer_NoDelearsDefined_AllSeatsWithPlayers_FirstSeat_Should_Be_Dealer()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID; 
                }
                
                table.NextDealer();
                Assert.IsTrue(table.Seats[0].IsDealer);
            }

            /// <summary>
            /// when all seats have players, seat 1 is dealer, NextDealer should be seat 2
            /// </summary>
            [TestMethod]
            public void NextDealer_AllSeatsWtihPlayers_SeatOneIsDealer_NextDealer_Should_Be_SeatTwo()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.SetDealer(1);
                table.NextDealer();
                Assert.IsTrue(table.Seats[1].IsDealer);
            }

            /// <summary>
            /// seat1 has a player, seat2 does not have a player, seat3 has a player.  seat1 is the dealer
            /// NextDealer should skip seat 2 and seat 3 should be dealer
            /// </summary>
            [TestMethod]
            public void NextDealer_Seat1Player_Seat2NoPlayer_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test3"));

                table.Seats[0].PlayerId = table.Players[0].ID;
                table.Seats[2].PlayerId = table.Players[1].ID;

                table.SetDealer(1);
                table.NextDealer();
                Assert.IsTrue(table.Seats[2].IsDealer);
            }

            /// <summary>
            /// seat1 has a player, seat2 has player sitting out, seat3 has a player.  seat1 is the dealer
            /// NextDealer should skip seat 2 and seat 3 should be dealer
            /// </summary>
            [TestMethod]
            public void NextDealer_Seat1Player_Seat2PlayerSittingOut_Seat3Player_Seat1IsDealer_NextDealer_Should_Be_Seat3()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                table.Players[1].State = Player.States.SittingOut;

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.SetDealer(1);
                table.NextDealer();
                Assert.IsTrue(table.Seats[2].IsDealer);                
            }

            /// <summary>
            /// Deal Flop should put 1 card in the burn list, 3 cards in the public cards list
            /// </summary>
            [TestMethod]
            public void DealFlop_Should_Burn1_Deal3_To_PublicCards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.DealFlop();
                Assert.AreEqual(1, table.Burn.Count());
                Assert.AreEqual(3, table.PublicCards.Count());
            }

            /// <summary>
            /// Deal Flop should take top card and put in burn list
            /// </summary>
            [TestMethod]
            public void DealFlop_Should_BurnFirstCard()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var topCard = table.Deck.Cards[0];
                table.DealFlop();
                Assert.AreEqual(topCard.Name(), table.Burn[0].Name());
            }

            /// <summary>
            /// Deal Flop should take 2,3 and 4 cards and put in public cards list
            /// </summary>
            [TestMethod]
            public void DealFlop_Should_Deal_2nd_3rd_4th_Cards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var card2 = table.Deck.Cards[1];
                var card3 = table.Deck.Cards[2];
                var card4 = table.Deck.Cards[3];
                table.DealFlop();

                Assert.AreEqual(card2.Name(), table.PublicCards[0].Name());
                Assert.AreEqual(card3.Name(), table.PublicCards[1].Name());
                Assert.AreEqual(card4.Name(), table.PublicCards[2].Name());
            }

            /// <summary>
            /// Deal Turn should put 1 card in the burn list, 1 cards in the public cards list
            /// </summary>
            [TestMethod]
            public void DealTurn_Should_Burn1_Deal1_To_PublicCards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.DealTurn();
                Assert.AreEqual(1, table.Burn.Count());
                Assert.AreEqual(1, table.PublicCards.Count());
            }

            /// <summary>
            /// Deal turn should take top card and put in burn list
            /// </summary>
            [TestMethod]
            public void DealTurn_Should_BurnFirstCard()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var topCard = table.Deck.Cards[0];
                table.DealTurn();
                Assert.AreEqual(topCard.Name(), table.Burn[0].Name());
            }

            /// <summary>
            /// Deal Turn should take 2 card and put in public cards list
            /// </summary>
            [TestMethod]
            public void DealTurn_Should_Deal_2nd_Card()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var card2 = table.Deck.Cards[1];
                table.DealTurn();

                Assert.AreEqual(card2.Name(), table.PublicCards[0].Name());
            }

            /// <summary>
            /// Deal River should put 1 card in the burn list, 1 cards in the public cards list
            /// </summary>
            [TestMethod]
            public void DealRiver_Should_Burn1_Deal1_To_PublicCards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.DealRiver();
                Assert.AreEqual(1, table.Burn.Count());
                Assert.AreEqual(1, table.PublicCards.Count());
            }

            /// <summary>
            /// Deal River should take top card and put in burn list
            /// </summary>
            [TestMethod]
            public void DealRiver_Should_BurnFirstCard()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var topCard = table.Deck.Cards[0];
                table.DealRiver();
                Assert.AreEqual(topCard.Name(), table.Burn[0].Name());
            }

            /// <summary>
            /// Deal River should take 2 card and put in public cards list
            /// </summary>
            [TestMethod]
            public void DealRiver_Should_Deal_2nd_Card()
            {
                var table = new Table(3, string.Empty, string.Empty);
                var card2 = table.Deck.Cards[1];
                table.DealRiver();

                Assert.AreEqual(card2.Name(), table.PublicCards[0].Name());
            }

            /// <summary>
            /// Deal Players should give each available player 2 cards
            /// </summary>
            [TestMethod]
            public void DealPlayers_EachAvailablePlayer_Should_Get_2_Cards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.NextDealer();
                table.DealPlayers();

                Assert.AreEqual(2, table.Players[0].Cards.Count());
                Assert.AreEqual(2, table.Players[1].Cards.Count());
                Assert.AreEqual(2, table.Players[2].Cards.Count());
            }

            /// <summary>
            /// Deal players, players set to status other then available should not get cards
            /// </summary>
            [TestMethod]
            public void DealPlayer_NotAvailablePlayers_Should_Not_Get_Cards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.Players[1].State = Player.States.SittingOut;

                table.NextDealer();
                table.DealPlayers();

                Assert.AreEqual(0, table.Players[1].Cards.Count());
            }

            /// <summary>
            /// Trying to deal the players cards and no dealer is set should not deal cards
            /// </summary>
            [TestMethod]
            public void DealPlayers_NoDealer_ShouldNotDealCards()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.DealPlayers();

                Assert.AreEqual(0, table.Players[0].Cards.Count());
                Assert.AreEqual(0, table.Players[1].Cards.Count());
                Assert.AreEqual(0, table.Players[2].Cards.Count());
            }

            /// <summary>
            /// Deal Players with 3 seats filled with available players, seat 1 is dealer, make
            /// sure cards are dealt correctly.
            /// </summary>
            [TestMethod]
            public void DealPlayers_3SeatsWithAvailablePlayers_Seat1IsDealer_ConfirmDealOrder()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.SetDealer(1);
                table.DealPlayers();

                Assert.AreEqual(table.Deck.Cards[0].Name(), table.Players[1].Cards[0].Name()); //// first card in deck should be first card dealt to 2nd player
                Assert.AreEqual(table.Deck.Cards[1].Name(), table.Players[2].Cards[0].Name()); //// second card in deck should be first card dealt to 3rd player
                Assert.AreEqual(table.Deck.Cards[2].Name(), table.Players[0].Cards[0].Name()); //// third card in deck should be first card dealt to 1st player

                Assert.AreEqual(table.Deck.Cards[3].Name(), table.Players[1].Cards[1].Name()); //// fourth card in deck should be second card dealt to 2nd player
                Assert.AreEqual(table.Deck.Cards[4].Name(), table.Players[2].Cards[1].Name()); //// fifth card in deck should be second card dealt to 3rd player
                Assert.AreEqual(table.Deck.Cards[5].Name(), table.Players[0].Cards[1].Name()); //// sixth card in deck should be second card dealt to 1st player
            }

            /// <summary>
            /// Deal Players with 3 seats filled with available players, seat 2 is dealer, make
            /// sure cards are dealt correctly.
            /// </summary>
            [TestMethod]
            public void DealPlayers_3SeatsWithAvailablePlayers_Seat2IsDealer_ConfirmDealOrder()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.SetDealer(2);
                table.DealPlayers();

                Assert.AreEqual(table.Deck.Cards[0].Name(), table.Players[2].Cards[0].Name()); //// first card in deck should be first card dealt to 3rd player
                Assert.AreEqual(table.Deck.Cards[1].Name(), table.Players[0].Cards[0].Name()); //// second card in deck should be first card dealt to 1st player
                Assert.AreEqual(table.Deck.Cards[2].Name(), table.Players[1].Cards[0].Name()); //// third card in deck should be first card dealt to 2nd player

                Assert.AreEqual(table.Deck.Cards[3].Name(), table.Players[2].Cards[1].Name()); //// fourth card in deck should be second card dealt to 3rd player
                Assert.AreEqual(table.Deck.Cards[4].Name(), table.Players[0].Cards[1].Name()); //// fifth card in deck should be second card dealt to 1st player
                Assert.AreEqual(table.Deck.Cards[5].Name(), table.Players[1].Cards[1].Name()); //// sixth card in deck should be second card dealt to 2nd player
            }

            /// <summary>
            /// Deal Players with 3 seats filled with available players, seat 3 is dealer, make
            /// sure cards are dealt correctly.
            /// </summary>
            [TestMethod]
            public void DealPlayers_3SeatsWithAvailablePlayers_Seat3IsDealer_ConfirmDealOrder()
            {
                var table = new Table(3, string.Empty, string.Empty);
                table.Players.Add(new Player("test1"));
                table.Players.Add(new Player("test2"));
                table.Players.Add(new Player("test3"));

                for (int i = 0; i < 3; i++)
                {
                    table.Seats[i].PlayerId = table.Players[i].ID;
                }

                table.SetDealer(3);
                table.DealPlayers();

                Assert.AreEqual(table.Deck.Cards[0].Name(), table.Players[0].Cards[0].Name()); //// first card in deck should be first card dealt to 1st player
                Assert.AreEqual(table.Deck.Cards[1].Name(), table.Players[1].Cards[0].Name()); //// second card in deck should be first card dealt to 2nd player
                Assert.AreEqual(table.Deck.Cards[2].Name(), table.Players[2].Cards[0].Name()); //// third card in deck should be first card dealt to 3rd player

                Assert.AreEqual(table.Deck.Cards[3].Name(), table.Players[0].Cards[1].Name()); //// fourth card in deck should be second card dealt to 1st player
                Assert.AreEqual(table.Deck.Cards[4].Name(), table.Players[1].Cards[1].Name()); //// fifth card in deck should be second card dealt to 2nd player
                Assert.AreEqual(table.Deck.Cards[5].Name(), table.Players[2].Cards[1].Name()); //// sixth card in deck should be second card dealt to 3rd player
            }
        }

        /// <summary>
        /// Player tests for the Table object
        /// </summary>
        [TestClass]
        public class PlayerTests
        {
            /// <summary>
            /// AddPlayer player not already on table, then player should be added
            /// </summary>
            [TestMethod]
            public void AddPlayer_PlayerDoesNotExist_Should_Add_Player()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);

                Assert.AreEqual(playerToAdd.Name, table.Players[0].Name);
            }

            /// <summary>
            /// AddPlayer player not already on table, should return true
            /// </summary>
            [TestMethod]
            public void AddPlayer_PlayerDoesNotExist_Should_Return_True()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                var result = table.AddPlayer(playerToAdd);

                Assert.IsTrue(result);
            }

            /// <summary>
            /// AddPlayer player already on table, should not add player again.
            /// </summary>
            [TestMethod]
            public void AddPlayer_PlayerExists_Should_Not_Add_Player_Again()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);
                table.AddPlayer(playerToAdd);

                Assert.AreEqual(1, table.Players.Count());
            }

            /// <summary>
            /// AddPlayer player already on table, should return false
            /// </summary>
            [TestMethod]
            public void AddPlayer_PlayerExists_Should_Return_False()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);
                var result = table.AddPlayer(playerToAdd);

                Assert.IsFalse(result);
            }

            /// <summary>
            /// RemovePlayer, if player is in seat, make sure they are removed.
            /// </summary>
            [TestMethod]
            public void RemovePlayer_PlayerInSeat_PlayerRemoved_From_Seat()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);
                table.AssignSeatToPlayer(1, playerToAdd.ID);

                table.RemovePlayer(playerToAdd.ID);

                Assert.AreEqual(null, table.Seats[0].PlayerId);
            }

            /// <summary>
            /// RemovePlayer player exists, player is removed
            /// </summary>
            [TestMethod]
            public void RemovePlayer_PlayerExists_Player_Is_Removed()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);

                table.RemovePlayer(playerToAdd.ID);

                Assert.AreEqual(0, table.Players.Count());
            }

            /// <summary>
            /// RemovePlayer player exists, should return true
            /// </summary>
            [TestMethod]
            public void RemovePlayer_PlayerExists_Should_Return_True()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var playerToAdd = new Player("test");
                table.AddPlayer(playerToAdd);

                var result = table.RemovePlayer(playerToAdd.ID);

                Assert.IsTrue(result);
            }

            /// <summary>
            /// RemovePlayer player does not exist should return false
            /// </summary>
            [TestMethod]
            public void RemovePlayer_PlayerDoesNotExist_Should_Return_False()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var result = table.RemovePlayer(Guid.NewGuid());
                Assert.IsFalse(result);
            }
        }

        /// <summary>
        /// Miscellaneous Tests for Table Object
        /// </summary>
        [TestClass]
        public class MiscTests
        {
            /// <summary>
            /// Reset should set a new deck
            /// </summary>
            [TestMethod]
            public void Reset_Should_Set_New_Deck()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var oldDeck = Utils.DeckCardsToString(table.Deck);
                table.Reset();
                var newDeck = Utils.DeckCardsToString(table.Deck);

                Assert.AreNotEqual(oldDeck, newDeck);
            }

            /// <summary>
            /// Reset should clear the burn list
            /// </summary>
            [TestMethod]
            public void Reset_Should_Clear_Burn_List()
            {
                var table = new Table(4, string.Empty, string.Empty);
                table.DealFlop();
                table.Reset();

                Assert.AreEqual(0, table.Burn.Count());
            }

            /// <summary>
            /// Reset should clear the public cards list
            /// </summary>
            [TestMethod]
            public void Reset_Should_Clear_PublicCards_List()
            {
                var table = new Table(4, string.Empty, string.Empty);
                table.DealFlop();
                table.Reset();

                Assert.AreEqual(0, table.PublicCards.Count());
            }

            /// <summary>
            /// Reset should reset all player on the table
            /// </summary>
            [TestMethod]
            public void Reset_Should_Reset_All_Player_On_Table()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player1 = new Mock<IPlayer>();
                var player2 = new Mock<IPlayer>();

                player1.SetupAllProperties();
                player2.SetupAllProperties();

                player1.Object.ID = Guid.NewGuid();
                player2.Object.ID = Guid.NewGuid();

                table.AddPlayer(player1.Object);
                table.AddPlayer(player2.Object);

                table.Reset();

                player1.Verify(x => x.Reset(), Times.Once());
                player2.Verify(x => x.Reset(), Times.Once());
            }

            /// <summary>
            /// A player can not be in two seats, if player is in another seat on the table
            /// make sure they are moved.
            /// </summary>
            [TestMethod]
            public void AssignSeatToPlayer__PlayerAlreadyInSeat_Should_Be_Moved_To_New_Seat()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");

                table.AddPlayer(player);
                table.AssignSeatToPlayer(1, player.ID);
                table.AssignSeatToPlayer(2, player.ID);

                Assert.IsNull(table.Seats[0].PlayerId);
                Assert.AreEqual(player.ID, table.Seats[1].PlayerId);
            }

            /// <summary>
            /// Assigning a seat to a player and that seat is occupied return false
            /// </summary>
            [TestMethod]
            public void AssignSeatToPlayer_SeatOccupied_Should_Return_False()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player1 = new Player("player1");
                var player2 = new Player("player2");

                table.AddPlayer(player1);
                table.AddPlayer(player2);

                table.AssignSeatToPlayer(1, player1.ID);
                var result = table.AssignSeatToPlayer(1, player2.ID);

                Assert.IsFalse(result);
            }

            /// <summary>
            /// Assigning a seat to a player and the seat is not occupied return true
            /// </summary>
            [TestMethod]
            public void AssignSeatToPlayer_SeatNotOccupied_Should_Return_True()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");

                table.AddPlayer(player);

                var result = table.AssignSeatToPlayer(1, player.ID);

                Assert.IsTrue(result);
            }

            /// <summary>
            /// Assigning a seat to a player and the seat is not occupied, make sure
            /// the seat is assigned to the player
            /// </summary>
            [TestMethod]
            public void AssignSeatToPlayer_SeatNotOccupied_Player_Should_Be_Assigned_To_Seat()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");

                table.AddPlayer(player);

                table.AssignSeatToPlayer(1, player.ID);

                Assert.AreEqual(player.ID, table.Seats[0].PlayerId);            
            }

            /// <summary>
            /// Removing a player from the seat by SeatId and the seat is occupied return true
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_SeatId_SeatOccupied_Should_Return_True()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");
                table.AddPlayer(player);
                table.AssignSeatToPlayer(1, player.ID);

                var result = table.RemovePlayerFromSeat(1);

                Assert.IsTrue(result);
            }

            /// <summary>
            /// Removing a player from the seat by SeatId and the seat is occupied, make sure
            /// the playerID is set to null
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_SeatId_SeatOccupied_PlayerID_Should_Be_Null()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");
                table.AddPlayer(player);
                table.AssignSeatToPlayer(1, player.ID);

                table.RemovePlayerFromSeat(1);

                Assert.IsNull(table.Seats[0].PlayerId);
            }

            /// <summary>
            /// Removing a player from the seat by SeatId and the seat is not occupied return false
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_SeatId_SeatNotOccupied_Should_Return_False()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var result = table.RemovePlayerFromSeat(1);
                Assert.IsFalse(result);
            }

            /// <summary>
            /// Remove a player from the seat by PlayerID and the player is in a seat return true
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_PlayerId_PlayerInASeat_Should_Return_True()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");
                table.AddPlayer(player);
                table.AssignSeatToPlayer(1, player.ID);

                var result = table.RemovePlayerFromSeat(player.ID);

                Assert.IsTrue(result);
            }

            /// <summary>
            /// Remove a player from the seat by PlayerId and the player is in a seat should set the seats
            /// playerID to null
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_PlayerId_PlayerInASeat_Should_Set_Seats_PlayerId_To_Null()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var player = new Player("test");
                table.AddPlayer(player);
                table.AssignSeatToPlayer(1, player.ID);

               table.RemovePlayerFromSeat(player.ID);

                Assert.IsNull(table.Seats[0].PlayerId);
            }

            /// <summary>
            /// Remove a player from the seat by PlayerId and the player is not in a seat return false
            /// </summary>
            [TestMethod]
            public void RemovePlayerFromSeat_By_PlayerId_PlayerNotInASeat_Should_Return_False()
            {
                var table = new Table(4, string.Empty, string.Empty);
                var result = table.RemovePlayerFromSeat(Guid.NewGuid());
                Assert.IsFalse(result);
            }
        }
    }
}
