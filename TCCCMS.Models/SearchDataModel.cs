using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TCCCMS.Models
{
    public class SearchDataModel
    {
        public List<SelectListItem> ListItems { get; set; }
        public int SelectedListItem { get; set; }
    }
}
