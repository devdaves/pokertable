using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PokerTable.Game;
using PokerTable.Web.Models.JsonModels;

namespace PokerTable.Web.Controllers
{
    public class PokerController : Controller
    {
        private IEngine engine;

        public PokerController()
        {
            this.engine = new Engine();   
        }

        public PokerController(IEngine engine)
        {
            this.engine = engine;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return new JsonResult()
                {
                    Data = new CreateTableJson()
                    {
                        Status = 1,
                        FailureMessage = "Table Name is required."
                    }
                };
            }

            var result = this.FillJson<CreateTableJson>(x =>
            {
                this.engine.CreateNewTable(10, tableName);
                x.TableId = this.engine.Table.Id;
            });

            return new JsonResult() {Data = result};
        }


        private TJson FillJson<TJson>(Action<TJson> action)
            where TJson : JsonBase, new()
        {
            var json = new TJson();

            try
            {
                action.Invoke(json);
            }
            catch (Exception ex)
            {
                json.Status = 1;
                json.FailureMessage = ex.Message;
            }

            return json;
        }
    }
}