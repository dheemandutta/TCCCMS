using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class CommonToAllManualController : Controller
    {
        private string controllerName = "CommonToAllManual";
        ManualBL manualBL = new ManualBL();
        // GET: CommonToAllManual
        public ActionResult Index()
        {
            ManualBL manualBL = new ManualBL();
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "COMMONTOALL.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 0);
            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual();
            file = manualBL.GetCommonToAllManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../CommonToAllManualsPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }
    }
}