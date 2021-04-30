using TCCCMS.Models;
using TCCCMS.Business;

using System;
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
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");
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
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        public ActionResult SOPEP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SOPEP");
            return View(file);
        }
        public ActionResult STS()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "STS");
            return View(file);
        }
        public ActionResult BWMP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BWMP");
            return View(file);
        }
        public ActionResult VOC()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "VOC");
            return View(file);
        }
        public ActionResult MSMPLMP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "MSMPLMP");
            return View(file);
        }
        public ActionResult PRPW()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "PRPW");
            return View(file);
        }
        public ActionResult BMP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BMP");
            return View(file);
        }
        public ActionResult CWBMP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CWBMP");
            return View(file);
        }
        public ActionResult GMP()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "GMP");
            return View(file);
        }
        public ActionResult ETA()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "ETA");
            return View(file);
        }
        public ActionResult SEEMP1()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP1");
            return View(file);
        }
        public ActionResult SEEMP2()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP2");
            return View(file);
        }
    }
}