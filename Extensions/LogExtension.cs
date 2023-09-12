using System;
using System.Web;
using StockControll.Context;
using StockControll.Models;
using static StockControll.Commons.Enums;

namespace StockControll.Extensions
{
    public class LogExtension
    {
        // *Obs.: Não usar dentro da controller de login pois os cookies não foram cofigurados ainda.
        private readonly AppDbContext _db;
        private readonly User _loggedUser;

        public LogExtension(AppDbContext _db = null)
        {
            this._db = _db ?? new AppDbContext();
            this._loggedUser = (User)HttpContext.Current.Session["user"];
        }

        public void AddMessage(ActivityType activityType, string message)
        {
            var sessionUser = _db.Users.Find(_loggedUser.Id);
            if (sessionUser == null)
                throw new Exception("Usuário da sessão não encontrado");

            _db.Logs.Add(new Log
            {
                User = sessionUser,
                Message = message,
                CreatedAt = DateTime.Now,
                ActivityType = activityType,
            });
            _db.SaveChanges();
        }
    }
}