using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PokerTable.Game;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;
using PokerTable.Web.ViewModels;

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
        private IEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineController" /> class.
        /// </summary>
        public EngineController()
        {
            this.engine = new Engine();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineController" /> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public EngineController(IEngine engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>returns JSON result of table</returns>
        [HttpPost]
        public JsonResult CreateTable(string tableName)
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
        /// Joins the table.
        /// </summary>
        /// <param name="tablePassword">The table password.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <returns>returns JSON of player</returns>
        [HttpPost]
        public JsonResult JoinTable(string tablePassword, string playerName)
        {
            if (string.IsNullOrEmpty(tablePassword))
            {
                return new JsonResult() { Data = new { result = "fail", message = "Table password is a required field." } };
            }

            if (string.IsNullOrEmpty(playerName))
            {
                return new JsonResult() { Data = new { result = "fail", message = "Player name is a required field." } };
            }

            try
            {
                var playerId = this.engine.JoinTable(tablePassword, playerName);
                var response = new 
                {
                    result = "sucess",
                    message = "Player added successfully",
                    tableId = this.engine.Table.Id,
                    playerId = playerId
                };
                return new JsonResult() { Data = response };
            }
            catch (Exception)
            {
            }

            return new JsonResult() { Data = new { result = "fail", message = "Invalid table password." } };
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
            this.engine.LoadTable(tableId);
            var model = new TableViewViewModel();
            model.Table = this.engine.Table;
            return this.View("TableView", model);
        }

        /// <summary>
        /// Players the view.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="playerId">The player id.</param>
        /// <returns>
        /// returns a player view
        /// </returns>
        public ActionResult PlayerView(Guid tableId, Guid playerId)
        {
            return this.View("PlayerView");
        }
    }
}
