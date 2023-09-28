using PagedList;
using StockControll.Models;

namespace StockControll.ViewModel
{
    public class CategoryViewModel
    {
        public FilterViewModel Filters { get; set; }
        public IPagedList<Category> Categories { get; set; }
        public Category NewCategory { get; set; }
        public Shoe newShoe { get; set; }
    }
}
