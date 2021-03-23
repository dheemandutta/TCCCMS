using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class CompanyProceduresManualController : Controller
    {
        //--------------------Vol2-------------------------

        private string controllerName = "CompanyProceduresManual";
        ManualBL manualBL = new ManualBL();
        // GET: CompanyProceduresManual
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 2);

            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName)
        {
            Manual file = new Manual();
            string filePath = "../ManualsPDF/Volume II/";
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        [HttpGet]
        public ActionResult Manual()
        {
            //---------------Vol. II Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ProceduresExplained()
        {
            //---------------PP01.2 - Procedures Explained-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ProceduresExplained");
            TempData["ProceduresExplained"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SEP()
        {
            //---------------PP02.0 - Safety Environmental Policy-----------------
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEP");
            TempData["SEP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MRA()
        {
            //----------------PP03.0 - Management Responsibility and Authority-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MRA");
            TempData["MRA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DesignatedPerson()
        {
            //----------------PP04.0 - Designated Person-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DesignatedPerson");
            TempData["DesignatedPerson"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MastersRA()
        {
            //----------------PP05.0 - Masters Responsibility and Authority.-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MastersRA");
            TempData["MastersRA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HRT()
        {
            //----------------'PP06.0 - Human Resource and Training-----------------
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HRT");
            TempData["HRT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PSO()
        {
            //----------------PP07.0 - Plans for Shipboard Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PSO");
            TempData["PSO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CCP()
        {
            //----------------PP08.0 - Company Contingency Plan-----------------
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CCP");
            TempData["CCP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult NCCA()
        {
            //----------------PP09.0 - Non Conformity and Corrective Action-----------------
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NCCA");
            TempData["NCCA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MIT()
        {
            //------------PP10.0 - Maintenance Inspection and Testing
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MIT");
            TempData["MIT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DDC()
        {
            //------PP11.0 - Documents & Data Control

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DDC");
            TempData["DDC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ISEPA()
        {
            //-----PP12.0 - Internal Safety and Environmental Protection Audits

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ISEPA");
            TempData["ISEPA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DAP()
        {
            //------PP20.0 - Drug & Alcohol Policy

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DAP");
            TempData["DAP"] = file.ManualBodyHtml;
            return View(file);
        }
    }
}