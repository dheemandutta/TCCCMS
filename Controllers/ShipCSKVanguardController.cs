using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class ShipCSKVanguardController : Controller
    {
        //--------------------Ship3-------------------------

        private string controllerName = "ShipCSKVanguard";
        private int shipId = 3;
        private string xmlPath = "~/xmlMenu/" + "ALLSHIPS1.xml";
        ShipBL shipBL = new ShipBL();
        // GET: ShipCSKVanguard
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
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
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
            return File(fileBytes, manualBl.GetMimeTypes()[ext], Path.GetFileName(filePath));
        }

        public ActionResult SOPEP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SOPEP");
            return View(file);
        }
        public ActionResult STS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "STS");
            return View(file);
        }
        public ActionResult BWMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BWMP");
            return View(file);
        }
        public ActionResult VOC()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "VOC");
            return View(file);
        }
        public ActionResult MSMPLMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "MSMPLMP");
            return View(file);
        }
        public ActionResult PRPW()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "PRPW");
            return View(file);
        }
        public ActionResult BMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BMP");
            return View(file);
        }
        public ActionResult CWBMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CWBMP");
            return View(file);
        }
        public ActionResult GMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "GMP");
            return View(file);
        }
        public ActionResult ETA()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "ETA");
            return View(file);
        }
        public ActionResult SEEMP1()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP1");
            return View(file);
        }
        public ActionResult SEEMP2()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP2");
            return View(file);
        }
        public ActionResult CRM()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CRM");
            return View(file);
        }
    }
}