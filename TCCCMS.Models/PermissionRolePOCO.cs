using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class PermissionRolePOCO
    {
        public int PermissionRoleId { get; set; }
        public int PermssionId { get; set; }
        public int RoleId { get; set; }
        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
