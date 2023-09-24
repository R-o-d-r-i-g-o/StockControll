using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using StockControll.Commons;
using StockControll.Context;
using StockControll.Extensions;
using StockControll.Models;
using StockControll.ViewModel;

namespace StockControll.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly LogExtension _log;

        public CategoryController()
        {
            this._db = new AppDbContext();
            this._log = new LogExtension(_db);
        }

        public ActionResult Index(FilterViewModel filters)
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();

            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            return View("Index", new CategoryViewModel {
                Filters = filters,
                Categories = GetCategories(filters),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(Category newCategory)
        {
            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    if (!ModelState.IsValid)
                        throw new Exception("Preencha o formulário corretamente");

                    var alreadyCreated = _db.Categories.Where(c => c.Id == newCategory.Id || c.Name == newCategory.Name).Any();
                    if (alreadyCreated)
                        throw new Exception("Já existem modelos de procutos com esse nome cadastrados");

                    _log.AddMessage(
                        Enums.ActivityType.CreateItems,
                        $"O usuário criou um novo modelo de produtos com o nome de: { newCategory.Name }"
                    );

                    _db.Categories.Add(newCategory);
                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Modelo criado com sucesso.";
                    transaction.Commit();

                    return RedirectToAction("Index", "Category");
                }
                catch (Exception ex) {
                    TempData["ErrorMessage"] = (ex.InnerException ?? ex).Message;
                    transaction.Rollback();

                    return Index(new FilterViewModel());
                }
            }
        }

        private IPagedList<Category> GetCategories(FilterViewModel filters)
        {
            filters.VerifyFilter();

            var query = _db.Categories.Select(c => c);

            if (!string.IsNullOrEmpty(filters.SearchColor))
                query = query.Where(c => c.Color == filters.SearchColor);

            if (!string.IsNullOrEmpty(filters.SearchSole))
                query = query.Where(c => c.Sole == filters.SearchSole);

            if (!string.IsNullOrEmpty(filters.SearchCategoryName))
                query = query.Where(c => c.Name == filters.SearchCategoryName);

            return query.OrderBy(c => c.Id).ToPagedList(filters.Page, filters.Rows);
        }
    }
}
