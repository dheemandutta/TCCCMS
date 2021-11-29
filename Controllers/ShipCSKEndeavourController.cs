using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class ShipCSKEndeavourController : Controller
    {
        //--------------------Ship5-------------------------

        private string controllerName = "ShipCSKEndeavour";
        private int shipId = 5;
        private string xmlPath = "~/xmlMenu/" + "ALLSHIPS1.xml";
        ShipBL shipBL = new ShipBL();
        // GET: ShipCSKEndeavour
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            //string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateBodyContentHtml(xPath, shipId);
            return View(file);
        }
        //public ActionResult Pages(string actionName)
        //{
        //    ShipManual file = new ShipManual();
        //    file = shipBL.GetManual(controllerName, actionName);
        //    TempData[actionName] = file.BodyHtml;
        //    return View(file);
        //}

        [CustomAuthorizationFilter]
        public ActionResult Pages(string actionName, string formName = "", string relformPath = "")
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            ShipManual file = new ShipManual();
            if (formName == "")
            {
                file = shipBL.GetManual(controllerName, actionName);
            }
            else
            {
                string filePath = "../ShipManualsPDF/" + relformPath + "/";
                filePath = filePath + formName + ".pdf#toolbar=0";
                file.PdfName = formName;
                file.PdfPath = filePath;


                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.BodyHtml);

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
                file.BodyHtml = sb2.ToString();
            }

            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }

        [CustomAuthorizationFilter]
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            ShipManual file = new ShipManual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../ShipManualsPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        [CustomAuthorizationFilter]
        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/ShipManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                                            .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx") || s.Contains(fileName + ".xls")).First();
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
        [CustomAuthorizationFilter]
        public ActionResult SOPEP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SOPEP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult STS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "STS");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult BWMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BWMP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult VOC()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "VOC");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult MSMPLMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "MSMPLMP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult PRPW()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "PRPW");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult BMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BMP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult CWBMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CWBMP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult GMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "GMP");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult ETA()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "ETA");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult SEEMP1()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP1");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult SEEMP2()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP2");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult CRM()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CRM");
            return View(file);
        }

        [CustomAuthorizationFilter]
        public ActionResult VGP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "VGP");
            return View(file);
        }
    }
}