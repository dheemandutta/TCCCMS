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
        //{---
        //    return View();
        //}
        public ActionResult SearchList(int currentPage = 1, string volNo = "-1", string text = null)
        {
            Session["IsSearched"] = "1";
            ManualBL manualBL = new ManualBL();
            int totalrecords = 0;
            ManualViewModel fsvm = new ManualViewModel();
            List<Manual> manuals = new List<Manual>();
            int shipId = 0;
            string category = null;//Added on 19th Jun 2021

            if(Session["ShipId"] !=null && Session["ShipId"].ToString() != "0")
            {
                shipId = int.Parse(Session["ShipId"].ToString());
            }
            else if(Session["DashboardShipId"].ToString() != null)
            {
                shipId = int.Parse(Session["DashboardShipId"].ToString());
            }

            if (Request.QueryString["s"] != null)
            {
                text = Request.QueryString["s"].ToString().Trim();
            }

            if(Request.QueryString["vol"] != null)
            {
                volNo = Request.QueryString["vol"].ToString();
                if(volNo != "-1")//Added on 21st Jun 2021
                {
                    shipId = 0;
                }
            }
            else if (volNo != "-1")//Added on 21st Jun 2021
            {
                shipId = 0;
            }


            if (Request.QueryString["cat"] != null)
            {
                category = Request.QueryString["cat"].ToString();
            }



            Pagination pgn = new Pagination();
            pgn.CurrentPage = currentPage == 0 ? 1 : currentPage;

            string searchText;
            if (String.IsNullOrEmpty(text))
                searchText = text;
            if (text == null)
            {
                searchText = "";
                //TempData["SearchText"] = "Business";
                //stop searching -- Dheeman
                //fsvm.SearchText = "Business";
                // show modal popup to user -- Dheeman 
                //searchText = "";
                
                
            }
            else
            {

                //Console.WriteLine(words);
                //    st= val;
                TempData["SearchText"] = text;
                searchText = text;
                fsvm.SearchText = text;

                manuals = manualBL.SearchManuals(currentPage, ref totalrecords, pgn.PageSize, Convert.ToInt32(volNo), searchText, shipId, category);
                pgn.Count = manuals.Count();
                fsvm.ManualList = manuals.Skip((pgn.CurrentPage - 1) * pgn.PageSize).Take(pgn.PageSize).ToList();
                fsvm.VolumeId = Convert.ToInt32(volNo);
                fsvm.Pagination = pgn;
               
            }

           


            return View(fsvm);

        }
        [HttpGet]
        public JsonResult GetSearchText()
        {
            string text = "";
            if (TempData["SearchText"] != null && Session["IsSearched"].ToString() == "1")
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