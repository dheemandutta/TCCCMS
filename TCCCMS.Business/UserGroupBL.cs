﻿using System;
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

        public List<UserGroupPOCO> GetAllUserGroupPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllUserGroupPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public List<UserGroupPOCO> GetAllUserGroupByUserID(int UserId/*, int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllUserGroupByUserID(UserId/*, VesselID*/);
        }


        public string GetAllCommaSeperatedUserGroupByUserId(int userId)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllCommaSeperatedUserGroupByUserId(userId);
            
        }

        public int SaveUserGroupMapping(int userId, string userGroupMapping)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.SaveUserGroupMapping(userId, userGroupMapping);
        }


            //for user drp
        public List<UserGroupPOCO> GetAllUserForDrp(/*int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllUserForDrp(/*VesselID*/);
        }

        //for group drp
        public List<UserGroupPOCO> GetAllGroupsForDrp(/*int VesselID*/)
        {
            UserGroupDAL dAL = new UserGroupDAL();
            return dAL.GetAllGroupsForDrp(/*VesselID*/);
        }
    }
}
