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
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class RevisionHistoryController : Controller
    {

        // GET: RevisionHistory----
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult RevisionHistory()
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


        [CustomAuthorizationFilter]
        public JsonResult GetFormIdForModifiedSection()
        {
            RevisionHistoryBL bL = new RevisionHistoryBL();

            RevisionHistory pC = new RevisionHistory();
            pC.FormId = pC.FormId;

            return Json(bL.GetFormIdForModifiedSection(), JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorizationFilter]
        public JsonResult SaveRevisionHistory(RevisionHistory pOCO)
        {
            RevisionHistoryBL bL = new RevisionHistoryBL();
            RevisionHistory pC = new RevisionHistory();

            return Json(bL.SaveRevisionHistory(pOCO), JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public ActionResult RevisionDetails()
        {
            RevisionHistoryBL rhBl = new RevisionHistoryBL();
            RevisionHeaderHistoryViewModel rhhVM = new RevisionHeaderHistoryViewModel();
            rhhVM = rhBl.GetAllRevisionDetails();

            return View(rhhVM);
        }
        [CustomAuthorizationFilter]
        public JsonResult GetRevisionHeaderForDrp()
        {
            List<RevisionHeader> rHeaderList = new List<RevisionHeader>();
            RevisionHistoryBL rhBl = new RevisionHistoryBL();
            rHeaderList = rhBl.GetRevisionHeaderForDrp();
            var data = rHeaderList;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public JsonResult SaveRevisionHeader(RevisionHeader rHeader)
        {
            RevisionHistoryBL rhBl = new RevisionHistoryBL();
            int x = rhBl.SaveRevisionHeader(rHeader);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]

        public JsonResult SaveRevisionViewer(string revisionId)
        {
            RevisionHistoryBL rhBl = new RevisionHistoryBL();
            RevisionViewer rViewer = new RevisionViewer();
           
            int x = 0;
            if (Session["Role"].ToString() == "ShipUser"/* || Session["Role"].ToString() == "ShipAdmin"*/)
            {
                rViewer.RevisionId = Convert.ToInt32(revisionId);
                rViewer.UserId = Convert.ToInt32(Session["UserId"].ToString());
                rViewer.ShipId = Convert.ToInt32(Session["ShipId"].ToString());//Added on 1st Jun 2021
                x = rhBl.SaveRevisionViewer(rViewer);
            }
               
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationFilter]
        public ActionResult GetViewerByRevision(int id)
        {
           List<RevisionViewer> viewers = new List<RevisionViewer>();
            RevisionHistoryBL rhBL = new RevisionHistoryBL();
            viewers = rhBL.GetAllRevisionViewers(id);
            return PartialView("_pvRevisionViewers", viewers);
        }
    }
}