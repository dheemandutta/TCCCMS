using TCCCMS.Models;
using TCCCMS.Business;
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

        [HttpGet]
        public ActionResult Upload()
        {
            UploadDocumentBL uploadBl = new UploadDocumentBL();
            List<FormsCategory> catrgoryList = new List<FormsCategory>();
            catrgoryList = uploadBl.GetCategoryList();
            //List<SelectListItem> catList = new List<SelectListItem>();
            //catList = catrgoryList.Select(c =>
            //                                            new SelectListItem()
            //                                            {
            //                                                Text = c.CatecoryName,
            //                                                Value = c.ID.ToString()
            //                                            }).ToList();

            //ViewBag.FormsCategory = catList.Insert(0,new SelectListItem { Value="0",Text="Select",Selected=true});
            ViewBag.FormsCategory = catrgoryList.Select(c =>
                                                        new SelectListItem()
                                                        {
                                                            Text = c.CatecoryName,
                                                            Value = c.ID.ToString()
                                                        }).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles(FormsCategory category, string categoryId, string categoryName, List<HttpPostedFileBase> fileData)
        {
            List<Forms> formList = new List<Forms>();
            UploadDocumentBL uploadBL = new UploadDocumentBL();
            
            if (Request.Files.Count > 0)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    string fileFath = Path.Combine(path, categoryName);
                    if (!Directory.Exists(fileFath))
                    {
                        Directory.CreateDirectory(fileFath);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        Forms form = new Forms();

                        
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        string fnameWithPath = Path.Combine(fileFath, fname);
                        file.SaveAs(fnameWithPath);

                        form.FormName   = fname;
                        form.FilePath   = fileFath;
                        form.CategoryId = Convert.ToInt32(categoryId);
                        form.CreateedBy = 1;

                        formList.Add(form);

                    }

                   int count = uploadBL.SaveUploadedForms(formList);
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [HttpPost]
        public ActionResult UploadDropFile(List<HttpPostedFileBase> fileData)
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