using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class FormsAndChecklistsController : Controller
    {
        // GET: FormsAndChecklists
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdministrationAndMmgt()
        {
            return View();
        }

        public ActionResult ManagementOfShipPersonnel()
        {
            return View();
        }

        public ActionResult Maintenance()
        {
            return View();
        }

        public ActionResult NavigationManagement()
        {
            return View();
        }

        public ActionResult CargoOperation()
        {
            return View();
        }

        public ActionResult SafetyMmgt()
        {
            return View();
        }

        public ActionResult FilingSystemAndRetentionMatrix()
        {
            return View();
        }





        // under Section 5 - CargoOperation
        public ActionResult CO031()
        {
            return View();
        }





        public JsonResult LoadData(int CategoryId)
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
                length = 50000;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            DownloadableFromsBL bL = new DownloadableFromsBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<DownloadableFroms> pocoList = new List<DownloadableFroms>();
            pocoList = bL.GetDownloadableFromsPageWise(pageIndex, ref totalrecords, length, CategoryId);
            List<DownloadableFroms> pList = new List<DownloadableFroms>();
            foreach (DownloadableFroms pC in pocoList)
            {
                DownloadableFroms pOCO = new DownloadableFroms();
                pOCO.ID = pC.ID;
                pOCO.FormName = pC.FormName;
                pOCO.Path = pC.Path;
                pOCO.Version = pC.Version;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }


    }
}