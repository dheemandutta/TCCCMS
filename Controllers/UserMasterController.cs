using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace TCCCMS.Controllers
{
    public class UserMasterController : Controller
    {
        // GET: UserMaster
        public ActionResult Index()
        {
            GetAllRanksForDrp();
            return View();
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

            UserMasterBL bL = new UserMasterBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<UserMasterPOCO> pocoList = new List<UserMasterPOCO>();
            pocoList = bL.GetAllUserPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<UserMasterPOCO> pList = new List<UserMasterPOCO>();
            foreach (UserMasterPOCO pC in pocoList)
            {
                UserMasterPOCO pOCO = new UserMasterPOCO();
                pOCO.UserName = pC.UserName;
                pOCO.CreatedOn = pC.CreatedOn;
                pOCO.Email = pC.Email;
                pOCO.CreatedBy = pC.CreatedBy;
                pOCO.ModifiedBy = pC.ModifiedBy;
                pOCO.Gender = pC.Gender;
                pOCO.VesselIMO = pC.VesselIMO;
                pOCO.RankName = pC.RankName;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUpdateUser(UserMasterPOCO pOCO)
        {
            UserMasterBL bL = new UserMasterBL();
            UserMasterPOCO pC = new UserMasterPOCO();

            pC.UserId = pOCO.UserId;

            pC.RankId = pOCO.RankId;
            pC.UserName = pOCO.UserName;
            pC.Password = pOCO.Password;
            pC.Email = pOCO.Email;
            pC.CreatedBy = pOCO.CreatedBy;
            pC.ModifiedBy = pOCO.ModifiedBy;
            pC.Gender = pOCO.Gender;
            pC.VesselIMO = pOCO.VesselIMO;

            return Json(bL.SaveUpdateUser(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }





        //for Ranks drp
        public void GetAllRanksForDrp()
        {
            UserMasterBL bL = new UserMasterBL();
            List<UserMasterPOCO> pocoList = new List<UserMasterPOCO>();

            pocoList = bL.GetAllRanksForDrp(/*int.Parse(Session["VesselID"].ToString())*/);


            List<UserMasterPOCO> itmasterList = new List<UserMasterPOCO>();

            foreach (UserMasterPOCO up in pocoList)
            {
                UserMasterPOCO unt = new UserMasterPOCO();
                unt.RankId = up.RankId;
                unt.RankName = up.RankName;

                itmasterList.Add(unt);
            }

            ViewBag.Ranks = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.RankName,
                                                Value = x.RankId.ToString()
                                            });

        }

    }
}