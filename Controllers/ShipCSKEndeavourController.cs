using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class ShipCSKEndeavourController : Controller
    {
        //--------------------Ship5-------------------------

        private string controllerName = "ShipCSKEndeavour";
        ShipBL shipBL = new ShipBL();
        // GET: ShipCSKEndeavour
        public ActionResult Index()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");
            file.BodyHtml = manualBL.GenerateBodyContentHtml(xPath, 5);
            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual    ();
            file = shipBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
    }
}