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
            //------------------------------------------------------

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
            string path = Server.MapPath("~/ManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            var filePath = Directory.GetFiles(path, "*.doc?")
                                                        .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")).First();
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(filePath));
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                //{".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
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