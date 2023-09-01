using System;
using System.Data.Entity;
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

            return View(new UsersViewModel {
                Filters = filters,
                Users = _db.Users.OrderBy(u => u.Id).ToPagedList(filters.Page, filters.Rows),
            });
        }

        [Authorize, HttpPost]
        public ActionResult DeleteUser(int userID)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _db.Users.Find(userID);
                    if (user != null)
                        throw new Exception("O usuário não foi encontrado");

                    // salvar logs aqui

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();

                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return Json(new {
                        success = false,
                        message = (ex.InnerException ?? ex).Message,
                    });
                }
            }
        }
    }
}