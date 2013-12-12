using System.Web.Mvc;

namespace PokerTable.Web.Controllers
{
    public class PokerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}