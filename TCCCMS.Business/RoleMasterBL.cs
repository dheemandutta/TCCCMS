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
    public class RoleMasterBL
    {
        public int SaveUpdateRoleMaster(RoleMasterPOCO pOCO /*,int VesselID*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.SaveUpdateRoleMaster(pOCO/*, VesselID*/);
        }

        public List<RoleMasterPOCO> GetAllRoleMasterPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.GetAllRoleMasterPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public RoleMasterPOCO GetRoleMasterByRoleId(int RoleId/*, int VesselID*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.GetRoleMasterByRoleId(RoleId/*, VesselID*/);
        }



        public int DeleteRoleMaster(int RoleId/*, ref string oUTPUT*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.DeleteRoleMaster(RoleId/*, ref oUTPUT*/);
        }

        public List<RoleGroup> GetAllRoles()
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.GetAllRoles();
        }

        public int SaveRoleGroup(RoleGroup pOCO)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.SaveRoleGroup(pOCO);
        }

        public RoleMasterPOCO GetRoleByGroupId(int GroupId)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.GetRoleByGroupId(GroupId);
        }
    }
}
