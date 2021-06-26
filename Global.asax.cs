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
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
        }
    }
}
