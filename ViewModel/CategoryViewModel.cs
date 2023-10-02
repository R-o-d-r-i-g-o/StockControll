using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using StockControll.Commons;
using StockControll.Models;

namespace StockControll.ViewModel
{
    public class CategoryViewModel
    {
        public FilterViewModel Filters { get; set; }
        public IPagedList<Category> Categories { get; set; }
        public Category NewCategory { get; set; }
        public Shoe newShoe { get; set; }

        public List<SelectListItem> GetPossibleFootSizes()
        {
            var footSizes = new List<SelectListItem>();

            foreach (int size in Constants.GetPossibleFootSizes())
            {
                footSizes.Add(new SelectListItem
                {
                    Text = size.ToString(),
                    Value = size.ToString()
                });
            }

            return footSizes;
        }
    }
}
