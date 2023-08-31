using System;
using System.Web.Mvc;
using System.Web.Security;
using StockControll.ViewModel;

namespace StockControll.Controllers
{
    public class LoginController : Controller
    {
        
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
                FormsAuthentication.SetAuthCookie(loginForm.Name, false);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //view

                return View("Index");
            }
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void ResetCookies()
        {
            Session["masterUser"] = null;
            Session["financialUser"] = null;
            Session["franchiseTaxes"] = null;
            Session["franchiseId"] = null;
            Session["franchiseType"] = null;
            Session["franchiseGateways"] = null;
            Session["timeout"] = null;
            Session["ecId"] = null;
            Session.Clear();
        }

    }
}