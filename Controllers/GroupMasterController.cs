using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class GroupMasterController : Controller
    {
        // GET: GroupMaster

        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }


        [CustomAuthorizationFilter]
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

            GroupMasterBL bL = new GroupMasterBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<GroupMasterPOCO> pocoList = new List<GroupMasterPOCO>();
            pocoList = bL.GetAllGroupMasterPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<GroupMasterPOCO> pList = new List<GroupMasterPOCO>();
            foreach (GroupMasterPOCO pC in pocoList)
            {
                GroupMasterPOCO pOCO = new GroupMasterPOCO();
                pOCO.GroupId = pC.GroupId;
                pOCO.GroupName = pC.GroupName;
                //pOCO.CreatedBy = pC.CreatedBy;
                //pOCO.ModifiedBy = pC.ModifiedBy;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizationFilter]
        public JsonResult SaveUpdateGroupMaster(GroupMasterPOCO pOCO)
        {
            GroupMasterBL bL = new GroupMasterBL();
            GroupMasterPOCO pC = new GroupMasterPOCO();

            pC.GroupId = pOCO.GroupId;

            pC.GroupName = pOCO.GroupName;
            pC.CreatedBy = pOCO.CreatedBy;
            pC.ModifiedBy = pOCO.ModifiedBy;

            return Json(bL.SaveUpdateGroupMaster(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizationFilter]
        public JsonResult GetGroupMasterByGroupId(int GroupId)
        {
            GroupMasterBL bL = new GroupMasterBL();
            GroupMasterPOCO pOCOList = new GroupMasterPOCO();

            pOCOList = bL.GetGroupMasterByGroupId(GroupId);

            GroupMasterPOCO dept = new GroupMasterPOCO();

            dept.GroupId = pOCOList.GroupId;
            dept.GroupName = pOCOList.GroupName;
            //dept.CreatedBy = pOCOList.CreatedBy;
            //dept.ModifiedBy = pOCOList.ModifiedBy;

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public ActionResult DeleteGroupMaster(int GroupId/*, ref string recordCount*/)
        {
            GroupMasterBL bL = new GroupMasterBL();
            int recordaffected = bL.DeleteGroupMaster(GroupId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);
        }

    }
}