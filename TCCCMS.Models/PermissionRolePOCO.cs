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
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }



        public string PermissionName { get; set; }
        public string RoleName { get; set; }


        public string SelectedRoles { get; set; }
        public IList<KeyValuePair<string, string>> Roles { get; set; }
    }
}
