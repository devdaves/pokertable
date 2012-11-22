using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }
    }
}
