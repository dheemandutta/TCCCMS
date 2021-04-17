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
            
            // optional condition: I didn't wanted it to show on home and account controller
            if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
            {
                return string.Empty;
            }

            StringBuilder breadcrumb = new StringBuilder("<ol class='breadcrumb'><li>").Append(helper.ActionLink("Home", "Index", "Home").ToHtmlString()).Append("</li>");
            breadcrumb.Append("<li>");
            breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                              "Index",
                                              helper.ViewContext.RouteData.Values["controller"].ToString()));
            breadcrumb.Append("</li>");
            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                breadcrumb.Append("<li>");
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                                                    helper.ViewContext.RouteData.Values["action"].ToString(),
                                                    helper.ViewContext.RouteData.Values["controller"].ToString()));

                breadcrumb.Append("</li>");
            }

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

           
            if(helper.ViewContext.RouteData.Values["controller"].ToString() == "Dashboard")
            {
                if(UserRole == "OfficeUser" || UserRole == "ShipUser")
                {
                    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                               "UserDashboard",
                                               helper.ViewContext.RouteData.Values["controller"].ToString()));
                }
                else
                {
                    breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                                                   "AdminDashboard",
                                                                   helper.ViewContext.RouteData.Values["controller"].ToString()));
                }
                
            }
            else
            {
                breadcrumb2.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                               "Index",
                                               helper.ViewContext.RouteData.Values["controller"].ToString()));
            }
            

            breadcrumb2.Append("</span></div>");

            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                if(helper.ViewContext.RouteData.Values["action"].ToString() == "Pages")
                {
                   // string manualFileAction = HttpContext.Current.Session["ManualFileActionName"].ToString();
                    breadcrumb2.Append("<div class='steps_item'><span class='steps_content'>");
                    string s = helper.ViewContext.RouteData.Values["action"].ToString();
                   // s = s + "?actionName=" + manualFileAction;
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