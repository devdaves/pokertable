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
            return this.View("Index");
        }
    }
}
