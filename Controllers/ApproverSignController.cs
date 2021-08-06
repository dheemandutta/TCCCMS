using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

namespace TCCCMS.Controllers
{
    public class ApproverSignController : Controller
    {
        // GET: ApproverSign
        public ActionResult Index()
        {
            if (Session["Role"].ToString() == "OfficeUser" && Session["IsApprover"].ToString() == "1" && Session["IsAllowSign"].ToString() == "0")
            {
               GetAllUserForDrpApproverSign();
                return View();
            }
            else
                return RedirectToAction("Login", "Home");

        }

        [HttpPost]
        public JsonResult SaveApproverSign(ApproverMaster ApproverSign)
        {
            ApproverSignBL bL = new ApproverSignBL();
            ApproverMaster pC = new ApproverMaster();

            pC.Id = ApproverSign.Id;
            pC.ApproverUserId = ApproverSign.ApproverUserId;
            pC.SignImagePath = ApproverSign.SignImagePath;
            pC.Name = ApproverSign.Name;
            pC.Position = ApproverSign.Position;
            //pC.CreatedOn1 = pOCO.CreatedOn1;
            //pC.ModifiedOn1 = pOCO.ModifiedOn1;

            return Json(bL.SaveApproverSign(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSign(string crewId, string crewPosition)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            string fnameWithPath = string.Empty;

            List<string> returnMsg = new List<string>();
            string fileName = String.Empty; //Path.GetFileNameWithoutExtension(postedFile.FileName);
            //fileName = "SignImages" + "_" + crewId;
            fileName = "SignImages" + "_" + Session["UserId"].ToString();   // 31-07-2021

            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {

                HttpPostedFileBase file = files[i];
                string fname;
                string extn;

                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                    extn = Path.GetExtension(fname);
                }
                else
                {
                    fname = file.FileName;
                    extn = Path.GetExtension(file.FileName);
                }
                string path = Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"].ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (System.IO.File.Exists(path + fileName))
                {

                    System.IO.File.Delete(path + fileName);
                }
                //fileName = fileName + ".zip"/*extn*/;
                fileName = fileName + extn;
                // Get the complete folder path and store the file inside it.  
                fnameWithPath = Path.Combine(path, fileName);
                file.SaveAs(fnameWithPath);
                returnMsg.Add(fnameWithPath);
            }

            ApproverSignBL bL = new ApproverSignBL();
            ApproverMaster pC = new ApproverMaster();

            pC.ApproverUserId = Convert.ToInt32(int.Parse(Session["UserId"].ToString()));
            pC.SignImagePath = AesOperation.EncryptString(key, fnameWithPath);
            //pC.Name = name;
            pC.Position = crewPosition;
            //pC.CreatedOn1 = pOCO.CreatedOn1;
            //pC.ModifiedOn1 = pOCO.ModifiedOn1;

            int x = bL.SaveApproverSign(pC);
           
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadData()
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
                length = 500;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            ApproverSignBL bL = new ApproverSignBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<ApproverMaster> pocoList = new List<ApproverMaster>();
            pocoList = bL.GetAllApproverSignPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<ApproverMaster> pList = new List<ApproverMaster>();
            foreach (ApproverMaster pC in pocoList)
            {
                ApproverMaster pOCO = new ApproverMaster();
                //pOCO.Id = pC.Id;
                pOCO.UserName = pC.UserName;
                pOCO.SignImagePath = pC.SignImagePath;
                pOCO.Name = pC.Name;
                pOCO.Position = pC.Position;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetAllApproverSign()
        //{
        //    ApproverSignBL bL = new ApproverSignBL();
        //    ApproverMaster pOCOList = new ApproverMaster();

        //    pOCOList = bL.GetAllApproverSign();

        //    ApproverMaster dept = new ApproverMaster();

        //    dept.Id = pOCOList.Id;
        //    dept.ApproverUserId = pOCOList.ApproverUserId;
        //    dept.SignImagePath = pOCOList.SignImagePath;
        //    dept.Name = pOCOList.Name;
        //    dept.Position = pOCOList.Position;
        //    dept.CreatedOn1 = pOCOList.CreatedOn1;
        //    dept.ModifiedOn1 = pOCOList.ModifiedOn1;

        //    var data = dept;

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //for User drp
        public void GetAllUserForDrpApproverSign()
        {
            ApproverSignBL bL = new ApproverSignBL();
            List<ApproverMaster> pocoList = new List<ApproverMaster>();

            pocoList = bL.GetAllUserForDrpApproverSign(/*int.Parse(Session["VesselID"].ToString())*/);


            List<ApproverMaster> itmasterList = new List<ApproverMaster>();

            foreach (ApproverMaster up in pocoList)
            {
                ApproverMaster unt = new ApproverMaster();
                unt.UserId = up.UserId;
                unt.UserName = up.UserName;

                itmasterList.Add(unt);
            }

            ViewBag.Users = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.UserName,
                                                Value = x.UserId.ToString()
                                            });

        }
    }
}