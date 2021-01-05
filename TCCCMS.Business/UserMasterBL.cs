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
    public class UserMasterBL
    {
        public int SaveUpdateUser(UserMasterPOCO pOCO /*,int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.SaveUpdateUser(pOCO/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetAllUser(/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllUser(/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetUserByUserId(int UserId/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetUserByUserId(UserId/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetUserByIMO(string VesselIMO/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetUserByIMO(VesselIMO/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetUserByEmailId(string Email/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetUserByEmailId(Email/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetUserByRank(int RankId/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetUserByRank(RankId/*, VesselID*/);
        }

        public int DeleteUserMaster(int UserId/*, ref string oUTPUT*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.DeleteUserMaster(UserId/*, ref oUTPUT*/);
        }
    }
}