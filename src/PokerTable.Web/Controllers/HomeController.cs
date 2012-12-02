using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PokerTable.Game;

namespace PokerTable.Web.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>the index view</returns>
        public ActionResult Index()
        {
            // lets create a table and have it saved to table storage so 
            // we can see if the app setting in azure is working correctly.
            var engine = new Engine();
            engine.CreateNewTable(5, "Test", "TestPassword");
            ViewBag.TableId = engine.Table.Id.ToString();

            return this.View();
        }
    }
}
