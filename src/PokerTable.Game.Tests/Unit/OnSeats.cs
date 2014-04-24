using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class OnSeats
    {
        [TestClass]
        public class WhenAdd
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldAddASeat()
            {
                var expectedSeatCount = 1;
                this.seats.Add();

                Assert.AreEqual(expectedSeatCount, seats.Count);
            }

            [TestMethod]
            public void ShouldAddSeatWithDealerSetToFalse()
            {
                var expectedValue = false;
                this.seats.Add();

                Assert.AreEqual(expectedValue, seats[0].IsDealer);
            }

            [TestMethod]
            public void ShouldAddSeatWithDealOrderSetTo0()
            {
                var expectedValue = 0;
                this.seats.Add();

                Assert.AreEqual(expectedValue, seats[0].DealOrder);
            }

            [TestMethod]
            public void ShouldAddSeatWithPlayerIdSetToNull()
            {
                this.seats.Add();

                Assert.AreEqual(null, seats[0].PlayerId);
            }

            [TestMethod]
            public void ShouldAddSeatWithId1_WhenNoExistingSeats()
            {
                var expectedId = 1;
                this.seats.Add();

                Assert.AreEqual(expectedId, seats[0].Id);
            }

            [TestMethod]
            public void ShouldAddSeatWithId2_When1ExistingSeat()
            {
                var expectedId = 2;
                this.seats.Add(); 
                this.seats.Add();

                Assert.AreEqual(expectedId, seats[1].Id);
            }
        }

        [TestClass]
        public class WhenClear
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldClearSeats()
            {
                var expectedCount = 0;
                this.seats.Add();
                this.seats.Add();

                this.seats.Clear();

                Assert.AreEqual(expectedCount, this.seats.Count);
            }
        }

        [TestClass]
        public class WhenRemove
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldReturnSameSeatListIfSeatIdDoesNotExist()
            {
                this.seats.Add();
                this.seats.Add();
                this.seats.Add();

                this.seats.Remove(5);

                Assert.AreEqual(3, this.seats.Count);
                Assert.AreEqual(1, this.seats[0].Id);
                Assert.AreEqual(2, this.seats[1].Id);
                Assert.AreEqual(3, this.seats[2].Id);
            }

            [TestMethod]
            public void ShouldReturn1LessSeat()
            {
                var expectedCount = 2;
                this.seats.Add();
                this.seats.Add();
                this.seats.Add();

                this.seats.Remove(1);

                Assert.AreEqual(expectedCount, this.seats.Count);
            }

            [TestMethod]
            public void ShouldRenumberSeatsAfterSeatThatIsRemoved()
            {
                this.seats.Add();
                this.seats.Add();
                this.seats.Add();
                this.seats.Add();

                this.seats.Remove(2);

                Assert.AreEqual(1, seats[0].Id);
                Assert.AreEqual(2, seats[1].Id);
                Assert.AreEqual(3, seats[2].Id);
            }
        }

        [TestClass]
        public class WenRemovePlayerFromSeatBySeatId
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldRemovePlayerIfInSeat()
            {
                this.seats.Add();
                this.seats.Add();
                seats[0].PlayerId = Guid.NewGuid();

                this.seats.RemovePlayerFromSeat(1);

                Assert.AreEqual(null, seats[0].PlayerId);
            }
        }

        [TestClass]
        public class WenRemovePlayerFromSeatByPlayerId
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldRemovePlayerIfInSeat()
            {
                var playerId = Guid.NewGuid();
                this.seats.Add();
                seats[0].PlayerId = playerId;

                this.seats.RemovePlayerFromSeat(playerId);

                Assert.AreEqual(null, seats[0].PlayerId);
            }
        }

        [TestClass]
        public class WhenAssignSeatToPlayer
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldRemovePlayerFromExistingSeat()
            {
                var playerId = Guid.NewGuid();
                this.seats.Add();
                this.seats.Add();
                seats[0].PlayerId = playerId;

                this.seats.AssignSeatToPlayer(2, playerId);

                Assert.AreEqual(null, seats[0].PlayerId);
            }

            [TestMethod]
            public void ShouldPutPlayerInNewSeat()
            {
                var playerId = Guid.NewGuid();
                this.seats.Add();
                this.seats.Add();
                seats[0].PlayerId = playerId;

                this.seats.AssignSeatToPlayer(2, playerId);

                Assert.AreEqual(playerId, seats[1].PlayerId);
            }
        }

        [TestClass]
        public class WhenDealerExists
        {
            private Seats seats;

            [TestInitialize]
            public void Setup()
            {
                this.seats = new Seats();
            }

            [TestMethod]
            public void ShouldReturnTrueIfDealerSet()
            {
                this.seats.Add();
                this.seats.Add();
                this.seats[0].IsDealer = true;
                this.seats.DealerExists();

                Assert.AreEqual(true, this.seats.DealerExists());
            }

            [TestMethod]
            public void ShouldReturnFalseIfDealerNotSet()
            {
                this.seats.Add();
                this.seats.Add();
                this.seats.DealerExists();

                Assert.AreEqual(false, this.seats.DealerExists());
            }
        }
    }
}
