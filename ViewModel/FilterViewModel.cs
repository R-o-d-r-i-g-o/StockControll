using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StockControll.Commons;

namespace StockControll.ViewModel
{
    public class FilterViewModel
    {
        [Display(Name = "Linhas por página")]
        public int Rows { get; set; }

        [Display(Name = "Número da página")]
        public int Page { get; set; }
        public string SearchItem { get; set; }

        [Display(Name = "Usuário")]
        public string SearchUser { get; set; }

        [Display(Name = "Cor do conjunto")]
        public string SearchColor { get; set; }

        [Display(Name = "Tipo de solado")]
        public string SearchSole { get; set; }

        [Display(Name = "Nome do modelo")]
        public string SearchCategoryName { get; set; }

        [Display(Name = "Data inícial")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data final")]
        public DateTime EndDate { get; set; }

        public List<SelectListItem> GetPaginationOptions()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "30", Value = "30" },
                new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "100", Value = "100" }
            };
        }

        public void VerifyFilter()
        {
            var curDate = DateTime.Now;

            if (this.Rows == default)
                this.Rows = 10;

            if (this.Page == default)
                this.Page = 1;

            if (this.EndDate == default)
                this.EndDate = curDate.EndDateOfCurrentStamp();

            if (this.StartDate == default)
                this.StartDate = curDate.StartDateOfTheMonth();
        }
    }
}