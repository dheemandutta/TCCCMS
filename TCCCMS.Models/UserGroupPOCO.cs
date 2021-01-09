using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class UserGroupPOCO
    {
        public int UserGroupId { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string SelectedGroup { get; set; }

        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }



        public IList<KeyValuePair<string,string>> Groups { get; set; }
    }
}
