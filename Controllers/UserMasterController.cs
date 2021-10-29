using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

using System.IO;
using Newtonsoft.Json;

namespace TCCCMS.Controllers
{
    public class UserMasterController : Controller
    {
        private int _shipId = 0;
        // GET: UserMaster

        public ActionResult Index()
        {
            try
            {
                if (Session["Role"].ToString() != "OfficeUser" && Session["Role"].ToString() != "ShipUser")
                {
                    GetAllRanksForDrp();
                    GetAllShipForDrp();
                    return View();
                }
                else
                    return RedirectToAction("Login", "Home");
            }catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }
            

        }

        public ActionResult CompanyUser()
        {
            GetAllRanksForDrp();
            GetAllShipForDrp();
            return View();
        }

        public ActionResult SupportUser()
        {
            GetAllRanksForDrp();
            GetAllShipForDrp();
            return View();
        }
        [HttpPost]
        public JsonResult LoadData()
        {
            _shipId = Convert.ToInt32(Session["ShipId"].ToString()); // Added on 17th Aug 2021
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
            pocoList = bL.GetAllUserPageWise(pageIndex, ref totalrecords, length, Convert.ToInt32(Session["UserType"].ToString()), _shipId);
            //List<UserMasterPOCO> pList = new List<UserMasterPOCO>();
            //foreach (UserMasterPOCO pC in pocoList)
            //{
            //    UserMasterPOCO pOCO = new UserMasterPOCO();
            //    pOCO.UserId = pC.UserId;
            //    pOCO.UserName = pC.UserName;
            //    pOCO.UserCode = pC.UserCode;
            //    pOCO.CreatedOn1 = pC.CreatedOn1;
            //    pOCO.Email = pC.Email;
            //    //pOCO.CreatedBy = pC.CreatedBy;
            //    //pOCO.ModifiedBy = pC.ModifiedBy;
            //    pOCO.Gender = pC.Gender;
            //    pOCO.VesselIMO = pC.VesselIMO;
            //    //pOCO.RankName = pC.RankName;
            //    //pOCO.ShipName = pC.ShipName;

            //    pList.Add(pOCO);
            //}

            var data = pocoList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }




        public JsonResult SaveUpdateUser(UserMasterPOCO pOCO)
        {
            UserMasterBL bL = new UserMasterBL();
            UserMasterPOCO pC = new UserMasterPOCO();

            pC.UserId       = pOCO.UserId;
            pC.RankId       = pOCO.RankId;
            pC.ShipId       = pOCO.ShipId;
            pC.UserName     = pOCO.UserName;
            pC.UserCode     = pOCO.UserCode;
            pC.Password     = pOCO.Password;
            pC.Email        = pOCO.Email;
            pC.CreatedBy    = pOCO.CreatedBy;
            pC.ModifiedBy   = pOCO.ModifiedBy;
            pC.Gender       = pOCO.Gender;
            pC.VesselIMO    = pOCO.VesselIMO;
            pC.RoleType = pOCO.RoleType;
            //pC.UserCode = pOCO.UserCode;
            pC.UserType     = pOCO.UserType;
            //pC.IsAdmin      = pOCO.IsAdmin;

            return Json(bL.SaveUpdateUser(pC), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUpdateCompanyUser(UserMasterPOCO pOCO)
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

            //pC.UserCode = pOCO.UserCode;
            pC.UserType = 2;
            pC.IsAdmin = pOCO.IsAdmin;

            return Json(bL.SaveUpdateUser(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUpdateSupportUser(UserMasterPOCO pOCO)
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

            //pC.UserCode = pOCO.UserCode;
            pC.UserType = 0;
            pC.IsAdmin = pOCO.IsAdmin;

            return Json(bL.SaveUpdateUser(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserByUserId(int UserId)
        {
            UserMasterBL bL = new UserMasterBL();
            UserMasterPOCO pOCOList = new UserMasterPOCO();

            pOCOList = bL.GetUserByUserId(UserId);

            UserMasterPOCO dept = new UserMasterPOCO();

            dept.UserId = pOCOList.UserId;
            dept.UserName = pOCOList.UserName;
            dept.UserCode = pOCOList.UserCode;//Added on 02nd Feb 2021
            dept.Password = pOCOList.Password;
            dept.CreatedOn = pOCOList.CreatedOn;
            dept.Email = pOCOList.Email;
            //dept.CreatedBy = pOCOList.CreatedBy;
            //dept.ModifiedBy = pOCOList.ModifiedBy;
            dept.Gender = pOCOList.Gender;
            dept.VesselIMO = pOCOList.VesselIMO;
            dept.RankId = pOCOList.RankId;
            dept.ShipId = pOCOList.ShipId;

            dept.UserType = pOCOList.UserType;
            dept.IsAdmin = pOCOList.IsAdmin;
            dept.RoleId = pOCOList.RoleId;

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUserMaster(int UserId/*, ref string recordCount*/)
        {
            UserMasterBL bL = new UserMasterBL();
            int recordaffected = bL.DeleteUserMaster(UserId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ApprovedRoNotInUserMaster(int UserId/*, ref string recordCount*/)
        {
            UserMasterBL bL = new UserMasterBL();
            int recordaffected = bL.ApprovedRoNotInUserMaster(UserId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UploadPermissionUserMaster(int UserId/*, ref string recordCount*/)
        {
            UserMasterBL bL = new UserMasterBL();
            int recordaffected = bL.UploadPermissionUserMaster(UserId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetIMONumberByShip(string shipId)
        {
            ApproverMaster approver = new ApproverMaster();
            ShipBL shipBL = new ShipBL();
            Ship ship = new Ship();

            ship = shipBL.GetShipDetailsById(Convert.ToInt32(shipId));
            approver.VesselIMONumber = ship.IMONumber;
            approver.Ship = ship;

            return Json(approver, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateUserCode(string userType, string shipId, string rankId, string userName)
        {
            string code = "";
            UserMasterBL userBl = new UserMasterBL();

            code = userBl.GenerateUserCode(userType, shipId, rankId, userName);

            return Json(code, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllowSignatureUpload(int UserId)
        {
            UserMasterBL bL = new UserMasterBL();
            int recordaffected = bL.AllowSignatureUpload(UserId);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }


        #region Upload Form

        [HttpGet]
        public ActionResult UploadForm()
        {
            _shipId = 6;
            GetApproverListByShipDropDown(_shipId);
            GetApproverLevel();
            return View();
        }
        
        
        #endregion

        #region DropDown
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
            //itmasterList.Add(new UserMasterPOCO { RankId = -1, RankName = "Please Select One" });
            ViewBag.Ranks = itmasterList.OrderBy(s => s.RankId).Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.RankName,
                                                Value = x.RankId.ToString()
                                            });

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
            //itmasterList.Add(new UserMasterPOCO { ShipId = -1, ShipName = "Please Select One" });
            ViewBag.Ships = itmasterList.OrderBy(s => s.ShipId).Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.ShipName,
                                                Value = x.ShipId.ToString()
                                            });

        }

        public void GetApproverLevel()
        {
            ApproverMasterBL approverBl = new ApproverMasterBL();
            List<ApproverLevel> approverLevelList = new List<ApproverLevel>();

            approverLevelList = approverBl.GetApproverLevelForDopDown();


            ViewBag.ApproverLevel = approverLevelList.OrderBy(a => a.ID).Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.Description,
                                                Value = x.ID.ToString()
                                            });

        }

        public void GetApproverListByShipDropDown(int shipId)
        {
            ApproverMasterBL approverBl = new ApproverMasterBL();
            List<UserMasterPOCO> userList = new List<UserMasterPOCO>();
            userList = approverBl.GetApproverListByShipForDopDown(shipId);
            ViewBag.Approver = userList.OrderBy(u => u.UserId).Select(r =>
                                              new SelectListItem()
                                              {
                                                  Text = r.UserName,
                                                  Value = r.UserId.ToString()
                                              }).ToList();
        }


        #endregion

        #region Utility Methods
        private string GetUniqueFileName(string fileName)
        {//Added on 8th jan 2021 @bk
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        private string GetUniqueFileNameWithUserId(string fileName)
        {
            //string userId = "1";

            string userId = Session["UserId"].ToString();
            string shipId = Session["ShipId"].ToString();
            var n = DateTime.Now;
            fileName = Path.GetFileName(fileName);
           
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + shipId
                      +"-"
                      + userId
                      + "_"
                      + string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", n.Year - 2000, n.Month, n.Day, n.Hour, n.Minute, n.Second)
                      + Path.GetExtension(fileName);
            //----test
            //fileName = Path.GetFileNameWithoutExtension(fileName);
            //string s = string.Format(fileName + "_" + userId + "_{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", n.Year - 2000, n.Month, n.Day, n.Hour, n.Minute, n.Second);
            //return s+ Path.GetExtension(fileName);
        }
        #endregion


        public ActionResult GetRoleByUserId(int UserId/*, ref string recordCount*/)
        {
            UserMasterBL bL = new UserMasterBL();
            string recordaffected = bL.GetRoleByUserId(UserId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }
    }
}