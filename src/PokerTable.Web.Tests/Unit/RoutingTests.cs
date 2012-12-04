using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokerTable.Web.Tests.Unit
{
    /// <summary>
    /// Routing Tests
    /// </summary>
    [TestClass]
    public class RoutingTests : TestBase
    {
        /// <summary>
        /// route collection field
        /// </summary>
        private RouteCollection routeCollection;

        /// <summary>
        /// Run before every test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.routeCollection = RouteTable.Routes;
            this.routeCollection.Clear();
            PokerTable.Web.RouteConfig.RegisterRoutes(this.routeCollection);

            this.SetupHttpMock();
        }

        /// <summary>
        /// / URL should go to the Home controller and Index action
        /// </summary>
        [TestMethod]
        public void Empty_URL_Should_Go_To_HomeController_IndexAction()
        {
            this.TestRoute("~/", "Home", "Index");
        }

        /// <summary>
        /// /Home URL should go to the Home controller and Index action
        /// </summary>
        [TestMethod]
        public void Home_URL_Should_Go_To_HomeController_IndexAction()
        {
            this.TestRoute("~/Home", "Home", "Index");
        }

        /// <summary>
        /// /Home/Index URL should go to the Home controller and Index action
        /// </summary>
        [TestMethod]
        public void HomeIndex_URL_Should_Go_To_HomeController_IndexAction()
        {
            this.TestRoute("~/Home/Index", "Home", "Index");
        }

        /// <summary>
        /// /Engine/CreateTable URL should go to the Engine controller and CreateTable action
        /// </summary>
        [TestMethod]
        public void EngineCreateTable_URL_Should_Go_To_EngineController_CreateTableAction()
        {
            this.TestRoute("~/Engine/CreateTable", "Engine", "CreateTable");
        }

        /// <summary>
        /// /Engine/JoinTable URL should go to the Engine controller and JoinTable action
        /// </summary>
        [TestMethod]
        public void EngineJoinTable_URL_Should_Go_To_EngineController_JoinTableAction()
        {
            this.TestRoute("~/Engine/JoinTable", "Engine", "JoinTable");
        }

        /// <summary>
        /// /Engine/TableView URL should go to the Engine controller and TableView action
        /// </summary>
        [TestMethod]
        public void EngineTableView_URL_Should_Go_To_EngineController_TableViewAction()
        {
            this.TestRoute("~/Engine/TableView", "Engine", "TableView");
        }

        /// <summary>
        /// /Engine/PlayerView URL should go to the Engine controller and PlayerView action
        /// </summary>
        [TestMethod]
        public void EnginePlayerView_URL_Should_Go_To_EngineController_PlayerViewAction()
        {
            this.TestRoute("~/Engine/PlayerView", "Engine", "PlayerView");
        }

        /// <summary>
        /// Tests the route.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedController">The expected controller.</param>
        /// <param name="expectedAction">The expected action.</param>
        private void TestRoute(string url, string expectedController, string expectedAction)
        {
            var r = this.GetRoute(url);
            Assert.IsNotNull(r, "Did not find the route");
            Assert.AreEqual(expectedController, r.Values["Controller"]);
            Assert.AreEqual(expectedAction, r.Values["Action"]);
        }

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>returns the route</returns>
        private RouteData GetRoute(string url)
        {
            MockContext.Http.Setup(x => x.Request.Path).Returns(url);
            MockContext.Http.Setup(x => x.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
            var r = this.routeCollection.GetRouteData(MockContext.Http.Object);
            return r;
        }
    }
}
