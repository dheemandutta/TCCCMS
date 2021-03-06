﻿using TCCCMS.Models;
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
    public class CompanyProceduresManualController : Controller
    {
        //--------------------Vol2-------------------------

        private string controllerName = "CompanyProceduresManual";
        ManualBL manualBL = new ManualBL();
        // GET: CompanyProceduresManual
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 2);

            return View(file);
        }
        //public ActionResult Pages(string actionName)
        //{
        //    System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, actionName);
        //    TempData[actionName] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult Pages(string actionName, string formName = "", string relformPath = "")
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            Manual file = new Manual();
            if (formName == "")
            {
                file = manualBL.GetManual(controllerName, actionName);
            }
            else
            {
                string filePath = "../ManualsPDF/" + relformPath + "/";
                filePath = filePath + formName + ".pdf#toolbar=0";
                file.PdfName = formName;
                file.PdfPath = filePath;


                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.ManualBodyHtml);

                sb.Append("</div>");
                sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
                sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
                sb.Append("</div>");
                //-------------------------------------------------------------------------
                StringBuilder sb2 = new StringBuilder("<div class='row'>");
                sb2.Append("<div class='col-sm-12 col-xs-12 marginTP10'>");
                sb2.Append("<div class='card'>");
                sb2.Append("<div class='col-sm-12 col-xs-12'>");
                sb2.Append("<div class='row'>");
                //------
                sb2.Append("<p class='marginTP10'>");
                sb2.Append("<div class='col-sm-10 col-xs-12'>");
                sb2.Append("<label>" + formName + "</label>");
                sb2.Append("</div>");

                sb2.Append("<div class='col-sm-2 col-xs-12'>");
                sb2.Append("<button type='button' class='btn btn-info btn-sm' style='background-color: #e90000;' data-toggle='modal' data-target='#formPreviewModal' >Preview</button>");
                //sb2.Append("</div>");

                //sb2.Append("<div class='col-sm-2 col-xs-12'>");
                sb2.Append("<a href='/" + controllerName + "/Download?fileName=");
                sb2.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a>");

                sb2.Append("</div>");
                sb2.Append("</p>");
                //-------
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");


                //file.ManualBodyHtml = sb.ToString();
                file.ManualBodyHtml = sb2.ToString();
            }

            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume III/";
            string filePath = "../ManualsPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/ManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                                            .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
            //                                                    || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();

            var filePath = Directory.GetFiles(path, "*.*")
                                                       .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
                                                               || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();
            //var memory = new MemoryStream();
            //using (var stream = new FileStream(filePath, FileMode.Open))
            //{
            //    stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            //return File(memory, GetMimeTypes()[ext], Path.GetFileName(filePath));

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes,manualBl.GetMimeTypes()[ext], Path.GetFileName(filePath));
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