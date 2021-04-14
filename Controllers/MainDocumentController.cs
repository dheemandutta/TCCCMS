using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class MainDocumentController : Controller
    {
        
        //--------------------Vol 0-------------------------

        private string controllerName = "MainDocument";
        ManualBL manualBL = new ManualBL();
        // GET: MainDocument
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 0);

            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }

        //public ActionResult PDFViewer(string fileName, string relPDFPath)
        //{
        //    Manual file = new Manual();
        //    //string filePath = "../ManualsPDF/Volume I/";
        //    string filePath = "../ManualsPDF/" + relPDFPath + "/";
        //    filePath = filePath + fileName + ".pdf";
        //    file.PdfName = fileName;
        //    file.PdfPath = filePath;
        //    return View(file);
        //}
    }
}