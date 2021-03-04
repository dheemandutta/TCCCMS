using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class FormsAndChecklistsController : Controller
    {
        //--------------------Vol8-------------------------

        private string controllerName = "FormsAndChecklists";
        ManualBL manualBL = new ManualBL();
        // GET: FormsAndChecklists
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 8);

            return View(file);
        }
    }
}