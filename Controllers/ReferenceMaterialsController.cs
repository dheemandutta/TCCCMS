using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class ReferenceMaterialsController : Controller
    {
        private string controllerName = "ReferenceMaterials";
        ManualBL manualBL = new ManualBL();
        // GET: ReferenceMaterials
        public ActionResult Index()
        {

            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "REFERENCEMATERIALS.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 0);
            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            ShipManual file = new ShipManual();
            file = manualBL.GetReferenceMaterialManual(controllerName, actionName);
            TempData[actionName] = file.BodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../ReferenceMaterialsPDF/" + relPDFPath + "/";
           // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }


        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/ReferenceMaterialsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                        .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
            //                                || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();

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
       
    }
}