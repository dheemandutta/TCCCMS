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
    public class RankBL
    {
        public int SaveUpdateUser(UserMasterPOCO pOCO /*,int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.SaveUpdateUser(pOCO/*, VesselID*/);
        }

        public List<UserMasterPOCO> GetAllUserPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllUserPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public UserMasterPOCO GetUserByUserId(int UserId/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetUserByUserId(UserId/*, VesselID*/);
        }

        public int DeleteUserMaster(int UserId/*, ref string oUTPUT*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.DeleteUserMaster(UserId/*, ref oUTPUT*/);
        }

    }
}
