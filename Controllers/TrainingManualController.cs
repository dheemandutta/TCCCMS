using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class TrainingManualController : Controller
    {
        //--------------------Vol7-------------------------

        private string controllerName = "TrainingManual";
        ManualBL manualBL = new ManualBL();

        // GET: TrainingManual
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 7);

            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume VII/";
            string filePath = "../ManualsPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        #region All (7+1)(8)


        public ActionResult Manual()
        {
            //---------------Vol. VII Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult StatutoryTraining()
        {
            //---------------Section 1 - Statutory Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "StatutoryTraining");
            TempData["StatutoryTraining"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CDT()
        {
            //---------------Section 2 - Career Development Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CDT");
            TempData["CDT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CadetTraining()
        {
            //---------------Section 3 - Cadet Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CadetTraining");
            TempData["CadetTraining"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CBT()
        {
            //---------------Section 4 - Computer Based Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CBT");
            TempData["CBT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TPSP()
        {
            //---------------Section 5 - Training Plan for Shore Personnel-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TPSP");
            TempData["TPSP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SDT()
        {
            //---------------Section 6 - Shipboard Drill and Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SDT");
            TempData["SDT"] = file.ManualBodyHtml;
            return View(file);
        }

        #region Appendix (2-1)(1)

        public ActionResult STAFCTCCV()
        {
            //---------------CBT - STA Flow Chart TCC Vessels-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "STAFCTCCV");
            TempData["STAFCTCCV"] = file.ManualBodyHtml;
            return View(file);
        }

        //public ActionResult StartingModules()
        //{
        //    //---------------CBT - Starting modules---XLS--------------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "StartingModules");
        //    TempData["StartingModules"] = file.ManualBodyHtml;
        //    return View(file);
        //}


        #endregion

        #endregion
    }
}