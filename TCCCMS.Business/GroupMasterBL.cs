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
        public int SaveUpdateGroupMaster(GroupMasterPOCO groupMaster /*,int VesselID*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.SaveUpdateGroupMaster(groupMaster/*, VesselID*/);
        }

        public List<GroupMasterPOCO> GetAllGroupMaster(/*, int VesselID*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.GetAllGroupMaster(/*, VesselID*/);
        }

        public int DeleteGroupMaster(int GroupId/*, ref string oUTPUT*/)
        {
            GroupMasterDAL dAL = new GroupMasterDAL();
            return dAL.DeleteGroupMaster(GroupId/*, ref oUTPUT*/);
        }
    }
}
