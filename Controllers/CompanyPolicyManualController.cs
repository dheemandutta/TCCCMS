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
    public class CompanyPolicyManualController : Controller
    {
        //--------------------Vol1-------------------------

        private string controllerName = "CompanyPolicyManual";
        ManualBL manualBL = new ManualBL();
        // GET: CompanyPolicyManual
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 1);

            return View(file);
        }
        //public ActionResult Pages(string actionName)
        //{
        //    System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, actionName);
            
        //    //TempData[actionName] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult Pages(string actionName, string formName = "", string relformPath= "")
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, actionName);
            if(formName != "")
            {
                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.ManualBodyHtml);

                sb.Append("</div>");
                sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
                sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
                sb.Append("</div>");

                file.ManualBodyHtml = sb.ToString();
            }
            
            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName,string relPDFPath)
        {
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../ManualsPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }
        public FileResult Download(string fileName, string relformPath)
        {
            string path = Server.MapPath("~/View/" + controllerName + "/");
            var folderPath = Path.Combine(path, relformPath);
            var filePath = Path.Combine(folderPath, fileName);
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

        #region Others Actions No used
        public ActionResult Manual()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName,"Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Introduction()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Introduction");
            TempData["Introduction"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Definition()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Definition");
            TempData["Definition"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SMS()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SMS");
            TempData["SMS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SFA()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFA");
            TempData["SFA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SEPP()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEPP");
            TempData["SEPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MRA()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MRA");
            TempData["MRA"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult DesignatedPerson()
        //{
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "DesignatedPerson");
        //    TempData["DesignatedPerson"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        //public ActionResult MastersRA()
        //{
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "MastersRA");
        //    TempData["MastersRA"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult ResourcesAndPersonnel()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ResourcesAndPersonnel");
            TempData["ResourcesAndPersonnel"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PSO()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PSO");
            TempData["PSO"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult EmergencyPreparednes()
        //{
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "EmergencyPreparednes");
        //    TempData["EmergencyPreparednes"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult CNCCA()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CNCCA");
            TempData["CNCCA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MSE()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MSE");
            TempData["MSE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DDC()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DDC");
            TempData["DDC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ISEPA()
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ISEPA");
            TempData["ISEPA"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult DAP()
        //{
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "DAP");
        //    TempData["DAP"] = file.ManualBodyHtml;
        //    return View(file);
        //}

        #endregion
    }
}