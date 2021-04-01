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
    public class GroupMasterBL
    {
        public List<GroupMasterPOCO> GetAllGroupMasterPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.GetAllGroupMasterPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public int SaveUpdateGroupMaster(GroupMasterPOCO groupMaster /*,int VesselID*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.SaveUpdateGroupMaster(groupMaster/*, VesselID*/);
        }

        public List<GroupUser> GetAllGroupMaster()
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.GetAllGroupMaster();
        }

        public List<RoleGroup> GetAllGroupsNotInRoles()
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.GetAllGroupsNotInRoles();
        }

            public GroupMasterPOCO GetGroupMasterByGroupId(int GroupId/*, int VesselID*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.GetGroupMasterByGroupId(GroupId/*, VesselID*/);
        }

        public int DeleteGroupMaster(int GroupId/*, ref string oUTPUT*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.DeleteGroupMaster(GroupId/*, ref oUTPUT*/);
        }
    }
}
