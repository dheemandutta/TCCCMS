using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCCMS.Models;
using TCCCMS.Business;

namespace TCCCMS.Controllers
{
    public class NoticeBoardController : Controller
    {
        private string controllerName = "NoticeBoard";
        private string xmlPath = "~/xmlMenu/" + "NOTICEBOARD.xml";
        ManualBL manualBL = new ManualBL();
        // GET: NoticeBoard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            ShipManual file = new ShipManual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../NoticeBoardPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf#toolbar=0";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        public ActionResult Advisory()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "Advisory");
            return View(file);
        }
        public ActionResult Notices()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "Notices");
            return View(file);
        }
        public ActionResult SafetyCampaigns()
        {
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateNoticeBoardFolderBodyContentHtml(xPath, "SafetyCampaigns");
            return View(file);
        }
    }
}