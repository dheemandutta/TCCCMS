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
    public class RevisionHistoryController : Controller
    {
        // GET: RevisionHistory
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

            RevisionHistoryBL bL = new RevisionHistoryBL(); 
            int totalrecords = 0;

            List<RevisionHistory> pocoList = new List<RevisionHistory>();
            pocoList = bL.GetRevisionHistoryPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<RevisionHistory> pList = new List<RevisionHistory>();
            foreach (RevisionHistory pC in pocoList)
            {
                RevisionHistory pOCO = new RevisionHistory();
                pOCO.ID = pC.ID;
                pOCO.FormName = pC.FormName;
                pOCO.ModifiedSection = pC.ModifiedSection;
                pOCO.UpdatedOn1 = pC.UpdatedOn1;
                pOCO.Version = pC.Version;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetFormIdForModifiedSection()
        {
            RevisionHistoryBL bL = new RevisionHistoryBL();

            RevisionHistory pC = new RevisionHistory();
            pC.FormId = pC.FormId;

            return Json(bL.GetFormIdForModifiedSection(), JsonRequestBehavior.AllowGet);
        }
    }
}