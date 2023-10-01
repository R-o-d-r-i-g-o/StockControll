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
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        private readonly LogExtension _log;
        private readonly User _loggedUser;

        public UserController()
        {
            this._db = new AppDbContext();
            this._log = new LogExtension(_db);
            this._loggedUser = (User)System.Web.HttpContext.Current.Session["user"];
        }

        public ActionResult Index(FilterViewModel filters)
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();

            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            if (_loggedUser.UserType != Enums.UserType.Admin) {
                ViewBag.ErrorMessage = $"{ _loggedUser.Name }, você não tem permissao para entrar nesta tela";

                return RedirectToAction("Index", "Home");
            }

            return View("Index", new UsersViewModel {
                Filters = filters,
                Users = GetUsers(filters),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User newUser)
        {
            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    VerifyInfo(newUser);

                    var alreadyCreated = _db.Users.Where(u => u.Email == newUser.Email || u.CPF == newUser.CPF).ToList();
                    if (alreadyCreated == null)
                        throw new Exception("Já existem usuários com esses dados cadastrados");

                    newUser.Password = AuthSettings.CalculateMD5(newUser.Password);

                    _log.AddMessage(
                        Enums.ActivityType.CreateItems,
                        $"O usuário criou um no usuário com o privilégio de { newUser.UserType.GetDescription() }"
                    );

                    _db.Users.Add(newUser);
                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Usuário criado com sucesso.";
                    transaction.Commit();

                    return RedirectToAction("Index", "User");
                }
                catch (Exception ex) {
                    TempData["ErrorMessage"] = (ex.InnerException ?? ex).Message;
                    transaction.Rollback();

                    return Index(new FilterViewModel());
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    VerifyInfo(user);

                    var registreduser = _db.Users.Find(user.Id);
                    if (registreduser == null)
                        throw new Exception("O usuário não foi encontrado");

                    if (user.Password != user.Password.HidePassword())
                        registreduser.Password = AuthSettings.CalculateMD5(user.Password);

                    registreduser.CPF = user.CPF;
                    registreduser.Name = user.Name;
                    registreduser.Email = user.Email;

                    _log.AddMessage(
                        Enums.ActivityType.EditItems,
                        $"O usuário editou o outro de nome { user.Name } e código { user.Id }"
                    );

                    _db.Entry(registreduser).State = EntityState.Modified;
                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Usuário editado com sucesso.";
                    transaction.Commit();

                    return RedirectToAction("Index", "User");
                }
                catch (Exception ex) {
                    TempData["ErrorMessage"] = (ex.InnerException ?? ex).Message;
                    transaction.Rollback();

                    return Index(new FilterViewModel());
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(int userID)
        {
            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    var user = _db.Users.Find(userID);
                    if (user == null)
                        throw new Exception("O usuário não foi encontrado");

                    _log.AddMessage(
                       Enums.ActivityType.DeleteItems,
                       $"O usuário deletou o usuário de id: {userID}"
                    );

                    user.DeletedAt = DateTime.Now;

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
                    ws.Cells[$"{col}{row}"].Value = $"{ u.CPF.AddMaskToCpf() }";
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

        private void VerifyInfo(User user)
        {
            if (!ModelState.IsValid)
                throw new Exception("Preencha o formulário corretamente");

            if (!user.Email.IsEmailValid())
                throw new Exception("A estrutura do e-mail está errada");

            if (!user.CPF.IsValidCPF())
                throw new Exception("O documento não é valido");
        }

        private IPagedList<User> GetUsers(FilterViewModel filters)
        {
            filters.VerifyFilter();

            var query = _db.Users.Select(u => u);

            if (!string.IsNullOrEmpty(filters.SearchUser))
                query = query.Where(u => u.Name == filters.SearchUser);

            return query.OrderBy(u => u.Id).ToPagedList(filters.Page, filters.Rows);
        }
    }
}
