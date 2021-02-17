using TCCCMS.Models;
using TCCCMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace TCCCMS.Controllers
{
    public class RevisionHistoryController : Controller
    {
        // GET: RevisionHistory
        public ActionResult Index()
        {
            return View();
        }
    }
}