using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using System.Text;
using TCCCMS.Infrastructure;
using System.Web.Mvc.Html;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class HelpController : Controller
    {
        // GET: Help

        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorizationFilter]
        public ActionResult HelpOfficeAdmin()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult HelpOfficeUser()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult HelpVesselAdmin()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult HelpVesselUser()
        {
            return View();
        }
    }
}