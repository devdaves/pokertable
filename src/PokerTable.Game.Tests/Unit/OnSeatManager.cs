using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class OnSeatManager
    {
        [TestClass]
        public class WhenAddSeat
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ShouldReturnAListOfSeats()
            {
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                var results = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.IsInstanceOfType(results, typeof (List<Seat>));
            }

            [TestMethod]
            public void ShouldAddASeat()
            {
                var expectedSeatCount = 1;
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(expectedSeatCount, seats.Count);
            }

            [TestMethod]
            public void ShouldAddSeatWithDealerSetToFalse()
            {
                var expectedValue = false;
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(expectedValue, seats[0].IsDealer);
            }

            [TestMethod]
            public void ShouldAddSeatWithDealOrderSetTo0()
            {
                var expectedValue = 0;
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(expectedValue, seats[0].DealOrder);
            }

            [TestMethod]
            public void ShouldAddSeatWithPlayerIdSetToNull()
            {
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(null, seats[0].PlayerId);
            }


            [TestMethod]
            public void ShouldAddSeatWithId1_WhenNoExistingSeats()
            {
                var expectedId = 1;
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(expectedId, seats[0].Id);
            }

            [TestMethod]
            public void ShouldAddSeatWithId2_When1ExistingSeat()
            {
                var expectedId = 2;
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                seats = this.seatManager.AddSeat(seats, someTableGuid);
                seats = this.seatManager.AddSeat(seats, someTableGuid);

                Assert.AreEqual(expectedId, seats[1].Id);
            }

            [TestMethod]
            public void ShouldSaveSeat()
            {
                var someTableGuid = Guid.NewGuid();
                var seats = new List<Seat>();
                
                this.seatManager.AddSeat(seats, someTableGuid);

                this.repositoryMock.Verify(x => x.SaveSeat(someTableGuid, It.IsAny<Seat>()), Times.Once());
            }
        }

        [TestClass]
        public class WhenAddManySeat
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ShouldReturnListOfSeats()
            {
                var someTableId = Guid.NewGuid();
                var result = this.seatManager.AddManySeats(3, someTableId);

                Assert.IsInstanceOfType(result, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldAddCorrectNumberOfSeats()
            {
                var expectedNumberOfSeats = 3;
                var someTableId = Guid.NewGuid();
                var result = this.seatManager.AddManySeats(expectedNumberOfSeats, someTableId);

                Assert.AreEqual(expectedNumberOfSeats, result.Count);
            }

            [TestMethod]
            public void ShouldAddSeatsWithCorrectIds()
            {
                var someTableId = Guid.NewGuid();
                var result = this.seatManager.AddManySeats(3, someTableId);

                Assert.AreEqual(1, result[0].Id);
                Assert.AreEqual(2, result[1].Id);
                Assert.AreEqual(3, result[2].Id);
            }

            [TestMethod]
            public void ShouldSaveSeats()
            {
                var someTableId = Guid.NewGuid();
                var result = this.seatManager.AddManySeats(3, someTableId);

                this.repositoryMock.Verify(x => x.SaveSeatAll(someTableId, It.IsAny<List<Seat>>()), Times.Once);
            }
        }

        [TestClass]
        public class WhenRemoveSeat
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ReturnsSeatList()
            {
                var response = this.seatManager.RemoveSeat(new List<Seat>(), 1, Guid.NewGuid());
                Assert.IsInstanceOfType(response, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldReturnSameSeatListIfSeatIdDoesNotExist()
            {
                var seats = this.BuildSeatList(3);
                var response = this.seatManager.RemoveSeat(seats, 5, Guid.NewGuid());

                Assert.AreEqual(3, response.Count);
                Assert.AreEqual(1, response[0].Id);
                Assert.AreEqual(2, response[1].Id);
                Assert.AreEqual(3, response[2].Id);
            }

            [TestMethod]
            public void ShouldReturn1LessSeat()
            {
                var expectedCount = 2;
                var seats = this.BuildSeatList(3);
                var response = this.seatManager.RemoveSeat(seats, 3, Guid.NewGuid());

                Assert.AreEqual(expectedCount, response.Count);
            }

            [TestMethod]
            public void ShouldRenumberSeatsAfterSeatThatIsRemoved()
            {
                var seats = this.BuildSeatList(4);
                var response = this.seatManager.RemoveSeat(seats, 2, Guid.NewGuid());

                Assert.AreEqual(1, seats[0].Id);
                Assert.AreEqual(2, seats[1].Id);
                Assert.AreEqual(3, seats[2].Id);
            }

            [TestMethod]
            public void ShouldRemoveSeatFromRepository()
            {
                var expectedTableId = Guid.NewGuid();
                var seats = this.BuildSeatList(4);
                var response = this.seatManager.RemoveSeat(seats, 2, expectedTableId);

                this.repositoryMock.Verify(x => x.RemoveSeat(expectedTableId, It.Is<Seat>(s => s.Id == 2)), Times.Once());
            }

            [TestMethod]
            public void ShouldSaveSeatInRepository()
            {
                var expectedTableId = Guid.NewGuid();
                var seats = this.BuildSeatList(4);
                var response = this.seatManager.RemoveSeat(seats, 2, expectedTableId);

                this.repositoryMock.Verify(x => x.SaveSeatAll(expectedTableId, It.IsAny<List<Seat>>()), Times.Once());
            }

            private List<Seat> BuildSeatList(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }
        }

        [TestClass]
        public class WenRemovePlayerFromSeatBySeatId
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ReturnsSeatList()
            {
                var response = this.seatManager.RemovePlayerFromSeat(new List<Seat>(), 1, Guid.NewGuid());
                Assert.IsInstanceOfType(response, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldRemovePlayerIfInSeat()
            {
                var seats = this.BuildSeatList(3);
                seats[0].PlayerId = Guid.NewGuid();

                var results = this.seatManager.RemovePlayerFromSeat(seats, 1, Guid.NewGuid());

                Assert.AreEqual(null, results[0].PlayerId);
            }

            [TestMethod]
            public void ShouldSaveSeatToRepository()
            {
                var expectedTableId = Guid.NewGuid();
                var seats = this.BuildSeatList(3);
                seats[0].PlayerId = Guid.NewGuid();

                var results = this.seatManager.RemovePlayerFromSeat(seats, 1, expectedTableId);

                this.repositoryMock.Verify(x => x.SaveSeat(expectedTableId, It.Is<Seat>(s => s.Id == 1)), Times.Once());
            }

            private List<Seat> BuildSeatList(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }
        }

        [TestClass]
        public class WenRemovePlayerFromSeatByPlayerId
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ReturnsSeatList()
            {
                var response = this.seatManager.RemovePlayerFromSeat(new List<Seat>(), Guid.NewGuid(), Guid.NewGuid());
                Assert.IsInstanceOfType(response, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldRemovePlayerIfInSeat()
            {
                var playerId = Guid.NewGuid();
                var seats = this.BuildSeatList(3);
                seats[0].PlayerId = playerId;

                var results = this.seatManager.RemovePlayerFromSeat(seats, playerId, Guid.NewGuid());

                Assert.AreEqual(null, results[0].PlayerId);
            }

            [TestMethod]
            public void ShouldSaveSeatToRepository()
            {
                var playerId = Guid.NewGuid();
                var expectedTableId = Guid.NewGuid();
                var seats = this.BuildSeatList(3);
                seats[0].PlayerId = playerId;

                var results = this.seatManager.RemovePlayerFromSeat(seats, playerId, expectedTableId);

                this.repositoryMock.Verify(x => x.SaveSeat(expectedTableId, It.Is<Seat>(s => s.Id == 1)), Times.Once());
            }

            private List<Seat> BuildSeatList(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }
        }

        [TestClass]
        public class WhenAssignSeatToPlayer
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ReturnsSeatList()
            {
                var result = this.seatManager.AssignSeatToPlayer(new List<Seat>(), 1, Guid.NewGuid(), Guid.NewGuid());
                Assert.IsInstanceOfType(result, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldRemovePlayerFromExistingSeat()
            {
                var playerId = Guid.NewGuid();
                var seats = this.BuildSeatList(2);
                seats[0].PlayerId = playerId;

                var results = this.seatManager.AssignSeatToPlayer(seats, 2, playerId, Guid.NewGuid());

                Assert.AreEqual(null, results[0].PlayerId);
            }

            [TestMethod]
            public void ShouldPutPlayerInNewSeat()
            {
                var playerId = Guid.NewGuid();
                var seats = this.BuildSeatList(2);
                seats[0].PlayerId = playerId;

                var results = this.seatManager.AssignSeatToPlayer(seats, 2, playerId, Guid.NewGuid());

                Assert.AreEqual(playerId, results[1].PlayerId);
            }

            [TestMethod]
            public void ShouldSaveAllSeatsToRepository()
            {
                var expectedTableId = Guid.NewGuid();
                var result = this.seatManager.AssignSeatToPlayer(new List<Seat>(), 1, Guid.NewGuid(), expectedTableId);

                this.repositoryMock.Verify(x => x.SaveSeatAll(expectedTableId, It.IsAny<List<Seat>>()), Times.Once());
            }

            private List<Seat> BuildSeatList(int howMany)
            {
                return this.seatManager.AddManySeats(howMany, Guid.NewGuid());
            }
        }

        [TestClass]
        public class WhenSaveSeats
        {
            private Mock<IRepository> repositoryMock;
            private ISeatManager seatManager;

            [TestInitialize]
            public void Setup()
            {
                this.repositoryMock = new Mock<IRepository>();
                this.seatManager = new SeatManager(this.repositoryMock.Object);
            }

            [TestMethod]
            public void ShouldReturnSeatList()
            {
                var results = this.seatManager.SaveSeats(new List<Seat>(), Guid.NewGuid());

                Assert.IsInstanceOfType(results, typeof(List<Seat>));
            }

            [TestMethod]
            public void ShouldSaveSeatsToRepository()
            {
                var expectedTableId = Guid.NewGuid();
                this.seatManager.SaveSeats(new List<Seat>(), expectedTableId);

                this.repositoryMock.Verify(x => x.SaveSeatAll(expectedTableId, It.IsAny<List<Seat>>()), Times.Once());
            }
        }
    }
}