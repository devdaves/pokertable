using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PokerTable.Web.Models.JsonModels;

namespace PokerTable.Web.Controllers
{
    public class PokerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateTable(string tableName)
        {
            var result = new CreateTableJson();
            return new JsonResult(){ Data = result};
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