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
    public class UserGroupBL
    {
        public int SaveUpdateUserGroup(UserGroupPOCO groupMaster /*,int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.SaveUpdateUserGroup(groupMaster/*, VesselID*/);
        }

        public List<UserGroupPOCO> GetAllUserGroupByUserID(int UserId/*, int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllUserGroupByUserID(UserId/*, VesselID*/);
        }

        public int DeleteUserGroup(int GroupId/*, ref string oUTPUT*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.DeleteUserGroup(GroupId/*, ref oUTPUT*/);
        }
    }
}
