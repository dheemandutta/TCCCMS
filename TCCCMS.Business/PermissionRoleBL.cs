using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
using TCCCMS.Models;
using TCCCMS.Data;

namespace TCCCMS.Business
{
    public class PermissionRoleBL
    {
        public int SavePermissionRole(PermissionRolePOCO pOCO /*,int VesselID*/)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.SavePermissionRole(pOCO/*, VesselID*/);
        }

        public int DeletePermissionRole(int PermissionRoleId/*, ref string oUTPUT*/)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.DeletePermissionRole(PermissionRoleId/*, ref oUTPUT*/);
        }
    }
}
