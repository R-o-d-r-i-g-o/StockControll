using PagedList;
using StockControll.Models;

namespace StockControll.ViewModel
{
    public class LogViewModel
    {
        public FilterViewModel Filters { get; set; }
        public IPagedList<Log> Logs { get; set; }
    }
}