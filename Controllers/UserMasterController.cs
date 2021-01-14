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
            GetAllShipForDrp();
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
                pOCO.UserId = pC.UserId;
                pOCO.UserName = pC.UserName;
                pOCO.CreatedOn1 = pC.CreatedOn1;
                pOCO.Email = pC.Email;
                //pOCO.CreatedBy = pC.CreatedBy;
                //pOCO.ModifiedBy = pC.ModifiedBy;
                pOCO.Gender = pC.Gender;
                pOCO.VesselIMO = pC.VesselIMO;
                pOCO.RankName = pC.RankName;
                pOCO.ShipName = pC.ShipName;

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
            pC.ShipId = pOCO.ShipId;
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



        public JsonResult GetUserByUserId(int UserId)
        {
            UserMasterBL bL = new UserMasterBL();
            UserMasterPOCO pOCOList = new UserMasterPOCO();

            pOCOList = bL.GetUserByUserId(UserId);

            UserMasterPOCO dept = new UserMasterPOCO();

            dept.UserId = pOCOList.UserId;
            dept.UserName = pOCOList.UserName;
            dept.Password = pOCOList.Password;
            dept.CreatedOn = pOCOList.CreatedOn;
            dept.Email = pOCOList.Email;
            //dept.CreatedBy = pOCOList.CreatedBy;
            //dept.ModifiedBy = pOCOList.ModifiedBy;
            dept.Gender = pOCOList.Gender;
            dept.VesselIMO = pOCOList.VesselIMO;
            dept.RankId = pOCOList.RankId;
            dept.ShipId = pOCOList.ShipId;

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeleteUserMaster(int UserId/*, ref string recordCount*/)
        {
            UserMasterBL bL = new UserMasterBL();
            int recordaffected = bL.DeleteUserMaster(UserId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }





        //for Ship drp
        public void GetAllShipForDrp()
        {
            UserMasterBL bL = new UserMasterBL();
            List<UserMasterPOCO> pocoList = new List<UserMasterPOCO>();

            pocoList = bL.GetAllShipForDrp(/*int.Parse(Session["VesselID"].ToString())*/);


            List<UserMasterPOCO> itmasterList = new List<UserMasterPOCO>();

            foreach (UserMasterPOCO up in pocoList)
            {
                UserMasterPOCO unt = new UserMasterPOCO();
                unt.ShipId = up.ShipId;
                unt.ShipName = up.ShipName;

                itmasterList.Add(unt);
            }

            ViewBag.Ships = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.ShipName,
                                                Value = x.ShipId.ToString()
                                            });

        }


    }
}