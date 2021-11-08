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
    public class LogForUserExcelController : Controller
    {
        // GET: LogForUserExcel
        public ActionResult LogForUserExcel()
        {
            if (Session["Role"].ToString() == "SupportUser" || Session["Role"].ToString() == "ShipAdmin")
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }

        public JsonResult GetLogForUserExcel()
        {
            LogForUserExcelBL bL = new LogForUserExcelBL();
            LogForUserExcelPOCO log = new LogForUserExcelPOCO();

            log = bL.GetLogForUserExcel();

            var data = log;

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveUpdateRank(LogForUserExcelPOCO pOCO)
        {
            LogForUserExcelBL bL = new LogForUserExcelBL();
            LogForUserExcelPOCO pC = new LogForUserExcelPOCO();

            pC.Id = pOCO.Id;
            pC.LogData = pOCO.LogData;

            return Json(bL.SaveUpdateRank(pC  ), JsonRequestBehavior.AllowGet);
        }


    }
}