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

        public List<UserMasterPOCO> GetAllUserPageWise(int pageIndex, ref int recordCount, int length, int UserType)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllUserPageWise(pageIndex, ref recordCount, length, UserType);
        }

        public List<GroupUser> GetAllUser(/*, int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllUser(/*, VesselID*/);
        }

        public UserMasterPOCO GetUserByUserId(int UserId/*, int VesselID*/)
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

        public int ApprovedRoNotInUserMaster(int UserId/*, ref string oUTPUT*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.ApprovedRoNotInUserMaster(UserId/*, ref oUTPUT*/);
        }

        public int UploadPermissionUserMaster(int UserId/*, ref string oUTPUT*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.UploadPermissionUserMaster(UserId/*, ref oUTPUT*/);
        }

        public string GenerateUserCode(string asUserType, string asShipId, string asRankId, string asUserName)
        {
            UserMasterDAL userDal = new UserMasterDAL();

            return userDal.GenerateUserCode(asUserType, asShipId, asRankId, asUserName);
        }

        #region Dropdown
        //for Ranks drp
        public List<UserMasterPOCO> GetAllRanksForDrp(/*int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllRanksForDrp(/*VesselID*/);
        }
        //for Ship drp
        public List<UserMasterPOCO> GetAllShipForDrp(/*int VesselID*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetAllShipForDrp(/*VesselID*/);
        }

        #endregion


        public string GetRoleByUserId(int UserId/*, ref string oUTPUT*/)
        {
            UserMasterDAL dAL = new UserMasterDAL();
            return dAL.GetRoleByUserId(UserId/*, ref oUTPUT*/);
        }
        /// <summary>
        /// Added on 13th Jul 2021 @BK
        /// </summary>
        /// <param name="pOCO"></param>
        /// <returns></returns>
        public int ChangePassword(UserMasterPOCO aUserMaster)
        {
            UserMasterDAL umDAL = new UserMasterDAL();
            return umDAL.ChangePassword(aUserMaster);
        }


    }
}