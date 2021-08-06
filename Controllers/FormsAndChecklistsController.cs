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
    public class FormsAndChecklistsController : Controller
    {
        //--------------------Vol8-------------------------

        private string controllerName = "FormsAndChecklists";
        string parentPdfPath = "/FormsAndChecklists/";
        ManualBL manualBL = new ManualBL();
        // GET: FormsAndChecklists
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 8);

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

            //----------------------------------------------------
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
            string filePath = "";
            if (fileName == "")
            {
                filePath = Path.ChangeExtension(relPDFPath, "pdf");
                file.PdfName = Path.GetFileName(filePath);
                file.PdfPath = filePath;
            }
            else
            {
                filePath = "../ManualsPDF/" + relPDFPath + "/";
                // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
                filePath = filePath + fileName + ".pdf#zoom=137";
                file.PdfName = fileName;
                file.PdfPath = filePath;
            }
            //string filePath = "../ManualsPDF/Volume III/";
           
           
            return View(file);
        }

        public JsonResult PreviewModal(string relPDFPath)
        {
            Manual file = new Manual();
            string filePath = "";
            filePath = Path.ChangeExtension(relPDFPath, "pdf");
            file.PdfName = Path.GetFileName(filePath);
            file.PdfPath = parentPdfPath + filePath;
            return Json(file, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/ManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                                            .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")).First();

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
        /// <summary>
        /// added on 3rd Jul 2021 @BK
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileResult DownloadFilledupForm(string fileName)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/UploadFilledUpFormForApproval/");

            //var filePath = Directory.GetFiles(path, "*.*")
            //                        .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
            //                                || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();

            var filePath = Directory.GetFiles(path, "*.*")
                                    .Where(s => s.Contains(fileName)).First();

            var ext = Path.GetExtension(filePath).ToLowerInvariant();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, manualBl.GetMimeTypes()[ext], Path.GetFileName(filePath));
        }

        public ActionResult AdministrationAndMmgt()
        {
            return View();
        }

        public ActionResult ManagementOfShipPersonnel()
        {
            return View();
        }

        public ActionResult Maintenance()
        {
            return View();
        }

        public ActionResult NavigationManagement()
        {
            return View();
        }

        public ActionResult CargoOperation()
        {
            return View();
        }

        public ActionResult SafetyMmgt()
        {
            return View();
        }

        public ActionResult FilingSystemAndRetentionMatrix()
        {
            return View();
        }






        // under Section 5 - CargoOperation
        public ActionResult CO031()
        {
            return View();
        }





        public JsonResult LoadData(int CategoryId)
        {
            int draw, start, length;
            int pageIndex = 0;

            if (null != Request.Form.GetValues("draw"))
            {
                draw = int.Parse(Request.Form.GetValues("draw").FirstOrDefault().ToString());
                start = int.Parse(Request.Form.GetValues("start").FirstOrDefault().ToString());
                length = int.Parse(Request.Form.GetValues("length").FirstOrDefault().ToString());
            }
            else
            {
                draw = 1;
                start = 0;
                length = 50000;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            DownloadableFromsBL bL = new DownloadableFromsBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<DownloadableFroms> pocoList = new List<DownloadableFroms>();
            pocoList = bL.GetDownloadableFromsPageWise(pageIndex, ref totalrecords, length, CategoryId);
            List<DownloadableFroms> pList = new List<DownloadableFroms>();
            foreach (DownloadableFroms pC in pocoList)
            {
                DownloadableFroms pOCO = new DownloadableFroms();
                pOCO.ID         = pC.ID;
                pOCO.CategoryId = pC.CategoryId;// Added on 5th Aug 2021 @BK
                pOCO.FormName   = pC.FormName;
                pOCO.Path       = pC.Path;
                pOCO.IsUpload   = pC.IsUpload;
                pOCO.Version    = pC.Version;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Deck()
        {
            return View();
        }
        public ActionResult Engine()
        {
            return View();
        }
    }
}