using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokerTable.Web.Tests.Unit
{
    /// <summary>
    /// Test Base Class
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Gets or sets the mock context.
        /// </summary>
        /// <value>
        /// The mock context.
        /// </value>
        public MockContext MockContext { get; set; }

        /// <summary>
        /// Sets up the HTTP mock.
        /// </summary>
        internal void SetupHttpMock()
        {
            this.MockContext = new MockContext();
        }

        /// <summary>
        /// Tests the name of the view.
        /// </summary>
        /// <param name="expectedView">The expected view.</param>
        /// <param name="action">The action.</param>
        internal void TestViewName(string expectedView, Func<ActionResult> action)
        {
            var result = action.Invoke() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedView, result.ViewName);
        }
    }
}
