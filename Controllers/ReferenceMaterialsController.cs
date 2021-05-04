using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class ReferenceMaterialsController : Controller
    {
        private string controllerName = "ReferenceMaterials";
        ManualBL manualBL = new ManualBL();
        // GET: ReferenceMaterials
        public ActionResult Index()
        {

            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "REFERENCEMATERIALS.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 0);
            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual();
            file = manualBL.GetReferenceMaterialManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../ReferenceMaterialsPDF/" + relPDFPath + "/";
           // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }
    }
}