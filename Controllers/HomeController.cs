using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using StockControll.Context;
using StockControll.Models;
using System.Web.Security;
using StockControll.ViewModel;
using System.Threading.Tasks;
using StockControll.Commons;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace StockControll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        private readonly User _loggedUser = (User)System.Web.HttpContext.Current.Session["user"];

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
            ViewBag.Message = "Your contact page.";

            return View(new UsersViewModel {
                Filters = filters,
                Users = GetUsers(filters),
            });
        }

        [HttpPost]
        public ActionResult DeleteUser(int userID)
        {
            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    var user = _db.Users.Find(userID);
                    if (user != null)
                        throw new Exception("O usuário não foi encontrado");

                    // salvar logs aqui

                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();

                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex) {
                    transaction.Rollback();

                    return Json(new {
                        success = false,
                        message = (ex.InnerException ?? ex).Message,
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult GenerateReportFromTransactions(FilterViewModel filters)
        {
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var users = GetUsers(filters);
                var fileName = "Relatório de usuários";

                var excelPackage = new ExcelPackage();
                var ws = excelPackage.Workbook.Worksheets.Add(fileName);

                // Título
                ws.Cells["A1"].Value = "RELATÓRIO DOS USUÁRIOS DA APLICAÇÃO";
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Detalhes da pesquisa
                ws.Cells["A3"].Value = "Nº da página: ";
                ws.Cells["B3"].Value = $"{filters.Page}";
                ws.Cells["A4"].Value = "Nº de itens p/ pág:";
                ws.Cells["B4"].Value = $"{filters.Rows}";
                ws.Cells["A5"].Value = "Total de registros:";
                ws.Cells["B5"].Value = $"{users.TotalItemCount}";

                // Cabeçalho do relatório
                ws.Cells[8, 1, 8, 14].Style.Numberformat.Format = "@";
                ws.Cells[8, 1, 8, 14].Value = "";
                ws.Cells[8, 1, 8, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[8, 1, 8, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[8, 1, 8, 14].Style.Font.Bold = true;
                ws.Cells[8, 1, 8, 14].Style.Font.Size = 12;

                // Colunas do relatório
                ws.Cells["A8"].Value = "Id";
                ws.Cells["B8"].Value = "Nome";
                ws.Cells["C8"].Value = "E-mail";
                ws.Cells["D8"].Value = "CPF";
                ws.Cells["E8"].Value = "Cargo";
                ws.Cells["F8"].Value = "Criado em";
                ws.Cells["G8"].Value = "Status";

                // dados da tabela
                char col = 'A';
                var row = 8;

                if (users == null || !users.Any())
                    throw new Exception("Não tem dados para consulta");

                foreach (var u in users) {
                    col = 'A';
                    row++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.Id }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.Name }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.Email }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.CPF }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.UserType.GetDescription() }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ u.CreatedAt }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ (u.DeletedAt.HasValue ? "Inativo" : "Ativo") }";
                    col++;
                }

                ws.Column(01).Width = 20;
                ws.Column(02).Width = 25;
                ws.Column(03).Width = 25;
                ws.Column(04).Width = 25;
                ws.Column(05).Width = 15;
                ws.Column(06).Width = 20;
                ws.Column(07).Width = 15;

                byte[] file = excelPackage.GetAsByteArray();

                return File(file, "application/octet-stream", fileName);
            }
            catch (Exception ex) {
                return Json(new {
                    status = "error",
                    message = (ex.InnerException ?? ex).Message
                });
            }
        }

        private IPagedList<User> GetUsers(FilterViewModel filters)
        {
            filters.VerifyFilter();

            return _db.Users.OrderBy(u => u.Id).ToPagedList(filters.Page, filters.Rows);
        }
    }
}