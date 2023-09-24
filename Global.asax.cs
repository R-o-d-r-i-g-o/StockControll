using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace StockControll
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
                return;

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Session["ReturnUrl"] = _GetIndexRoute();

            // stop request
            HttpContext.Current.Response.End();
        }

        private string _GetIndexRoute()
        {
            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);

            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            RouteData routeData = urlHelper.RouteCollection.GetRouteData(currentContext);

            return routeData.Values["controller"] as string;
        }
    }
}
