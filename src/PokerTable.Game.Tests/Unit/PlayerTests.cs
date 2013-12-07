using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class PlayerTests
    {
        private Engine engine;

        private Mock<IRepository> repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.repositoryMock.Setup(x => x.TablePasswordExists(It.IsAny<string>())).Returns(false);
            this.engine = new Engine(this.repositoryMock.Object);
            this.engine.CreateNewTable(4, string.Empty);
        }

        [TestMethod]
        public void Creating_A_New_Player_Should_Have_ID()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Id);
        }

        [TestMethod]
        public void Creating_A_New_Player_Player_Name_Should_Have_Value()
        {
            const string expectedName = "test";
            var player = new Player(expectedName);

            Assert.IsNotNull(player);
            Assert.AreEqual(expectedName, player.Name);
        }

        [TestMethod]
        public void Create_A_New_Player_PlayerState_Should_Be_Available()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.AreEqual(Player.States.Available, player.State);
        }

        [TestMethod]
        public void Create_A_New_Player_Cards_Should_Be_Empty()
        {
            var player = new Player("test");
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.Cards);
            Assert.AreEqual(0, player.Cards.Count());
        }

        [TestMethod]
        public void Fold_Should_Set_Status_To_Folded()
        {
            const Player.States expectedState = Player.States.Folded;
            var player = new Player("test");
            this.engine.AddPlayer(player);
            this.engine.FoldPlayer(player.Id);

            Assert.IsNotNull(player);
            Assert.AreEqual(expectedState, player.State);
        }

        [TestMethod]
        public void Reset_Should_Set_Status_To_Available()
        {
            const Player.States expectedState = Player.States.Available;
            var player = new Player("test");
            this.engine.Table.Players.Add(player);

            this.engine.FoldPlayer(player.Id);
            this.engine.ResetPlayer(player.Id);

            Assert.AreEqual(expectedState, player.State);
        }

        [TestMethod]
        public void Reset_Should_Set_Cards_To_EmptyList()
        {
            var player = new Player("test");
            player.Cards.Add(new Card());
            player.Cards.Add(new Card());

            this.engine.Table.Players.Add(player);
            this.engine.ResetPlayer(player.Id);

            Assert.AreEqual(0, player.Cards.Count());
        }
    }
}
