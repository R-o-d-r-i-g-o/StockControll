using System;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using StockControll.Commons;
using StockControll.Context;
using StockControll.Models;
using StockControll.ViewModel;

namespace StockControll.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        public ActionResult Index(FilterViewModel filters)
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();

            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            return View("Index", new LogViewModel {
                Filters = filters,
                Logs = GetLogs(filters),
            });
        }

        [HttpPost]
        public ActionResult GenerateReportFromLogs(FilterViewModel filters)
        {
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var logs = GetLogs(filters);
                var fileName = "Relatório de registros";

                var excelPackage = new ExcelPackage();
                var ws = excelPackage.Workbook.Worksheets.Add(fileName);

                // Título
                ws.Cells["A1"].Value = "RELATÓRIO DOS REGISTROS DA APLICAÇÃO";
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Detalhes da pesquisa
                ws.Cells["A3"].Value = "Nº da página: ";
                ws.Cells["B3"].Value = $"{filters.Page}";
                ws.Cells["A4"].Value = "Nº de itens p/ pág:";
                ws.Cells["B4"].Value = $"{filters.Rows}";
                ws.Cells["A5"].Value = "Total de registros:";
                ws.Cells["B5"].Value = $"{logs.TotalItemCount}";

                // Cabeçalho do relatório
                ws.Cells[8, 1, 8, 5].Style.Numberformat.Format = "@";
                ws.Cells[8, 1, 8, 5].Value = "";
                ws.Cells[8, 1, 8, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[8, 1, 8, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[8, 1, 8, 5].Style.Font.Bold = true;
                ws.Cells[8, 1, 8, 5].Style.Font.Size = 12;

                // Colunas do relatório
                ws.Cells["A8"].Value = "Id do registro";
                ws.Cells["B8"].Value = "Código do colaborador";
                ws.Cells["C8"].Value = "Colaborador";
                ws.Cells["D8"].Value = "Data do registro";
                ws.Cells["E8"].Value = "Tipo de atividade";
                ws.Cells["F8"].Value = "Mensagem";

                // dados da tabela
                char col = 'A';
                var row = 8;

                if (logs == null || !logs.Any())
                    throw new Exception("Não tem dados para consulta");

                foreach (var l in logs) {
                    col = 'A';
                    row++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.Id }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.User.Id }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.User.Name }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.CreatedAt }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.ActivityType.GetDescription() }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ l.Message }";
                    col++;
                }

                ws.Column(01).Width = 20;
                ws.Column(02).Width = 25;
                ws.Column(03).Width = 30;
                ws.Column(04).Width = 30;
                ws.Column(05).Width = 25;
                ws.Column(06).Width = 40;

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

        private IPagedList<Log> GetLogs(FilterViewModel filters)
        {
            filters.VerifyFilter();

            var query = _db.Logs.Select(l => l);

            if (!string.IsNullOrEmpty(filters.SearchUser))
                query = query.Where(l => l.User.Name == filters.SearchUser);

            return query.OrderByDescending(l => l.Id).ToPagedList(filters.Page, filters.Rows);
        }
    }
}