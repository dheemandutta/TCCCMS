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
    public class ManualController : Controller
    {

        // GET: Manual
        public ActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult SearchList(int currentPage = 1, string volNo = "0", string text = null)
        {
            ManualBL manualBL = new ManualBL();
            int totalrecords = 0;
            ManualViewModel fsvm = new ManualViewModel();
            List<Manual> manuals = new List<Manual>();
           

            Pagination pgn = new Pagination();
            pgn.CurrentPage = currentPage == 0 ? 1 : currentPage;

            string searchText;
            if (text == null)
            {
                searchText = "Business";
                //TempData["SearchText"] = "Business";
                fsvm.SearchText = "Business";
            }
            else
            {

                //Console.WriteLine(words);
                //    st= val;
                TempData["SearchText"] = text;
                searchText = text;
                fsvm.SearchText = text;
            }

            manuals = manualBL.SearchManuals(currentPage, ref totalrecords, pgn.PageSize,Convert.ToInt32(volNo), searchText);

            
            pgn.Count       = manuals.Count();
            fsvm.ManualList = manuals.Skip((pgn.CurrentPage - 1) * pgn.PageSize).Take(pgn.PageSize).ToList();
            fsvm.VolumeId = Convert.ToInt32(volNo);
            fsvm.Pagination = pgn;



            return View(fsvm);
        }
        [HttpGet]
        public JsonResult GetSearchText()
        {
            string text = "Business";
            if (TempData["SearchText"] != null)
            {
                text = TempData["SearchText"].ToString();
                TempData.Keep("SearchText");
                MatchCollection matches = Regex.Matches(text, @"\b[\w']*\b");

                //string[] st;
                var words = from m in matches.Cast<Match>()
                            where !string.IsNullOrEmpty(m.Value)
                            select m.Value;
                //foreach (string val in words)


                return Json(words, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(text, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetText()
        {

            return Json("Business", JsonRequestBehavior.AllowGet);
        }


    }
}