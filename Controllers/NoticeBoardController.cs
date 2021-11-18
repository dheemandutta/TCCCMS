using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCCMS.Models;
using TCCCMS.Business;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class NoticeBoardController : Controller
    {
        private string controllerName = "NoticeBoard";
        private string xmlPath = "~/xmlMenu/" + "NOTICEBOARD.xml";
        ManualBL manualBL = new ManualBL();
        // GET: NoticeBoard

        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            ShipManual file = new ShipManual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../NoticeBoardPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }


        [CustomAuthorizationFilter]
        public ActionResult Advisory()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "Advisory");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult Notices()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "Notices");
            return View(file);
        }
        [CustomAuthorizationFilter]
        public ActionResult SafetyCampaigns()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "SafetyCampaigns");
            return View(file);
        }

        [CustomAuthorizationFilter]

        public ActionResult SafetyAlerts()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "SafetyAlerts");
            return View(file);
        }

        [CustomAuthorizationFilter]
        public ActionResult Circulars()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "Circulars");
            return View(file);
        }
    }
}