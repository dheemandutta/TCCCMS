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
using TCCCMS.Infrastructure;
using System.Web.Mvc.Html;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class DashboardController : Controller
    {
        [CustomAuthorizationFilter]
        public ActionResult UserDashboard()
        {
            //-------
            return View();
            //return RedirectToAction("UserDashboard", "Dashboard");
        }
        [CustomAuthorizationFilter]
        public ActionResult CommonToAllManualDashboard()
        {
            //-------
            return View();
            //return RedirectToAction("UserDashboard", "Dashboard");
        }

        [CustomAuthorizationFilter]
        // GET: Dashboard
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [CustomAuthorizationFilter]
        public ActionResult ShipDashboard(string id)
        {
            string x = Session["DashboardShipId"].ToString();
           if(Session["Role"].ToString() == "ShipUser" || Session["Role"].ToString() == "ShipAdmin")
           {
                if (Session["DashboardShipId"].ToString() == null || Session["DashboardShipId"].ToString() == "0")
                {
                    Session["DashboardShipId"] = id;
                }
           }
            else
            {
                if(id != null)
                {

                    Session["DashboardShipId"] = id;
                }
            }
            
           
            return View();
        }
        [CustomAuthorizationFilter]
        public ActionResult ShipMenuLayout()
        {
            Menu menu = new Menu();
            menu.Menulist = GenerateShipWiseMenu2();
            return PartialView("_ShipWise_Menu_Layout", menu);
        }

        #region Generate Menu--

        public string GenerateShipWiseMenu()
        {
            string userType = Session["UserType"].ToString();
            string UserRole = Session["Role"].ToString();
            string shipId = Session["ShipId"].ToString();   
            ManualBL manuBl = new ManualBL();


            //string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");//Commented on 30th Apr 2021
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS1.xml");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {
                if (userType == "1")
                {
                    sb.Append("<div class='dropdown-content' style='min-width: 300px; top: -45px; '>");
                }
                else
                {
                    sb.Append("<div class='dropdown-content' style='min-width: 300px; top: -300px; '>");

                }
                sb.Append("\n");
                sb.Append("<ul>");//---ul1
                foreach (XmlNode ship in node)
                {
                    Volume vol          = new Volume();
                    //string filename     = ship.InnerText.ToString();
                    string sName        = ship.Attributes["name"].Value.ToString();
                    string ctrlName     = ship.Attributes["controllername"].Value.ToString();
                    string partName     = sName.Split(' ').Last();
                    string sNo          = ship.Attributes["shipnumber"].Value.ToString();
                    //vol = manuBl.GetVolumeById(sNo);
                    sb.Append("\n");
                    //sb.Append("<li class='mainmenu'><a href='#'><span class='vul'>Volume <b>I</b> </span><span class='pgnam'>" + volName + "</span></a>");
                    //string s= "'@Url.Action('"
                    //sb.Append("<li class='mainmenu'><a href='@Url.Action('Index', '"+vol.ControllerName+"')'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                    //sb.Append("<li class='dropmenuright' ><a href='/" + vol.ControllerName + "/Index'> "+ vol.Description +"</a>");///------------li1
                   
                    if(userType == "1")
                    {
                        if (shipId == sNo)
                        {
                            //sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Index'> " + sName + "</a>");///------------li1
                            //-----------------------Commented on 19th Jun 2021----------------------------------------------------
                            //sb.Append("<li class='dropmenuright' ><a href='/Dashboard/ShipDashboard/" + sNo + "'> " + sName + "</a>");
                            //sb.Append("</li>");///----End--------li1

                            
                            //----------------Added on 19th jun 2021-----------------------------------------------------------------------------
                            if(sNo == "1") //this if/else condition was applied due to difference in actionname in xml if resolved in xml then no need below condition
                                sb.Append("<li class='dropmenuright' ><a href='/"+ ctrlName + "/Pages?actionName=STSP2SP&fileName=Ship\'s Particulars' > " + sName + "</a>");
                            else
                                sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSSP&fileName=Ship\'s Particulars' > " + sName + "</a>");
                            sb.Append("</li>");///----End--------li1
                        }

                    }
                    else
                    {


                        //sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Index'> " + sName + "</a>");///------------li1

                        //-----------------------Commented on 19th Jun 2021----------------------------------------------------
                        //sb.Append("<li class='dropmenuright' ><a href='/Dashboard/ShipDashboard/"+ sNo + "'> " + sName + "</a>");

                        //----------------Added on 19th jun 2021-----------------------------------------------------------------------------
                        if (sNo == "1") //this if/else condition was applied due to difference in actionname in xml if resolved in xml then no need below condition
                            sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSP2SP&fileName=Ship\'s Particulars' > " + sName + "</a>");
                        else
                            sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSSP&fileName=Ship\'s Particulars' > " + sName + "</a>");

                        #region Line Commented on 23rd Feb 2021
                        //sb.Append("\n");
                        //sb.Append("<ul class='dropdown-rightcontent'>");///------------ul2
                        //foreach (XmlNode item in volume)
                        //{
                        //    Manual manual = new Manual();
                        //    int l = 1;

                        //    if (item.Name == "filename")
                        //    {
                        //        string filename = item.InnerText.ToString();
                        //        manual = manuBl.GetActionNameByFileName(filename + ".html");
                        //        if (manual.ActionName != null)
                        //        {
                        //            sb.Append("\n");
                        //            //sb.Append("<li><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + "</span></a></li>");
                        //            //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                        //            //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                        //            ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                        //            sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                        //            sb.Append( filename + "</a></li>");


                        //        }

                        //    }
                        //    else if (item.Name == "foldername")
                        //    {

                        //        string fName = item.Attributes["name"].Value.ToString();
                        //        sb.Append("\n");
                        //        sb.Append("<li class='dropmenurightL"+ l+"'><a href='#'>" + fName + "</a>");
                        //        sb.Append("\n");
                        //        sb.Append("<ul class='dropdown-rightcontentL"+ l +"'>");
                        //        string sChild = GetChild(item, ref l);
                        //        //l = l;
                        //        sb.Append(sChild);
                        //        sb.Append("\n");
                        //        sb.Append("</ul>");
                        //        sb.Append("\n");
                        //        sb.Append("</li>");
                        //    }



                        //}
                        //sb.Append("\n");
                        //sb.Append("</ul>");///-----End-------ul2
                        //sb.Append("\n");
                        #endregion
                        sb.Append("</li>");///----End--------li1
                    }


                }
                sb.Append("\n");
                sb.Append("</ul>");//--End--ul1
                sb.Append("\n");
                sb.Append("</div>");
                //WriteToText(sb);

            }
            return sb.ToString();
        }

        public string GenerateShipWiseMenu2()
        {
            string userType = Session["UserType"].ToString();
            string UserRole = Session["Role"].ToString();
            string shipId = Session["ShipId"].ToString();
            ManualBL manuBl = new ManualBL();


            //string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");//Commented on 30th Apr 2021
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS1.xml");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {
                if (userType == "1")
                {
                    sb.Append("<div class='dropdown-content' style='min-width: 300px; top: -45px; '>");
                }
                else
                {
                    sb.Append("<div class='dropdown-content' style='min-width: 300px; top: -300px; '>");

                }
                sb.Append("\n");
                sb.Append("<ul>");//---ul1
                foreach (XmlNode ship in node)
                {
                    Volume vol = new Volume();
                    string sName = ship.Attributes["name"].Value.ToString();
                    string ctrlName = ship.Attributes["controllername"].Value.ToString();
                    string partName = sName.Split(' ').Last();
                    string sNo = ship.Attributes["shipnumber"].Value.ToString();
                    //vol = manuBl.GetVolumeById(sNo);
                    sb.Append("\n");
                   
                    if (UserRole == "OfficeAdmin" || UserRole == "ShipAdmin")
                    {
                        if (userType == "1")
                        {
                            if (shipId == sNo)
                            {
                                //----------------Added on 19th jun 2021-----------------------------------------------------------------------------
                                if (sNo == "1") //this if/else condition was applied due to difference in actionname in xml if resolved in xml then no need below condition
                                    sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSP2SP&fileName=Ship Particulars'> " + sName + "</a>");
                                else
                                    sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSSP&fileName=Ship Particulars'> " + sName + "</a>");
                                sb.Append("</li>");///----End--------li1
                            }

                        }
                        else
                        {
                            //----------------Added on 19th jun 2021-----------------------------------------------------------------------------
                            if (sNo == "1") //this if/else condition was applied due to difference in actionname in xml if resolved in xml then no need below condition
                                sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSP2SP&fileName=Ship Particulars'> " + sName + "</a>");
                            else
                                sb.Append("<li class='dropmenuright' ><a href='/" + ctrlName + "/Pages?actionName=STSSP&fileName=Ship Particulars'> " + sName + "</a>");

                            sb.Append("</li>");///----End--------li1
                        }

                    }
                    else
                    {
                        if (shipId == sNo && userType == "1")
                        {

                            sb.Append("<li class='dropmenuright' ><a href='/Dashboard/ShipDashboard/" + sNo + "'> " + sName + "</a>");
                            sb.Append("</li>");///----End--------li1

                        }
                        else
                        {
                            sb.Append("<li class='dropmenuright' ><a href='/Dashboard/ShipDashboard/" + sNo + "'> " + sName + "</a>");
                            sb.Append("</li>");///----End--------li1
                        }

                    }


                }
                sb.Append("\n");
                sb.Append("</ul>");//--End--ul1
                sb.Append("\n");
                sb.Append("</div>");
                //WriteToText(sb);

            }
            return sb.ToString();
        }


        public string GetChild(XmlNode node, ref int l)
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
                    if (manual.ActionName != null)
                    {
                        sb.Append("\n");
                        //sb.Append("<li ><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                        sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                        sb.Append(filename + " </a></li>");
                    }

                }
                else if (item.Name == "foldername")
                {
                    int x = 0;
                    string fName = item.Attributes["name"].Value.ToString();
                    sb.Append("\n");

                    sb.Append("<li class='dropmenurightL"+ l +"'>" + fName + "</a>");
                    sb.Append("\n");
                    sb.Append("<ul class='dropdown-rightcontentL"+l+"'>");
                    x = l + 1;
                    string sChild = GetChild(item, ref x);
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
    }
}