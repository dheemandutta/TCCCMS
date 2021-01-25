using TCCCMS.Models;
using TCCCMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;

namespace TCCCMS.Controllers
{
    public class ApproverController : Controller
    {
        // GET: Approver
        public ActionResult Index()
        {
            GetAllShipForDropDown();
            GetAllRanksForDropDown();
            //GetAllUserForDropDown();


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
                length = 10;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            ApproverMasterBL approverBL = new ApproverMasterBL();
            int totalrecords = 0;

            List<ApproverMaster> sapproverList = new List<ApproverMaster>();
            sapproverList = approverBL.GetAllApproverListPageWise(pageIndex, ref totalrecords, length);
            

            var data = sapproverList;

            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Old / for single approver
        /// </summary>
        /// <param name="approver"></param>
        /// <returns></returns>
        public JsonResult SaveApprover(ApproverMaster approver)
        {
            ApproverMasterBL approverBL = new ApproverMasterBL();
            
            int rowaffected = approverBL.SaveApprover(approver);

            return Json(rowaffected, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// New/ for Multiple approver 
        /// </summary>
        /// <param name="approverList"></param>
        /// <returns></returns>
        public JsonResult SaveApproverList(List<ApproverMaster> approverList = null)
        {
            ApproverMasterBL approverBL = new ApproverMasterBL();
            List<UserMasterPOCO> lst = new List<UserMasterPOCO>();
            //lst = userList;
            //var s = JsonConvert.DeserializeObject(userList);


            int rowaffected = approverBL.SaveApprover(approverList);

            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteApprover(string approverMasterId)
        {
            int recordaffected = 0;
            ApproverMasterBL approverBl = new ApproverMasterBL();
            recordaffected = approverBl.DeleteApprover(Convert.ToInt32(approverMasterId));

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
        /// <summary>
        /// this method used to get Rank by UserId
        /// and call from JS on change User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult GetUserDetailsByUser(string userId)
        {
            ApproverMaster approver = new ApproverMaster();
            UserMasterBL userBl = new UserMasterBL();
            UserMasterPOCO user = new UserMasterPOCO();
            user = userBl.GetUserByUserId(Convert.ToInt32(userId));
            approver.RankId = user.RankId;
            approver.User = user;
            return Json(approver, JsonRequestBehavior.AllowGet);
        }

        #region DropDown
        public void GetAllShipForDropDown()
        {
            ApproverMasterBL sapproverBl = new ApproverMasterBL();
            List<Ship> shipList = new List<Ship>();
            shipList = sapproverBl.GetAllShipForDropDown();
            ViewBag.Ship = shipList.OrderBy(s => s.ID).Select(s =>
                                                            new SelectListItem()
                                                            {
                                                                Text = s.ShipName,
                                                                Value = s.ID.ToString()
                                                            }).ToList();
        }

        public void GetAllRanksForDropDown()
        {
            ApproverMasterBL approverBl = new ApproverMasterBL();
            List<RankPOCO> rankList = new List<RankPOCO>();
            rankList = approverBl.GetAllRanksForDropDown();
            ViewBag.Rank = rankList.OrderBy(r =>r.RankId).Select(r =>
                                                            new SelectListItem()
                                                            {
                                                                Text = r.RankName,
                                                                Value = r.RankId.ToString()
                                                            }).ToList();
        }
        public void GetAllUserForDropDown()
        {
            ApproverMasterBL approverBl = new ApproverMasterBL();
            List<UserMasterPOCO> userList = new List<UserMasterPOCO>();
            userList = approverBl.GetAllUserForDropDown();
            ViewBag.User = userList.OrderBy(u => u.UserId).Select(r =>
                                              new SelectListItem()
                                              {
                                                  Text = r.UserName,
                                                  Value = r.UserId.ToString()
                                              }).ToList();
        }
        public JsonResult GetAllUserByShipForDropDown(string shipId)
        {
            int sId = Convert.ToInt32(shipId);
            ApproverMasterBL approverBl = new ApproverMasterBL();
            List<UserMasterPOCO> userList = new List<UserMasterPOCO>();
            userList = approverBl.GetAllUserByShipForDropDown(sId);
            ViewBag.User = userList.OrderBy(u => u.UserId).Select(r =>
                                              new SelectListItem()
                                              {
                                                  Text = r.UserName,
                                                  Value = r.UserId.ToString()
                                              }).ToList();

            var data = userList.OrderBy(u => u.UserId).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}