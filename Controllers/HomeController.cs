﻿using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using System.Text;
using System.Net.Mail;
using System.Web.Routing;
using System.Web.Caching;
using System.Configuration;
using TCCCMS.Infrastructure;
using System.Web.Security;

namespace TCCCMS.Controllers
{
   
    public class HomeController : Controller
    {
        private Cache _cache= new Cache();

        [CustomAuthorizationFilter]
        public ActionResult ChangePassword()
        {
            UserMasterPOCO userMaster = new UserMasterPOCO();
            try
            {
                if (Session["Role"].ToString() == "OfficeUser" || Session["Role"].ToString() == "ShipUser")
                {
                    userMaster.hasChange = 0;
                    return View(userMaster);
                }
                else
                    return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        [CustomAuthorizationFilter]
        public ActionResult ChangePassword(UserMasterPOCO aUserMaster)
        {
            try
            {
                UserMasterBL umBL = new UserMasterBL();
                int isValid = 0;
                if(aUserMaster.UserId ==0)
                {
                    aUserMaster.UserId = int.Parse(Session["UserId"].ToString());
                }
                isValid = umBL.ChangePassword(aUserMaster);

                if(isValid == 0)
                {
                    //return Json(isValid, JsonRequestBehavior.AllowGet);
                    aUserMaster.hasChange = 2;
                    return View(aUserMaster);
                }
                else
                {
                    //return RedirectToAction("Login", "Home");
                    return new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "Login", controller = "Home" }));
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [CustomAuthorizationFilter]
        public ActionResult Test2()
        {        
            return View();
        }
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizationFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [CustomAuthorizationFilter]
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
        [AllowAnonymous]
        public ActionResult Login()
        {
            //string forgotFor = ConfigurationManager.AppSettings["officeOrShipserver"].ToString();
            Session["UserId"] = null;
            Session["UserCode"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["ShipId"] = null;
            Session["ShipName"] = null;
            Session["VesselIMO"] = null;
            Session["UserType"] = null;

            System.Web.HttpContext.Current.Session["Role"] = null;

            Session["DashboardShipId"] = null;//used in Ship Dashboard for Ship Layout

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserMasterPOCO user)
        {
            UserMasterBL userMasterBL   = new UserMasterBL();
            HomeBL homeBl               = new HomeBL();
            string lsReturnMessage      = "0";
            string lsApprover           = "0";// Added on 29th Jul 2021 @BK
            string lsAllowSign          = "0";// Added on 29th Jul 2021 @BK
            UserMasterPOCO lUser        = new UserMasterPOCO();
            lUser                       = homeBl.CheckUserLogin(user,ref lsReturnMessage);
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


                ////----------------------------------------------------------------------------------
                ///

                //_cache.Insert("VolMenuData", Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml"));
                //_cache.Insert("ShipMenuData", Server.MapPath("~/xmlMenu/" + "ALLSHIPS1.xml"));


                ///---------------------------------------------------------------------------------
                ///

               

                Session["UserId"]           = lUser.UserId.ToString();
                Session["UserCode"]         = lUser.UserCode.ToString();
                Session["UserName"]         = lUser.UserName.ToString();
                Session["Email"]            = lUser.Email.ToString();
                Session["ShipId"]           = lUser.ShipId.ToString();
                Session["DashboardShipId"]  = lUser.ShipId.ToString();
                Session["IsApprover"]       = lUser.IsApprover.ToString();//Added on 29th Jul 2021 @BK
                lsApprover                  = lUser.IsApprover.ToString();//Added on 29th Jul 2021 @BK
                Session["IsAllowSign"]      = lUser.IsAllowSign.ToString();//Added on 29th Jul 2021 @BK
                lsAllowSign                 = lUser.IsAllowSign.ToString();//Added on 29th Jul 2021 @BK

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


                if (role == "OfficeAdmin" || role == "ShipAdmin")
                {
                    return RedirectToAction("AdminDashboard", "Dashboard");
                }
                else if(role == "OfficeUser")
                {
                    if(lsApprover == "1" && lsAllowSign == "0")
                        return RedirectToAction("Index", "ApproverSign");
                    else
                        return RedirectToAction("UserDashboard", "Dashboard");
                } 
                else
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
        [CustomAuthorizationFilter]
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
        [CustomAuthorizationFilter]
        public ActionResult MenuLayout()
        {
            Menu menu = new Menu();
            
            menu.Menulist = GenerateMenu();
            return PartialView("_Menu_Layout", menu);
        }

        /// <summary>
        ///  Added on 26th Apr 2021
        /// </summary>
        /// <returns></returns>

        [CustomAuthorizationFilter]
        public ActionResult ShipsMenuLayout()
        {
            Menu menu = new Menu();
            if(Session["DashboardShipId"].ToString() != "")
            {
                menu.Menulist = GenerateShipWiseMenu(Convert.ToInt32(Session["DashboardShipId"].ToString()));
            }
            else
            {
                menu.Menulist = GenerateShipWiseMenu(1);
            }

            return PartialView("_Menu_Layout", menu);
        }
        #region Generate Menu--

        public string GenerateMenu()
        {
            ManualBL manuBl = new ManualBL();

           
            string menu0Enabled = ConfigurationManager.AppSettings["menu0Enabled"].ToString();
            string xPath = Server.MapPath( "~/xmlMenu/" + "ALLVOLUMES.xml");
            XmlDocument xDoc = new XmlDocument();
            //if (_cache.Get("VolMenuData") != null)
            //{
            //    xDoc.Load(xPath);
            //    //_cache.Insert("VolMenuData", xPath);
            //    _cache.Insert("VolMenuData", xDoc, new CacheDependency(xPath));
            //}
            //else
            //{
            //    xDoc = (XmlDocument)_cache.Get("VolMenuData");
            //}

            //if ((XmlDocument)Cache["VolMenuData"])
            //{

            //}

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

                   // string ctrlName = volume.Attributes["controllername"].Value.ToString();//--Added on 04/04/2021
                    sb.Append("\n");
                    //sb.Append("<li class='mainmenu'><a href='#'><span class='vul'>Volume <b>I</b> </span><span class='pgnam'>" + volName + "</span></a>");
                    //string s= "'@Url.Action('"
                    //sb.Append("<li class='mainmenu'><a href='@Url.Action('Index', '"+vol.ControllerName+"')'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                    //sb.Append("<li class='mainmenu'><span class='tooltip'><a href='/" + vol.ControllerName + "/Index'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='tooltiptext'>" + vol.Description + "</span></a></span>");
                    if (volumeId == "0" && menu0Enabled == "false")
                    {
                        //menu 0 has no click at Hong Kong Office
                        sb.Append("<li class='mainmenu'><span class='tooltip'><a><span class='vul'>Volume <b>" + partName + "</b> </span><span class='tooltiptext'>" + vol.Description + "</span></a></span>");
                    }
                    else
                    {
                        sb.Append("<li class='mainmenu'><span class='tooltip'><a href='/" + vol.ControllerName + "/Index'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='tooltiptext'>" + vol.Description + "</span></a></span>");
                    }
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
        /// <summary>
        /// Added on 26th Apr 2021
        /// For Ship Ship Specific Menu
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public string GenerateShipWiseMenu(int shipId)
        {
            ManualBL manuBl = new ManualBL();


            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS1.xml");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);

            //if (_cache.Get("ShipMenuData") == null)
            //{
            //    xDoc.Load(xPath);
            //    _cache.Insert("ShipMenuData", xDoc, new CacheDependency(xPath));
            //}
            //else
            //{
            //    xDoc = (XmlDocument)_cache.Get("ShipMenuData");
            //}

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {

                sb.Append("<ul>");
                foreach (XmlNode ship in node)
                {
                    Volume vol = new Volume();
                    string sName = ship.Attributes["name"].Value.ToString();
                    string ctrlName = ship.Attributes["controllername"].Value.ToString();
                    string sNo = ship.Attributes["shipnumber"].Value.ToString();
                    string relaiveFilePath = sName;//---this for pdf preview link path
                    if (Convert.ToInt32(sNo) == shipId)
                    {
                        foreach (XmlNode item in ship)
                        {
                            if (item.Name == "filename")
                            {
                                string filename = item.InnerText.ToString();
                                string actionName = item.Attributes["actionname"].Value.ToString();
                                string type = item.Attributes["doctype"].Value.ToString();
                                string isDownload = item.Attributes["isdownloadable"].Value.ToString();
                                // manual = manuBl.GetActionNameByFileName(filename + ".html");
                                if (type == "DOC" && actionName != "")
                                {
                                    sb.Append("\n");
                                    
                                    sb.Append("<li class='mainmenu'><a href='/" + ctrlName + "/Pages?actionName=" + actionName+"' ><span class='pgnam'>" + filename + "</span></a>");


                                }
                                else if (type == "PDF")
                                {
                                    sb.Append("\n");
                                    sb.Append("<li class='mainmenu'><a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                    sb.Append("<span class='pgnam'>"+filename + "</span></a>");
                                    //sb.Append("</br>");

                                }

                            }
                            else if (item.Name == "foldername")
                            {


                                string fName = item.Attributes["name"].Value.ToString();
                                string fDesc = item.Attributes["description"].Value.ToString();
                                string actionName = item.Attributes["actionname"].Value.ToString();
                               
                                if(actionName != "")
                                {
                                    if(fDesc != "")
                                    {
                                        sb.Append("<li class='mainmenu'><a href='/" + ctrlName + "/" + actionName + "'><span class='pgnam'>" + fDesc + "</span></a>");
                                    }
                                    else
                                    {
                                        sb.Append("<li class='mainmenu'><a href='/" + ctrlName + "/" + actionName + "'><span class='pgnam'>" + fName + "</span></a>");

                                    }
                                    

                                }

                                

                            }
                        }

                        sb.Append("\n");

                        
                        sb.Append("\n");
                        sb.Append("</li>");
                    }


                }
                sb.Append("\n");
                sb.Append("</ul>");

                //WriteToText(sb);

            }
            return sb.ToString();
        }

        #endregion

        [CustomAuthorizationFilter]
        public JsonResult GetUserType()
        {
           
            return Json(Session["UserType"].ToString(), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetRoleByUserId(int UserId/*, ref string recordCount*/)
        //{
        //    UserMasterBL bL = new UserMasterBL();
        //    string recordaffected = bL.GetRoleByUserId(UserId/*, ref recordCount*/);
        //    return Json(recordaffected, JsonRequestBehavior.AllowGet);

        //}
        [CustomAuthorizationFilter]
        public ActionResult ForgotPassword()
        {
            UserMasterPOCO userMaster = new UserMasterPOCO();
            try
            {
              
                    userMaster.hasChange = 0;
                    return View(userMaster);
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        [CustomAuthorizationFilter]
        public ActionResult ForgotPassword(UserMasterPOCO aUserMaster)
        {
            bool isSendSuccessfully = false;
            try
            {
                UserMasterBL umBL = new UserMasterBL();
                int isValid = 0;
                string initialPwd = "";
                string userCode = "";
                string receiverEmail = aUserMaster.Email;
                isValid = umBL.ForgotPassword(aUserMaster,ref initialPwd,ref userCode);

                StringBuilder mailBody = new StringBuilder();

                if (isValid == 0)
                {
                    //return Json(isValid, JsonRequestBehavior.AllowGet);
                    aUserMaster.hasChange = 2;
                    return View(aUserMaster);
                }
                else if (isValid == 1)
                {
                    aUserMaster.hasChange = 1;

                    #region send mail----

                    //receiverEmail = um.Email.ToString();

                    MailMessage mail = new MailMessage();

                    mailBody.Append("User Code : ");
                    mailBody.Append(userCode.ToString());
                    mailBody.Append("\n");
                    mailBody.Append("Initial Password : ");
                    mailBody.Append(initialPwd.ToString());
                    mailBody.Append("\n");
                    mailBody.Append("\n");
                    mailBody.Append("Messege : ");
                    mailBody.Append("Login with above User Code and Password and Change your Password");
                    mailBody.Append("\n");
                    mail.Body = mailBody.ToString();


                    SendEmail.SendMail("TCC Request for Forgot Password", "", receiverEmail, mail, ref isSendSuccessfully);
                    



                    #endregion

                    return View(aUserMaster);
                }
                else 
                {

                    //return RedirectToAction("Login", "Home");
                    return new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "Login", controller = "Home" }));
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public JsonResult KeepSessionActive()
        {

            return new JsonResult
            {
                Data = "Beat Generated"
            };
        }
    }

    public static class CacheHandler
    {
        public static void Add<T>(T objInfo, string key)
        {
            HttpContext.Current.Cache.Insert(key, objInfo, null, DateTime.Now.AddMinutes(1440), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public static void Clear(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value =
                        default(T);
                    return false;
                }
                value = (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                value =
                    default(T);
                return false;
            }
            return true;
        }
    
    
    }
}