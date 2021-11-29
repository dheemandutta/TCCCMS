using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using System.Web.Caching;
using System.Xml;
using System.Text;

namespace TCCCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //static Cache _cache = null;
        //static string _path = null;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //-----------------------------------------------------

            //_cache = Context.Cache;
            //_path = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.Load(_path);
            //_cache.Insert("VolMenuData", xDoc, new CacheDependency(_path),
            //                Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);

            //------------------------------------------------------
            //if (Session["UserName"] != null)
            //{
            //    //Redirect to Welcome Page if Session is not null    
            //    Response.Redirect("~/Home/Login");
            //}
            //else
            //{
            //    //Redirect to Login Page if Session is null & Expires     
            //    Response.Redirect("~/Home/Login");
            //}
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError();
            //Response.Clear();

            //HttpException httpException = ex as HttpException;

            //if (httpException != null)
            //{
            //    string action;

            //    switch (httpException.GetHttpCode())
            //    {
            //        case 404:
            //            // page not found
            //            action = "HttpError404";
            //            break;
            //        case 500:
            //            // server error
            //            action = "HttpError500";
            //            break;
            //        default:
            //            action = "Login";
            //            break;
            //    }

            //    // clear error on server
            //    Server.ClearError();

            //    //Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, ex.Message));

            //    //Response.Redirect(String.Format("~/Home/{0}", action));
            //    Response.Redirect(String.Format("~/Home/Login"));

            //}
        }
    }
}
