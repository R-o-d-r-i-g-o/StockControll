using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockControll.ViewModel
{
    public class FilterViewModel
    {
        [Display(Name = "Linhas por página")]
        public int Rows { get; set; }

        [Display(Name = "Número da página")]
        public int Page { get; set; }
        public string SearchItem { get; set; }

        [Display(Name = "Procurar por usuário")]
        public string SearchUser { get; set; }

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
            if (this.Rows == default)
                this.Rows = 10;

            if (this.Page == default)
                this.Page = 1;
        }
    }
}