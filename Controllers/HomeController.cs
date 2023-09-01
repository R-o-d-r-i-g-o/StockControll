using System.Linq;
using System.Web.Mvc;
using PagedList;
using StockControll.Context;
using StockControll.ViewModel;

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

        public ActionResult Contact(FilterViewModel filters)
        {
            filters.VerifyFilter();
            ViewBag.Message = "Your contact page.";

            return View(new UsersViewModel
            {
                Filters = filters,
                Users = _db.Users.OrderBy(u => u.Id).ToPagedList(filters.Page, filters.Rows),
            });
        }
    }
}