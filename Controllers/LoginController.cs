using System;
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
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();

            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(UserViewModel loginForm)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Preencha o formulário corretamente");
               
                var parsedPassword = AuthSettings.CalculateMD5(loginForm.Password);

                var lastPerformed = Session["ReturnUrl"] as string;

                var user = _db.Users.FirstOrDefault(u => u.Name == loginForm.Name && u.Password == parsedPassword && !u.DeletedAt.HasValue);
                if (user == null)
                    throw new Exception("Usuário não encontrado");

                ResetCookies();
                FormsAuthentication.SetAuthCookie(loginForm.Name, false);

                Session["user"] = user;

                return RedirectToAction(
                    actionName: "Index",
                    controllerName: lastPerformed ?? "Home"
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = (ex.InnerException ?? ex).Message;

                return Index();
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