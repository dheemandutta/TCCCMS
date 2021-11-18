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
    public class RankController : Controller
    {
        // GET: Rank

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

            RankBL bL = new RankBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<RankPOCO> pocoList = new List<RankPOCO>();
            pocoList                = bL.GetAllRankPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<RankPOCO> pList    = new List<RankPOCO>();
            foreach (RankPOCO pC in pocoList)
            {
                RankPOCO pOCO       = new RankPOCO();
                pOCO.RankId         = pC.RankId;
                pOCO.RankName       = pC.RankName;
                pOCO.Description    = pC.Description;
                pOCO.Email          = pC.Email;//Added on 30th Jan 2021 @BK

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public JsonResult SaveUpdateRank(RankPOCO pOCO)
        {
            RankBL bL       = new RankBL();
            RankPOCO pC     = new RankPOCO();

            pC.RankId       = pOCO.RankId;

            pC.RankName     = pOCO.RankName;
            pC.Description  = pOCO.Description;
            pC.Email        = pOCO.Email;//Added on 30th Jan 2021 @BK

            return Json(bL.SaveUpdateRank(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizationFilter]
        public JsonResult GetRankByRankId(int RankId)
        {
            RankBL bL           = new RankBL();
            RankPOCO pOCOList   = new RankPOCO();

            pOCOList            = bL.GetRankByRankId(RankId);

            RankPOCO dept       = new RankPOCO();

            dept.RankId         = pOCOList.RankId;
            dept.RankName       = pOCOList.RankName;
            dept.Description    = pOCOList.Description;
            dept.Email          = pOCOList.Email;//Added on 30th Jan 2021 @BK

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public ActionResult DeleteRank(int RankId/*, ref string recordCount*/)
        {
            RankBL bL = new RankBL();
            int recordaffected = bL.DeleteRank(RankId/*, ref recordCount*/);
            return Json(recordaffected, JsonRequestBehavior.AllowGet);

        }
    }
}