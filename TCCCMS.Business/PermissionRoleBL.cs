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

        public string GetAllCommaSeperatedPermissionRoleByPermissionId(int permissionId)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.GetAllCommaSeperatedPermissionRoleByPermissionId(permissionId);

        }

        public int SavePermissionRole(int permissionId, string permissionRole)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.SavePermissionRole(permissionId, permissionRole);
        }


        //for permission drp
        public List<PermissionRolePOCO> GetAllPermissionForDrp(/*int VesselID*/)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.GetAllPermissionForDrp(/*VesselID*/);
        }

        //for Role drp
        public List<PermissionRolePOCO> GetAllRolesForDrp(/*int VesselID*/)
        {
            PermissionRoleDAL dAL = new PermissionRoleDAL();
            return dAL.GetAllRoleForDrp(/*VesselID*/);
        }

    }
}
