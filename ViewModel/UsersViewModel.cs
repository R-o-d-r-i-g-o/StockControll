using PagedList;
using StockControll.Models;

namespace StockControll.ViewModel
{
    public class UsersViewModel
    {
        public FilterViewModel Filters { get; set; }
        public IPagedList<User> Users { get; set; }
        public User NewUser { get; set; }
    }
}