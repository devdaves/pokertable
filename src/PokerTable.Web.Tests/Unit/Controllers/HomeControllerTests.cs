using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerTable.Web.Controllers;

namespace PokerTable.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// Home Controller Tests
    /// </summary>
    [TestClass]
    public class HomeControllerTests : TestBase
    {
        /// <summary>
        /// controller field
        /// </summary>
        private HomeController controller;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.SetupHttpMock();
            this.controller = new HomeController();
            this.controller.ControllerContext = new System.Web.Mvc.ControllerContext(this.MockContext.Http.Object, new RouteData(), this.controller);
        }

        /// <summary>
        /// Index action returns Index view
        /// </summary>
        [TestMethod]
        public void IndexAction_Returns_IndexView()
        {
            this.TestViewName("Index", () => { return this.controller.Index(); });
        }
    }
}
