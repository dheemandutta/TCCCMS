using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class SearchDataModel
    {
        public List<SelectListItem> ListItems { get; set; }
        public int SelectedListItem { get; set; }
    }
}