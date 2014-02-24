using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Data.Interfaces;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Models;

namespace PokerTable.Game.Tests.Unit
{
    [TestClass]
    public class EngineTests
    {
        private Engine engine;

        private Mock<IRepository> repositoryMock;
        private IDeckBuilder deckBuilder;
        private IDealer dealer;

        [TestInitialize]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.deckBuilder = new DeckBuilder();
            this.dealer = new Dealer();
            this.repositoryMock.Setup(x => x.TablePasswordExists(It.IsAny<string>())).Returns(false);
            this.engine = new Engine(this.repositoryMock.Object, this.deckBuilder, this.dealer);
            this.engine.CreateNewTable(4, string.Empty);
        }

        [TestMethod]
        public void JoinTable_InvalidTablePassword_DoesNotTryAndLoadTable()
        {
            var someTableGuid = Guid.NewGuid();
            this.repositoryMock.Setup(x => x.GetTableIdByTablePassword(It.IsAny<string>())).Returns((Guid?) null);
               
            this.engine.JoinTable("Some Password", "Some Player");

            this.repositoryMock.Verify(x => x.LoadTable(It.IsAny<Guid>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(TableDoesNotExistException))]
        public void JoinTable_ValidTablePassword_TableNull_Returns_TableDoesNotExistException()
        {
            var someTableGuid = Guid.NewGuid();
            this.repositoryMock.Setup(x => x.GetTableIdByTablePassword(It.IsAny<string>())).Returns(someTableGuid);
            this.repositoryMock.Setup(x => x.LoadTable(someTableGuid)).Returns((Table) null);

            this.engine.JoinTable("Some Table Password", "Some Player");
        }

        [TestMethod]
        public void JoinTable_ValidPlayer_PlayerGetsAdded()
        {
            var expectedPlayerName = "Super Player";
            var someTableGuid = Guid.NewGuid();
            this.repositoryMock.Setup(x => x.GetTableIdByTablePassword(It.IsAny<string>())).Returns(someTableGuid);
            this.repositoryMock.Setup(x => x.LoadTable(someTableGuid)).Returns(new Table("TableName", "TablePassword"));

            this.engine.JoinTable("Some Password", expectedPlayerName);

            Assert.AreEqual(expectedPlayerName, this.engine.Table.Players[0].Name);
        }

        [TestMethod]
        public void JoingTable_SameNameAsExistingPlayer_Returns_ExistingPlayersGuid()
        {
            var expectedPlayer = new Player("RoboCop");
            var table = new Table("TableName", "TablePassword");
            table.Players.Add(expectedPlayer);
            var someTableGuid = Guid.NewGuid();
            this.repositoryMock.Setup(x => x.GetTableIdByTablePassword(It.IsAny<string>())).Returns(someTableGuid);
            this.repositoryMock.Setup(x => x.LoadTable(someTableGuid)).Returns(table);

            var playerGuid = this.engine.JoinTable("Some Password", expectedPlayer.Name);

            Assert.AreEqual(expectedPlayer.Id, playerGuid);
        }
    }
}
