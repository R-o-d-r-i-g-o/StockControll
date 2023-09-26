﻿using System;
using System.Collections.Generic;
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

        [HttpPost]
        public ActionResult GenerateReportOfCategories(FilterViewModel filters)
        {
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var fileName = "Relatório de usuários";
                var categories = GetCategories(filters);

                var excelPackage = new ExcelPackage();
                var ws = excelPackage.Workbook.Worksheets.Add(fileName);

                var listOfSizes = new int[] { 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 };

                // Título
                ws.Cells["A1"].Value = "RELATÓRIO DOS USUÁRIOS DA APLICAÇÃO";
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Detalhes da pesquisa
                ws.Cells["A3"].Value = "Nº da página: ";
                ws.Cells["B3"].Value = $"{ filters.Page }";
                ws.Cells["A4"].Value = "Nº de itens p/ pág:";
                ws.Cells["B4"].Value = $"{ filters.Rows }";
                ws.Cells["A5"].Value = "Total de registros:";
                ws.Cells["B5"].Value = $"{ categories.TotalItemCount }";

                // Cabeçalho do relatório
                ws.Cells[8, 1, 9, 34].Style.Numberformat.Format = "@";
                ws.Cells[8, 1, 9, 34].Value = "";
                ws.Cells[8, 1, 9, 34].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[8, 1, 9, 34].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[8, 1, 9, 34].Style.Font.Bold = true;
                ws.Cells[8, 1, 9, 34].Style.Font.Size = 12;
                ws.Cells["A7:A9"].Merge = true;
                ws.Cells["B7:B9"].Merge = true;
                ws.Cells["C7:C9"].Merge = true;
                ws.Cells["D7:D9"].Merge = true;
                ws.Cells["E7:Y8"].Merge = true;

                // Colunas do relatório
                // ws.Cells["A8"].Value = "Código";
                ws.Cells["A8"].Value = "Produto";
                ws.Cells["B8"].Value = "Cor";
                ws.Cells["C8"].Value = "Modelo do solado";
                ws.Cells["D8"].Value = "Descrição";
                ws.Cells["E8"].Value = "Tamanhos dos calçados";

                // tamanhos dos calçados
                ws.Cells["E9"].Value = "33";
                ws.Cells["F9"].Value = "34";
                ws.Cells["G9"].Value = "35";
                ws.Cells["H9"].Value = "36";
                ws.Cells["I9"].Value = "37";
                ws.Cells["J9"].Value = "38";
                ws.Cells["K9"].Value = "39";
                ws.Cells["L9"].Value = "40";
                ws.Cells["M9"].Value = "41";
                ws.Cells["N9"].Value = "42";
                ws.Cells["O9"].Value = "43";
                ws.Cells["P9"].Value = "44";
                ws.Cells["Q9"].Value = "45";
                ws.Cells["R9"].Value = "46";
                ws.Cells["S9"].Value = "47";
                ws.Cells["T9"].Value = "48";
                ws.Cells["U9"].Value = "49";
                ws.Cells["V9"].Value = "50";
                ws.Cells["W9"].Value = "51";
                ws.Cells["X9"].Value = "52";
                ws.Cells["Y9"].Value = "Total";

                // dados da tabela
                char col = 'A';
                var row = 9;

                if (categories == null || !categories.Any())
                    throw new Exception("Não tem dados para consulta");

                foreach (var c in categories) {
                    col = 'A';
                    row++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ c.Name }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ c.Color }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ c.Sole }";
                    col++;

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ c.Description ?? "--" }";
                    col++;

                    foreach (var size in listOfSizes)
                    {
                        ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                        ws.Cells[$"{col}{row}"].Value = $"{ FormatGridItem(c.ShoesBySize, size) }";
                        col++;
                    }

                    ws.Cells[$"{col}{row}"].Style.Numberformat.Format = "@";
                    ws.Cells[$"{col}{row}"].Value = $"{ FormatNumber(c.Shoes.Count()) }";
                    col++;
                }

                ws.Column(01).Width = 20;
                ws.Column(02).Width = 25;
                ws.Column(03).Width = 25;
                ws.Column(04).Width = 25;
                ws.Column(34).Width = 15;

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

        private string FormatNumber(int number)
        {
            if (number <= 0)
                return string.Empty;

            return number.ToString();
        }

        private string FormatGridItem(Dictionary<int, int> groupedInfo, int dic)
        {
            if (!groupedInfo.ContainsKey(dic))
                return string.Empty;

            return groupedInfo[dic].ToString();
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

            var exect = query.OrderBy(c => c.Id).ToPagedList(filters.Page, filters.Rows);

            var idRange = exect.Select(e => e.Id).ToList();

            var shoes = _db.Shoes.Where(s => idRange.Contains(s.Category.Id)).ToList();

            foreach (var category in exect)
            {
                category.Shoes = shoes.Where(s => s.Category.Id == category.Id).ToList();

                category.ShoesBySize = category.Shoes.GroupBy(s => s.Size).ToDictionary(group => group.Key, group => group.Count());
            }

            return exect;
        }
    }
}
