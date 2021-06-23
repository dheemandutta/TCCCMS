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
    public class MajorCasualtyAndEnvironmentalProtectionPlanController : Controller
    {
        //--------------------Vol6-------------------------

        private string controllerName = "MajorCasualtyAndEnvironmentalProtectionPlan";
        ManualBL manualBL = new ManualBL();

        // GET: MajorCasualtyAndEnvironmentalProtectionPlan
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 6);

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
            //file = manualBL.GetManual(controllerName, actionName);
            //if (formName != "")
            //{
            //    StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
            //    sb.Append(file.ManualBodyHtml);

            //    sb.Append("</div>");
            //    sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
            //    sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
            //    sb.Append("</div>");

            //    file.ManualBodyHtml = sb.ToString();
            //}
            //--------------------------------------------------------------------------
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
            return File(fileBytes, manualBl.GetMimeTypes()[ext], Path.GetFileName(filePath));
        }
        #region All (5+6+7)(18)

        public ActionResult Manual()
        {
            //---------------Vol. VI Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SEPGG()
        {
            //---------------Section 1 - Shipboard Emergency Plan – General Guidance-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEPGG");
            TempData["SEPGG"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SEPACS()
        {
            //---------------Section 3 - Shipboard Emergency Plan – Action Checklist for Security-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEPACS");
            TempData["SEPACS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SEPM()
        {
            //---------------Section 4 - Shipboard Emergency Plan - Medical-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEPM");
            TempData["SEPM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CCPTHRA()
        {
            //---------------Section 5 - Company Contingency Plan for Transiting HRA-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CCPTHRA");
            TempData["CCPTHRA"] = file.ManualBodyHtml;
            return View(file);
        }

        #region Section 2 - Shipboard Emergency Plan – Action Checklist (9-3)(6)

        public ActionResult CVA()
        {
            //---------------Annex A - Checklist for Vessel Action-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CVA");
            TempData["CVA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CSD()
        {
            //---------------Annex B - Checklist for Submission of Document -----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CSD");
            TempData["CSD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IMORF()
        {
            //---------------Annex C - IMO Reporting Format  -----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IMORF");
            TempData["IMORF"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ESDC()
        {
            //---------------Annex D - Emergency Squad Duty Card-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ESDC");
            TempData["ESDC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult BSDC()
        {
            //---------------Annex E - Back-up Squad Duty Card-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BSDC");
            TempData["BSDC"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult MPACRF()
        //{
        //    //---------------Annex F - MPA Casualty Reporting Form------PDF-----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "MPACRF");
        //    TempData["MPACRF"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        //public ActionResult AMSA18()
        //{
        //    //---------------Annex G - AMSA18----------PDF-------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "AMSA18");
        //    TempData["AMSA18"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        //public ActionResult AMSA19()
        //{
        //    //---------------Annex G - AMSA19-------PDF----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "AMSA19");
        //    TempData["AMSA19"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult Section2()
        {
            //---------------Volume VI - Section 2-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Section2");
            TempData["Section2"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #region Section 6 - Company Contingency Plan (9-2)(7)
        public ActionResult ERCO()
        {
            //---------------Annex A - Emergency Reponse checklist office-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ERCO");
            TempData["ERCO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RecordOfEvent()
        {
            //---------------Annex B - Record of Event -----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RecordOfEvent");
            TempData["RecordOfEvent"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CEL()
        {
            //---------------Annex C - Casualty Event Log-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CEL");
            TempData["CEL"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CMT()
        {
            //---------------Annex D - Crisis Management Team-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CMT");
            TempData["CMT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ERCN()
        {
            //---------------Annex E - Emergency Response Contact Numbers----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ERCN");
            TempData["ERCN"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult SCL()
        //{
        //    //---------------Annex F - Ship Communication List-----XLS------------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "SCL");
        //    TempData["SCL"] = file.ManualBodyHtml;
        //    return View(file);

        //}
        //public ActionResult LOFR1()
        //{
        //    //---------------Annex G - LOF Rev 01-------PDF---------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "LOFR1");
        //    TempData["LOFR1"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult VCPR()
        {
            //---------------Annex H - Vessel Casualty Prompt Report-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "VCPR");
            TempData["VCPR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Section6()
        {
            //---------------Volume VI - Section 6-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Section6");
            TempData["Section6"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #endregion

    }
}