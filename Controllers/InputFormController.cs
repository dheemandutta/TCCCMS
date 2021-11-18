using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class InputFormController : Controller
    {
        // GET: InputForm

        [CustomAuthorizationFilter]
        public ActionResult ManagementOfChange()
        {
            return View();
        }
    }
}