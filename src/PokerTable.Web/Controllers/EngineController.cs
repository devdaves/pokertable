using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PokerTable.Game;

namespace PokerTable.Web.Controllers
{
    /// <summary>
    /// Engine controller
    /// </summary>
    public class EngineController : Controller
    {
        /// <summary>
        /// engine field
        /// </summary>
        private Engine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineController" /> class.
        /// </summary>
        public EngineController()
        {
            this.engine = new Engine();
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public ActionResult CreateTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return new JsonResult() { Data = new { result = "fail", message = "Table name is a required field." } };
            }

            this.engine.CreateNewTable(10, tableName);
            var response = new
            {
                result = "success",
                message = "Table create successfully",
                tableId = this.engine.Table.Id.ToString()
            };

            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Tests the table create.
        /// </summary>
        /// <returns>returns a GUID of the table created.</returns>
        public ActionResult TestTableCreate()
        {
            this.engine.CreateNewTable(10, "abc123");
            return new ContentResult() { Content = this.engine.Table.Id.ToString() };
        }

        /// <summary>
        /// Table view.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns a table view
        /// </returns>
        public ActionResult TableView(Guid tableId)
        {
            return this.View();
        }
    }
}
