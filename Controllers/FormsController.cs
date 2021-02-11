using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult ListOfAdministrationMmanagementForms()
        {
            return View();
        }

        public ActionResult ListOfMaintenanceFroms()
        {
            return View();
        }

        public ActionResult ListOfMmanagementOfShipPersonalForms()
        {
            return View();
        }

        public ActionResult NavigationMmanagementForms()
        {
            return View();
        }
        public ActionResult SafetyMmanagementForms()
        {
            return View();
        }
    }
}