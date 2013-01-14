using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using PokerTable.Game;
using PokerTable.Game.Exceptions;
using PokerTable.Game.Interfaces;
using PokerTable.Web.Hubs;
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
        /// Table view.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>
        /// returns a table view
        /// </returns>
        public ActionResult TableView(Guid tableId)
        {
            this.engine.LoadTable(tableId);

            ////var player = new PokerTable.Game.Models.Player("Dave");
            ////this.engine.AddPlayer(player);
            ////this.engine.AssignSeatToPlayer(3, player.ID);
            ////this.engine.DealPlayers();
            ////this.engine.DealFlop();
            ////this.engine.DealTurn();
            ////this.engine.DealRiver();

            var model = new TableViewViewModel();
            model.Table = this.engine.Table;

            var serializer = new JavaScriptSerializer();
            model.TableJSON = serializer.Serialize(this.engine.Table);

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

        /// <summary>
        /// Assign the seat to the player.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seatId">The seat id.</param>
        /// <param name="playerId">The player id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult AssignSeatToPlayer(Guid tableId, int seatId, Guid playerId)
        {
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.AssignSeatToPlayer(seatId, playerId);

            var response = new
            {
                result = "success",
                message = "Player assigned to seat",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Sets the seat as the dealer.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="seatId">The seat id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult SetSeatAsDealer(Guid tableId, int seatId)
        {
            this.engine.LoadTable(tableId);
            this.engine.SetDealer(seatId);

            var response = new
            {
                result = "success",
                message = "Dealer Set to seat:" + seatId.ToString(),
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Deals the players.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult DealPlayers(Guid tableId)
        {
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.DealPlayers();

            var response = new
            {
                result = "success",
                message = "Players Dealt",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Deals the flop.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult DealFlop(Guid tableId)
        { 
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.DealFlop();

            var response = new
            {
                result = "success",
                message = "Flop Dealt",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };            
        }

        /// <summary>
        /// Deals the turn.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost] 
        public JsonResult DealTurn(Guid tableId)
        {
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.DealTurn();

            var response = new
            {
                result = "success",
                message = "Turn Dealt",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Deals the river.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost] 
        public JsonResult DealRiver(Guid tableId)
        {
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.DealRiver();

            var response = new
            {
                result = "success",
                message = "River Dealt",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Resets the table
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult ResetTable(Guid tableId)
        {
            // TODO: error checking
            this.engine.LoadTable(tableId);
            this.engine.ResetTable();
            this.engine.NextDealer();

            var response = new
            {
                result = "success",
                message = "Table Reset",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
        }

        /// <summary>
        /// Refresh Table
        /// </summary>
        /// <param name="tableId">The table id</param>
        /// <returns>returns JSON result</returns>
        [HttpPost]
        public JsonResult RefreshTable(Guid tableId)
        {
            this.engine.LoadTable(tableId);
            
            var response = new
            {
                result = "success",
                message = "Table Refreshed",
                table = this.engine.Table
            };
            return new JsonResult() { Data = response };
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

                // broadcast over the hub to refresh the table;
                var context = GlobalHost.ConnectionManager.GetHubContext<PokerTableHub>();
                context.Clients.All.refreshTable();

                return new JsonResult() { Data = response };
            }
            catch (Exception)
            {
            }

            return new JsonResult() { Data = new { result = "fail", message = "Invalid table password." } };
        }
    }
}
