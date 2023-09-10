using System.Data.Entity;
using StockControll.Models;

namespace StockControll.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Shoe> Shoes { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public AppDbContext() : base("name=DefaultConnection") { }
    }
}