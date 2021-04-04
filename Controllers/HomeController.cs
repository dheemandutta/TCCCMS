using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using System.Text;
using System.Web.Routing;

namespace TCCCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Test2()
        {        
            return View();
        }

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
            Session["UserId"] = null;
            Session["UserCode"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["ShipId"] = null;
            Session["ShipName"] = null;
            Session["VesselIMO"] = null;
            Session["UserType"] = null;

            System.Web.HttpContext.Current.Session["Role"] = null;

            return View();
        }
        [HttpPost]
        public ActionResult Login(UserMasterPOCO user)
        {
            UserMasterBL userMasterBL = new UserMasterBL();
            HomeBL homeBl = new HomeBL();
            string lsReturnMessage = "0";
            UserMasterPOCO lUser = new UserMasterPOCO();
            lUser = homeBl.CheckUserLogin(user,ref lsReturnMessage);
            if(lsReturnMessage == "1")
            {



                string role = userMasterBL.GetRoleByUserId(lUser.UserId);
                if (!string.IsNullOrEmpty(role))
                {
                    System.Web.HttpContext.Current.Session["Role"] = role;
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }





                Session["UserId"]       = lUser.UserId.ToString();
                Session["UserCode"]     = lUser.UserCode.ToString();
                Session["UserName"]     = lUser.UserName.ToString();
                Session["Email"]        = lUser.Email.ToString();
                Session["ShipId"]       = lUser.ShipId.ToString();

                if (!string.IsNullOrEmpty(lUser.ShipName))
                    Session["ShipName"] = lUser.ShipName.ToString();
                else
                    Session["ShipName"] = "";

                if (!string.IsNullOrEmpty(lUser.VesselIMO))
                    Session["VesselIMO"] = lUser.VesselIMO.ToString();
                else
                    Session["VesselIMO"] = "";

                Session["UserType"]     = lUser.UserType.ToString();
                //Session["IsAdmin"]      = lUser.IsAdmin.ToString();

                return RedirectToAction("UserDashboard", "Dashboard");

                //return new RedirectToRouteResult(new RouteValueDictionary(
                //new { action = "UserDashboard", controller = "Dashboard" }));
            }
            else
            {
                return Json(lsReturnMessage,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
        public ActionResult MenuLayout()
        {
            Menu menu = new Menu();
            menu.Menulist = GenerateMenu();
            return PartialView("_Menu_Layout", menu);
        }
        #region Generate Menu--

        public string GenerateMenu()
        {
            ManualBL manuBl = new ManualBL();

            
            string xPath = Server.MapPath( "~/xmlMenu/" + "ALLVOLUMES.xml");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {
               
                sb.Append("<ul>");
                foreach (XmlNode volume in node)
                {
                    Volume vol = new Volume();
                    string volName = volume.Attributes["name"].Value.ToString();
                    string partName = volName.Split(' ').Last();
                    string volumeId = volume.Attributes["id"].Value.ToString();
                    vol = manuBl.GetVolumeById(volumeId);
                    sb.Append("\n");
                    //sb.Append("<li class='mainmenu'><a href='#'><span class='vul'>Volume <b>I</b> </span><span class='pgnam'>" + volName + "</span></a>");
                    //string s= "'@Url.Action('"
                    //sb.Append("<li class='mainmenu'><a href='@Url.Action('Index', '"+vol.ControllerName+"')'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                    sb.Append("<li class='mainmenu'><a href='/" + vol.ControllerName + "/Index'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                    sb.Append("\n");

                    #region Lines Commented
                    /* ----------Lines Commented on 23rd Feb 2021 @BK  */
                    //sb.Append("<ul class='submenu'>");
                    //foreach (XmlNode item in volume)
                    //{
                    //    Manual manual = new Manual();
                    //    int l = 1;

                    //    if (item.Name == "filename")
                    //    {
                    //        string filename = item.InnerText.ToString();
                    //        manual = manuBl.GetActionNameByFileName(filename+".html");
                    //        if(manual.ActionName != null)
                    //        {
                    //            sb.Append("\n");
                    //            //sb.Append("<li><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + "</span></a></li>");
                    //            //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                    //            //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                    //            ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                    //            sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>");
                    //            sb.Append(partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");


                    //        }

                    //    }
                    //    else if (item.Name == "foldername")
                    //    {

                    //        string fName = item.Attributes["name"].Value.ToString();
                    //        sb.Append("\n");
                    //        sb.Append("<li class='menu_itm'><a href='#'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + fName + "</span></a>");
                    //        sb.Append("\n");
                    //        sb.Append("<ul class='submenuL" + l + "'>");
                    //        string sChild = GetChild(item, ref l, partName);
                    //        //l = l;
                    //        sb.Append(sChild);
                    //        sb.Append("\n");
                    //        sb.Append("</ul>");
                    //        sb.Append("\n");
                    //        sb.Append("</li>");
                    //    }



                    //}
                    //sb.Append("\n");
                    //sb.Append("</ul>");
                    /* ----End------Lines Commented on 23rd Feb 2021 @BK  */
                    #endregion
                    sb.Append("\n");
                    sb.Append("</li>");


                }
                sb.Append("\n");
                sb.Append("</ul>");

                //WriteToText(sb);
               
            }
            return sb.ToString();
        }

        public string GetChild(XmlNode node, ref int l, string part)
        {
            ManualBL manuBl = new ManualBL();
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode item in node)
            {
                Manual manual = new Manual();
                if (item.Name == "filename")
                {

                    string filename = item.InnerText.ToString();
                    manual = manuBl.GetActionNameByFileName(filename + ".html");
                    if(manual.ActionName != null)
                    {
                        sb.Append("\n");
                        //sb.Append("<li ><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                        sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>");
                        sb.Append(part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                    }

                }
                else if (item.Name == "foldername")
                {
                    int x = 0;
                    string fName = item.Attributes["name"].Value.ToString();
                    sb.Append("\n");

                    sb.Append("<li class='menu_itmL" + l + "'><a href='#'><span class='vul'>Volume <b>"+part+"</b> </span><span class='pgnam' style='background-color:salmon; '>" + fName + " </span></a>");
                    sb.Append("\n");
                    sb.Append("<ul class='submenuL" + (l + 1) + "'>");
                    x = l + 1;
                    string sChild = GetChild(item, ref x, part);
                    sb.Append("\n");
                    sb.Append(sChild);
                    sb.Append("\n");
                    sb.Append("</ul>");
                    sb.Append("\n");
                    sb.Append("</li>");
                }

            }

            return sb.ToString();
        }

        #endregion



        //public ActionResult GetRoleByUserId(int UserId/*, ref string recordCount*/)
        //{
        //    UserMasterBL bL = new UserMasterBL();
        //    string recordaffected = bL.GetRoleByUserId(UserId/*, ref recordCount*/);
        //    return Json(recordaffected, JsonRequestBehavior.AllowGet);

        //}
    }
}