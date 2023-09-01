using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockControll.ViewModel
{
    public class FilterViewModel
    {
        public int Rows { get; set; }
        public int Page { get; set; }
        public string SearchItem { get; set; }

        public void VerifyFilter()
        {
            if (this.Rows == default)
                this.Rows = 10;

            if (this.Page == default)
                this.Page = 1;
        }
    }
}