using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace TCCCMS.Infrastructure
{
    public class CustomAuthorizationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["Role"])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller  
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Home" },
                     { "action", "Login" }
                });
            }
        }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext != null)
        //    {
        //        HttpSessionStateBase objHttpSessionStateBase = filterContext.HttpContext.Session;
        //        var userSession = objHttpSessionStateBase["Role"];
        //        if (((userSession == null) && (!objHttpSessionStateBase.IsNewSession)) || (objHttpSessionStateBase.IsNewSession))
        //        {
        //            objHttpSessionStateBase.RemoveAll();
        //            objHttpSessionStateBase.Clear();
        //            objHttpSessionStateBase.Abandon();
        //            if (filterContext.HttpContext.Request.IsAjaxRequest())
        //            {
        //                filterContext.HttpContext.Response.StatusCode = 403;
        //                filterContext.Result = new JsonResult { Data = "Session Expired" };
        //            }
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(
        //                                            new RouteValueDictionary {{ "Controller", "Home" },
        //                                                                     { "Action", "Login" } });
        //            }

        //        }


        //    }
        //}
    }
}