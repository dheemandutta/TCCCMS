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
            if(HttpContext.Current.Session["ShipId"].ToString() != null)
            {
                ShipId = HttpContext.Current.Session["ShipId"].ToString();
            }
            else if(HttpContext.Current.Session["DashboardShipId"].ToString() != null)
            {
                ShipId = HttpContext.Current.Session["DashboardShipId"].ToString();
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
            breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
            if(UserRole == "OfficeUser" || UserRole == "ShipUser")
            {
                breadcrumb2.Append(helper.ActionLink("Home", "UserDashboard", "Dashboard").ToHtmlString());
                breadcrumb2.Append("</span></div>");
            }
            else
            {
                breadcrumb2.Append(helper.ActionLink("Home", "AdminDashboard", "Dashboard").ToHtmlString());
                breadcrumb2.Append("</span></div>");
            }
            

           
            breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");

            string controller = helper.ViewContext.RouteData.Values["controller"].ToString();
            if (controller == "Dashboard")
            {
                if(UserRole == "OfficeUser" || UserRole == "ShipUser")
                {
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

                    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                              "UserDashboard",
                                              helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());


                }
                else
                {
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

                    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                                                      "AdminDashboard",
                                                                      helper.ViewContext.RouteData.Values["controller"].ToString()).ToHtmlString());

                }
                
            }
            else
            {
                //--- Titleize() makes all characters to lower case of a single word except First Chracter
                //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                //                               "Index",
                //                               helper.ViewContext.RouteData.Values["controller"].ToString()));

                if (controller == "ShipKHKVision" || controller == "ShipKWKExcelsus" || controller == "ShipCSKVanguard" || controller == "ShipCSKValiant" || controller == "ShipCSKEndeavour" || controller == "ShipKHKEmpress" || controller == "ShipKHKMajesty")
                {
                    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString(),
                                                     "ShipDashboard",
                                                     "Dashboard"));

                    //breadcrumb2.Append(helper.ActionLink("Bingshu",
                    //                                 "ShipDashboard",
                    //                                 "Dashboard"));

                    //breadcrumb2.Append("< a href = '/Dashboard/ShipDashboard/" + ShipId + "' >");
                    //breadcrumb2.Append(controller + "</ a >");
                }
                else
                {
                    if(controller == "NoticeBoard")
                    {
                        if(ShipId != null)
                            breadcrumb2.Append("< a href = '/Dashboard/ShipDashboard/" + ShipId + "' >");
                        else
                        breadcrumb2.Append("< a href = '/Dashboard/UserDashboard' >");
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

            }


            breadcrumb2.Append("</span></div>");

            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                if(helper.ViewContext.RouteData.Values["action"].ToString() == "Pages")
                {
                    //string manualFileAction = HttpContext.Current.Session["ManualFileActionName"].ToString();
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    string s = helper.ViewContext.RouteData.Values["action"].ToString();
                    //s = s + "?actionName=" + manualFileAction;
                    //breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                    //                                    s,
                    //                                    //helper.ViewContext.RouteData.Values["action"].ToString() + "?actionName=" + manualFileAction,
                    //                                    helper.ViewContext.RouteData.Values["controller"].ToString()));
                    breadcrumb2.Append(helper.ViewContext.RouteData.Values["action"].ToString());

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

            //return breadcrumb.Append("</ol>").ToString();
            return breadcrumb2.ToString();
        }
    }
}