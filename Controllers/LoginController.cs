﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StockControll.Context;
using StockControll.ViewModel;
using StockControll.Commons;

namespace StockControll.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(UserViewModel loginForm)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(@"Preencha o formulário corretamente");
               
                var parsedPassword = AuthSettings.CalculateMD5(loginForm.Password);

                var user = _db.Users.FirstOrDefault(u => u.Name == loginForm.Name && u.Password == parsedPassword);
                if (user == null)
                    throw new Exception(@"Usuário não encontrado");

                ResetCookies();
                FormsAuthentication.SetAuthCookie(loginForm.Name, false);

                Session["user"] = user;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = (ex.InnerException ?? ex).Message;

                return View("Index");
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            ResetCookies();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }

        private void ResetCookies()
        {
            Session["user"] = null;
            Session["timeout"] = null;
            Session.Clear();
        }
    }
}