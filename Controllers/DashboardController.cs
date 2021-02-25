using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult UserDashboard()
        {
            return View();
            //return RedirectToAction("UserDashboard", "Dashboard");
        }

        // GET: Dashboard
        public ActionResult AdminDashboard()
        {
            return View();
        }
    }
}