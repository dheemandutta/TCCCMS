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
            return View();
        }

        public JsonResult GetLogForUserExcel(/*int Id*/)
        {
            LogForUserExcelBL bL = new LogForUserExcelBL();
            LogForUserExcelPOCO pOCOList = new LogForUserExcelPOCO();

            pOCOList = bL.GetLogForUserExcel(/*Id*/);

            LogForUserExcelPOCO dept = new LogForUserExcelPOCO();

            //dept.Id = pOCOList.Id;
            dept.LogData = pOCOList.LogData;
            dept.Count = pOCOList.Count;

            var data = dept;

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