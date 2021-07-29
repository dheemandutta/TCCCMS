using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TCCCMS
{
    public static class BreadCrumbHtmlExtensions
    {
        public static string BuildBreadcrumbNavigation(this HtmlHelper helper)
        {
            string userType = HttpContext.Current.Session["UserType"].ToString();
            string UserRole = HttpContext.Current.Session["Role"].ToString();
            string ShipId = null;
            if(HttpContext.Current.Session["DashboardShipId"].ToString() != null | HttpContext.Current.Session["DashboardShipId"].ToString() != "0")
            {
                ShipId = HttpContext.Current.Session["DashboardShipId"].ToString();
            }
            else if (HttpContext.Current.Session["ShipId"].ToString() != null | HttpContext.Current.Session["ShipId"].ToString() != "0")
            {
                ShipId = HttpContext.Current.Session["ShipId"].ToString();
            }

            // optional condition: I didn't wanted it to show on home and account controller
            if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
            {
                return string.Empty;
            }
            #region Not Used Old
            StringBuilder breadcrumb = new StringBuilder("<ol class='breadcrumb'><li>").Append(helper.ActionLink("Home", "Index", "Home").ToHtmlString()).Append("</li>");
            breadcrumb.Append("<li>");
            //--- Titleize() makes all characters to lower case of a single word except First Chracter
            //breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
            //                                  "Index",
            //                                  helper.ViewContext.RouteData.Values["controller"].ToString()));
            if(ShipId !=null)
            {
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                              "ShipDashboard",
                                              "Dashboard", new { id = ShipId }));
            }
            else
            {
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                              "Index",
                                              helper.ViewContext.RouteData.Values["controller"].ToString()));
            }
            
            breadcrumb.Append("</li>");
            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                breadcrumb.Append("<li>");
                //--- Titleize() makes all characters to lower case of a single word except First Chracter
                //breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                //                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                //                                    helper.ViewContext.RouteData.Values["controller"].ToString()));

                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString(),
                                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                                                    helper.ViewContext.RouteData.Values["controller"].ToString()));

                breadcrumb.Append("</li>");
            }
            #endregion
            ///------------------------------------------------------------------------------
            //StringBuilder breadcrumb2 = new StringBuilder("<div class='steps_item'><span class='steps_content'>").Append(helper.ActionLink("Home", "Index", "Home").ToHtmlString()).Append("</span></div>");
            StringBuilder breadcrumb2 = new StringBuilder();

            #region Home Part..
            breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
            if(UserRole == "OfficeUser" || UserRole == "ShipUser")
            {
                breadcrumb2.Append(helper.ActionLink("Home", "UserDashboard", "Dashboard").ToHtmlString());
                breadcrumb2.Append("</span></div>");

                if (UserRole == "OfficeUser")
                {
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    breadcrumb2.Append(helper.ActionLink("Office",
                                              "UserDashboard",
                                              "Dashboard").ToHtmlString());
                    breadcrumb2.Append("</span></div>");
                }
                else if (UserRole == "ShipUser")
                {
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    breadcrumb2.Append(helper.ActionLink("Ship",
                                              "UserDashboard",
                                              "Dashboard").ToHtmlString());
                    breadcrumb2.Append("</span></div>");
                }

            }
            else
            {
                breadcrumb2.Append(helper.ActionLink("Home", "AdminDashboard", "Dashboard").ToHtmlString());
                breadcrumb2.Append("</span></div>");
            }

            #endregion

            #region Controller Part..

           // breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");

            string controller = helper.ViewContext.RouteData.Values["controller"].ToString();
            if (controller == "Dashboard")
            {
                //breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                if (UserRole == "OfficeUser" || UserRole == "ShipUser")
                {
                    //breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    //--- Titleize() makes all characters to lower case of a single word except First Chracter
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                    //                           "UserDashboard",
                    //                           helper.ViewContext.RouteData.Values["controller"].ToString()));

                    //if (ShipId != null)
                    //{
                    //    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                                  "ShipDashboard",
                    //                                  "Dashboard", new { id = ShipId }));
                    //}
                    //else
                    //{
                    //    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                          "UserDashboard",
                    //                          helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    //}

                    //--------------------------Commented on 19th Jun 2021---------------------------------------------------
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                          "UserDashboard",
                    //                          helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());


                    //----------------------Changed on 19th Jun 2021--------------------------commented on 28th jun------------------------------
                    //if (UserRole == "OfficeUser")
                    //{
                    //    breadcrumb2.Append(helper.ActionLink("Office",
                    //                              "UserDashboard",
                    //                              helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    //}
                    //else if (UserRole == "ShipUser")
                    //{
                    //    breadcrumb2.Append(helper.ActionLink("Ship",
                    //                              "UserDashboard",
                    //                              helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    //}

                    //breadcrumb2.Append("</span></div>");
                }
                else
                {
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    //--- Titleize() makes all characters to lower case of a single word except First Chracter
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                    //                                               "AdminDashboard",
                    //                                               helper.ViewContext.RouteData.Values["controller"].ToString()));

                    //if (ShipId != null)
                    //{
                    //    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                                  "ShipDashboard",
                    //                                  "Dashboard", new { id = ShipId }).ToHtmlString());
                    //}
                    //else
                    //{

                    //    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                                                   "AdminDashboard",
                    //                                                   helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    //}


                    //--------------------------Commented on 19th Jun 2021---------------------------------------------------
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                                                  "AdminDashboard",
                    //                                                  helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());

                    //----------------------Changed on 19th Jun 2021--------------------------------------------------------
                    if (UserRole == "OfficeAdmin")
                    {
                        breadcrumb2.Append(helper.ActionLink("Office",
                                                  "AdminDashboard",
                                                  helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    }
                    else if (UserRole == "ShipAdmin")
                    {
                        breadcrumb2.Append(helper.ActionLink("Ship",
                                                  "AdminDashboard",
                                                  helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                    }
                    
                    breadcrumb2.Append("</span></div>");
                }

                //breadcrumb2.Append("</span></div>");
            }
            else
            {
                breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");

                //--- Titleize() makes all characters to lower case of a single word except First Chracter
                //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                //                               "Index",
                //                               helper.ViewContext.RouteData.Values["controller"].ToString()));

                if (controller == "ShipKHKVision" || controller == "ShipKWKExcelsus" || controller == "ShipCSKVanguard" || controller == "ShipCSKValiant" || controller == "ShipCSKEndeavour" || controller == "ShipKHKEmpress" || controller == "ShipKHKMajesty")
                {
                    //// -A- commented on 19th jun 2021 @BK and add B----------------------------------------
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                    //                                 "ShipDashboard",
                    //                                 "Dashboard"));

                    
                    ////--B---Added on 19th jun 2021 @BK because Admin user can not see other manuals(ship/vol)
                    if (UserRole == "OfficeAdmin" || UserRole == "ShipAdmin")
                    {
                        breadcrumb2.Append(helper.ViewContext.RouteData.Values["controller"].ToString());
                    }
                    else
                    {
                        breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                                     "ShipDashboard",
                                                     "Dashboard"));
                    }




                    ////----Test1---------------
                    //breadcrumb2.Append(helper.ActionLink("Bingshu",
                    //                                 "ShipDashboard",
                    //                                 "Dashboard"));

                    ////----Test1---------------
                    //breadcrumb2.Append("< a href = '/Dashboard/ShipDashboard/" + ShipId + "' >");
                    //breadcrumb2.Append(controller + "</ a >");
                }
                else
                {
                    if(controller == "NoticeBoard")
                    {
                        if(ShipId != null | ShipId != "0")
                            breadcrumb2.Append("<a href = \"/Dashboard/ShipDashboard/" + ShipId + "\">");
                        else
                            breadcrumb2.Append("<a href = '/Dashboard/UserDashboard'>");

                        breadcrumb2.Append(controller + "</a>");
                    }
                    else
                        breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                              "Index",
                                              helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());
                }

                //--------------test
                //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                //                             "Index",
                //                              "Ship").ToHtmlString());
                //-------------test end

                breadcrumb2.Append("</span></div>");
            }


            //breadcrumb2.Append("</span></div>");

            #endregion

            #region Action Part..
            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                if(helper.ViewContext.RouteData.Values["action"].ToString() == "Pages")
                {
                    

                    //string manualFileAction = HttpContext.Current.Session["ManualFileActionName"].ToString();
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    string s = helper.ViewContext.RouteData.Values["action"].ToString();
                    string fAction = helper.ViewContext.HttpContext.Request.QueryString["actionName"];
                    string fName = helper.ViewContext.HttpContext.Request.QueryString["fileName"]??  helper.ViewContext.HttpContext.Request.QueryString["formName"];// null coalescing  added on 29th jul 2021
                    //s = s + "?actionName=" + manualFileAction;
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                    //                                    s,
                    //                                    //helper.ViewContext.RouteData.Values["action"].ToString() + "?actionName=" + manualFileAction,
                    //                                    helper.ViewContext.RouteData.Values["controller"].ToString()));
                    
                    //breadcrumb2.Append(helper.ViewContext.RouteData.Values["action"].ToString());

                    breadcrumb2.Append(fName.CheckStringLenghtAndGetFirstFewCharecters(30));

                    breadcrumb2.Append("</span></div>");
                }
                else
                {
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                    //                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                    //                                    helper.ViewContext.RouteData.Values["controller"].ToString()));

                    breadcrumb2.Append(helper.ViewContext.RouteData.Values["action"].ToString());

                    breadcrumb2.Append("</span></div>");
                }
                
            }

            #endregion

            //return breadcrumb.Append("</ol>").ToString();
            return breadcrumb2.ToString();
        }
    }
}