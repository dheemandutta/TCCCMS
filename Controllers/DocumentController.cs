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
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            DocumentBL uploadBl = new DocumentBL();
            List<FormsCategory> catrgoryList = new List<FormsCategory>();
            catrgoryList = uploadBl.GetCategoryList();
            ViewBag.FormsCategory = catrgoryList.Select(c =>
                                                        new SelectListItem()
                                                        {
                                                            Text  = c.CatecoryName,
                                                            Value = c.ID.ToString()
                                                        }).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            DocumentBL uploadBl = new DocumentBL();
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
        public ActionResult UpdateForm()
        {
            DocumentBL uploadBl = new DocumentBL();
            List<FormsCategory> catrgoryList = new List<FormsCategory>();
            catrgoryList = uploadBl.GetCategoryList();
            ViewBag.FormsCategory = catrgoryList.Select(c =>
                                                        new SelectListItem()
                                                        {
                                                            Text = c.CatecoryName,
                                                            Value = c.ID.ToString()
                                                        }).ToList();
            return View();
        }

        #region Save/Load/Delete Methods
        [HttpPost]
        public JsonResult SaveFiles()
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadData(string catId)
        {
            int categoryId = Convert.ToInt32(catId);
            int draw, start, length;
            //int pageIndex = 0;

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
                length = 500;
            }

            //if (start == 0)
            //{
            //    pageIndex = 1;
            //}
            //else
            //{
            //    pageIndex = (start / length) + 1;
            //}

            DocumentBL bL = new DocumentBL(); ///////////////////////////////////////////////////////////////////////////
            //int totalrecords = 0;

            List<Forms> formsList = new List<Forms>();
            formsList = bL.GetFormsListCategoryWise(categoryId);
            List<Forms> formList = new List<Forms>();
            foreach (Forms frm in formsList)
            {
                Forms form = new Forms();
                form.ID         = frm.ID;
                form.RowNumber = frm.RowNumber;
                form.FormName   = frm.FormName;

                formList.Add(form);
            }

            var data = formList;
            return Json(new { draw = draw,data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string formName,string catName)
        {
            DocumentBL documentBl = new DocumentBL();
            int recordaffected = documentBl.DeleteForm(formName);
            #region for Form Archive
            if (recordaffected == 1)
            {
                string archivedFolder = "~/ArchivedForms/" + catName + "/";
                string uploadedFolder = "~/Uploads/" + catName +"/";
                string sourcePath = Server.MapPath(uploadedFolder);
                string destinationPath = Server.MapPath(archivedFolder);
                string uploadedFile = Path.Combine(sourcePath , formName);
                string archiveFile = Path.Combine(destinationPath, formName);
                if (System.IO.File.Exists(uploadedFile))
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    System.IO.File.Move(uploadedFile, archiveFile);
                }

            }
            #endregion
            return Json(recordaffected, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Download/Upload
        [HttpPost]
        public ActionResult UploadFiles(FormsCategory category, string categoryId, string categoryName, List<HttpPostedFileBase> fileData)
        {
            List<Forms> formList = new List<Forms>();
            DocumentBL uploadBL = new DocumentBL();

            if (Request.Files.Count > 0)
            {
                try
                {
                    string relativePath = "~/Uploads/" + categoryName;
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

                        form.FormName = fname;
                        form.FilePath = fileFath;
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

        [HttpGet]
        public  FileResult Download(string catName, string formName)
        {
            string path = Server.MapPath("~/Uploads/");
            var folderPath = Path.Combine(path, catName);
            var filePath = Path.Combine(folderPath, formName);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                 stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(filePath));
        }

        public JsonResult UploadAndUpdateForm(string categoryId, string categoryName,string formName, string formVersion)
        {
            List<Forms> formList = new List<Forms>();
            DocumentBL uploadBL = new DocumentBL();

            if (Request.Files.Count == 1)
            {
                try
                {
                    string tempFolder = "~/UploadForUpdate/";
                    string relativePath = "~/Forms/" + categoryName;  // deep
                    string path = Server.MapPath("~/Forms/");         // deep
                    //string tempath = Server.MapPath(tempFolder);
                    string fileFath = Path.Combine(path, categoryName);
                    //if (!Directory.Exists(tempath))
                    //{
                    //    Directory.CreateDirectory(fileFath);
                    //}
                    //  Get all files from Request object  

                    #region form to Archive
                    string archivedFolder       = "~/ArchivedForms/" + categoryName + "/";
                    string uploadedFolder       = "~/Forms/" + categoryName + "/";          // deep
                    string sourcePath           = Server.MapPath(uploadedFolder);
                    string destinationPath      = Server.MapPath(archivedFolder);
                    string uploadedFile         = Path.Combine(sourcePath, formName);
                    string archiveFile          = Path.Combine(destinationPath, formName);
                    if (System.IO.File.Exists(uploadedFile))
                    {
                        if (!Directory.Exists(destinationPath))
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                        //System.IO.File.Move(uploadedFile, archiveFile);
                        System.IO.File.Copy(uploadedFile, archiveFile,true);
                    }
                    #endregion

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        Forms form = new Forms();


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

                        //form.FormName   = fname;
                        form.FormName   = formName;
                        form.FilePath   = fileFath;
                        form.CategoryId = Convert.ToInt32(categoryId);
                        form.Version    = formVersion;
                        form.CreateedBy = 1;

                        formList.Add(form);

                    }

                    int count = uploadBL.SaveUploadedForms(formList);
                    // Returns message that successfully uploaded  
                    return Json("File Updated Successfully!");
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

        #endregion

        #region Dropdown
        [HttpPost]
        public JsonResult GetFormsByCategoryForDropdown(string categoryId)
        {
            int catId = Convert.ToInt32(categoryId);
            DocumentBL documentBl = new DocumentBL();
            List<Forms> formsList = new List<Forms>();

            formsList = documentBl.GetFormsByCategoryForDropdown(catId);


            var data = formsList;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}