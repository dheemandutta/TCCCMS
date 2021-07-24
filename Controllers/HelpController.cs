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
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HelpOfficeAdmin()
        {
            return View();
        }

        public ActionResult HelpOfficeUser()
        {
            return View();
        }

        public ActionResult HelpVesselAdmin()
        {
            return View();
        }

        public ActionResult HelpVesselUser()
        {
            return View();
        }
    }
}