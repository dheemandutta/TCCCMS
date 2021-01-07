using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace TCCCMS.Controllers
{
    public class UploadDocumentController : Controller
    {
        // GET: UploadDocument
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(List<HttpPostedFileBase> fileData)
        {
            string path = Server.MapPath("~/Uploads/");
            foreach (HttpPostedFileBase postedFile in fileData)
            {
                if (postedFile != null)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + fileName);
                }
            }

            return Content("Success");
        }

        [HttpPost]
        public JsonResult SaveFiles()
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}