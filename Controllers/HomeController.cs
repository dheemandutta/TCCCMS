using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserMasterPOCO user)
        {
            string lsReturnMessage = "";
            HomeBL homeBl = new HomeBL();
            UserMasterPOCO lUser = new UserMasterPOCO();

            lUser = homeBl.CheckUserLogin(user,ref lsReturnMessage);

            if(lsReturnMessage == "1")
            {
                Session["UserId"]       = lUser.UserId.ToString();
                Session["UserCode"]     = lUser.UserCode.ToString();
                Session["UserName"]     = lUser.UserName.ToString();
                Session["Email"]        = lUser.Email.ToString();
                Session["ShipId"]       = lUser.ShipId.ToString();
                Session["ShipName"]     = lUser.ShipName.ToString();
                Session["VesselIMO"]    = lUser.VesselIMO.ToString();
                Session["UserType"]     = lUser.UserType.ToString();
                Session["IsAdmin"]      = lUser.IsAdmin.ToString();

                return RedirectToAction("Index","Ship");
            }
            else
            {
                return Json(lsReturnMessage,JsonRequestBehavior.AllowGet);
            }

        }
    }
}