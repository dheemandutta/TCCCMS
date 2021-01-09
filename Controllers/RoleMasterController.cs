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
    public class RoleMasterController : Controller
    {
        // GET: RoleMaster
        public ActionResult Index()
        {
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

            RoleMasterBL bL = new RoleMasterBL();
            int totalrecords = 0;

            List<RoleMasterPOCO> pocoList = new List<RoleMasterPOCO>();
            pocoList = bL.GetAllRoleMasterPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<RoleMasterPOCO> pList = new List<RoleMasterPOCO>();
            foreach (RoleMasterPOCO pC in pocoList)
            {
                RoleMasterPOCO pOCO = new RoleMasterPOCO();
                pOCO.RoleId = pC.RoleId;
                pOCO.RoleName = pC.RoleName;
                //pOCO.CreatedBy = pC.CreatedBy;
                //pOCO.ModifiedBy = pC.ModifiedBy;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUpdateRoleMaster(RoleMasterPOCO pOCO)
        {
            RoleMasterBL bL = new RoleMasterBL();
            RoleMasterPOCO pC = new RoleMasterPOCO();

            pC.RoleId = pOCO.RoleId;

            pC.RoleName = pOCO.RoleName;
            pC.CreatedBy = pOCO.CreatedBy;
            pC.ModifiedBy = pOCO.ModifiedBy;

            return Json(bL.SaveUpdateRoleMaster(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleMasterByRoleId(int RoleId)
        {
            RoleMasterBL bL = new RoleMasterBL();
            RoleMasterPOCO pOCOList = new RoleMasterPOCO();

            pOCOList = bL.GetRoleMasterByRoleId(RoleId);

            RoleMasterPOCO dept = new RoleMasterPOCO();

            dept.RoleId = pOCOList.RoleId;
            dept.RoleName = pOCOList.RoleName;
            //dept.CreatedBy = pOCOList.CreatedBy;
            //dept.ModifiedBy = pOCOList.ModifiedBy;

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRoleMaster(int RoleId/*, ref string recordCount*/)
        {
            RoleMasterBL bL = new RoleMasterBL();
            int recordaffected = bL.DeleteRoleMaster(RoleId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);
        }

    }
}