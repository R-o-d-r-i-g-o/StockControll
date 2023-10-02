
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StockControll.Context;
using StockControll.Extensions;
using StockControll.Models;
using StockControll.ViewModel;

namespace StockControll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly LogExtension _log;
        private readonly User _loggedUser;

        public HomeController()
        {
            this._db = new AppDbContext();
            this._log = new LogExtension(_db);
            this._loggedUser = (User)System.Web.HttpContext.Current.Session["user"];
        }

        public ActionResult Index(FilterViewModel filters)
        {
            if (_loggedUser == null)
                return RedirectToAction("Index", "Login");

            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();

            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            filters.VerifyFilter();

            return View("Index", new HomeViewModel
            {
                Filters = filters,
                SaleDetails = GetSaleInfo(filters),
                ShoesRegistred = GetShoesRegistred(filters),
                ModelsRegistred = GetModelsRegistred(filters),
            });
        }

        private int GetModelsRegistred(FilterViewModel filters)
        {
            return _db.Categories
                      .Where(c => c.CreatedAt >= filters.StartDate
                               && c.CreatedAt <= filters.EndDate
                            )
                            .Select(_ => 1)
                            .Count();
        }

        private int GetShoesRegistred(FilterViewModel filters)
        {
            return _db.Shoes
                      .Where(s => s.CreatedAt >= filters.StartDate
                               && s.CreatedAt <= filters.EndDate
                            )
                            .Select(_ => 1)
                            .Count();
        }

        private List<SoldItens> GetSaleInfo(FilterViewModel filters)
        {
            return _db.Sales
             .Join(_db.Shoes, sale => sale.Shoe.BarCodeHash,
                              shoe => shoe.BarCodeHash, (sale, shoe) => new { sale, shoe }
             )
             .Where(q => q.sale.CreatedAt >= filters.StartDate
                      && q.sale.CreatedAt <= filters.EndDate
             )
             .Select(q => new SoldItens
             {
                 Sale = q.sale,
                 Shoe = q.shoe
             })
             .ToList();
        }
    }
}