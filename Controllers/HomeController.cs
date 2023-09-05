using System.Web.Mvc;
using StockControll.Context;

namespace StockControll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}