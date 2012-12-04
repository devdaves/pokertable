using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokerTable.Game.Interfaces;
using PokerTable.Game.Models;
using PokerTable.Web.Controllers;
using PokerTable.Web.ViewModels;

namespace PokerTable.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// Engine Controller Tests
    /// </summary>
    [TestClass]
    public class EnigneControllerTests : TestBase
    {
        /// <summary>
        /// controller field
        /// </summary>
        private EngineController controller;

        /// <summary>
        /// engine mock
        /// </summary>
        private Mock<IEngine> engineMock;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.SetupHttpMock();
            this.engineMock = new Mock<IEngine>();

            this.controller = new EngineController(this.engineMock.Object);
            this.controller.ControllerContext = new System.Web.Mvc.ControllerContext(this.MockContext.Http.Object, new RouteData(), this.controller);
        }

        /// <summary>
        /// When the TableView action is called it should return a TableView view
        /// </summary>
        [TestMethod]
        public void TableViewAction_Returns_TableViewView()
        {
            this.engineMock.Setup(x => x.Table).Returns(new Table(string.Empty, string.Empty));
            this.TestViewName("TableView", () => { return this.controller.TableView(Guid.NewGuid()); });
        }

        /// <summary>
        /// When the TableView action is called it should call load table from the engine
        /// </summary>
        [TestMethod]
        public void TableViewAction_Should_LoadTable()
        {
            this.engineMock.Setup(x => x.Table).Returns(new Game.Models.Table(string.Empty, string.Empty));
            var result = this.controller.TableView(Guid.NewGuid());
            this.engineMock.Verify(x => x.LoadTable(It.IsAny<Guid>()), Times.Once());
        }

        /// <summary>
        /// When the TableView action is called it should return a TableViewViewModel model
        /// </summary>
        [TestMethod]
        public void TableViewAction_Should_Return_TableViewViewModel_Model()
        {
            var expectedTable = new Table(string.Empty, string.Empty);
            this.engineMock.Setup(x => x.Table).Returns(expectedTable);

            var result = this.controller.TableView(Guid.NewGuid()) as ViewResult;
            var model = result.Model as TableViewViewModel;

            Assert.AreEqual(expectedTable.Id, model.Table.Id);
        }

        /// <summary>
        /// When the PlayerView action is called it should return a PlayerView view
        /// </summary>
        [TestMethod]
        public void PlayerViewAction_Returns_PlayerViewView()
        {
            this.TestViewName("PlayerView", () => { return this.controller.PlayerView(Guid.NewGuid(), Guid.NewGuid()); });
        }
    }
}
