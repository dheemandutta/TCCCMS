using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class SearchDataModel
    {
        public List<SelectListItem> ListItems { get; set; }
        public int SelectedListItem { get; set; }
    }
}