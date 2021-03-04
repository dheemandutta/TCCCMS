using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class ShipKHKMajestyController : Controller
    {
        //--------------------Ship7-------------------------

        private string controllerName = "ShipKHKMajesty";
        ShipBL shipBL = new ShipBL();
        // GET: ShipKHKMajesty
        public ActionResult Index()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");
            file.BodyHtml = manualBL.GenerateBodyContentHtml(xPath, 7);
            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
    }
}