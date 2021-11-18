using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
     
    public class ApproverMasterBL
     {
        public List<ApproverMaster> GetAllApproverListPageWise(int pageIndex, ref int totalCount, int pageSize)
        {
            List<ApproverMaster> approverList = new List<ApproverMaster>();
            ApproverMasterDAL approverDal = new ApproverMasterDAL();
            approverList = approverDal.GetAllApproverListPageWise(pageIndex, ref totalCount, pageSize);

            return approverList;
        }
        public int SaveApprover(ApproverMaster approver)
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();

            return approverDal.SaveApprover(approver);

        }
        public int SaveApprover(List<ApproverMaster> approverList)
        {
            int totalRowAffected = 0;
            ApproverMasterDAL approverDal = new ApproverMasterDAL();
            foreach (ApproverMaster approver in approverList)
            {
                int rowAffected = 0;
                rowAffected = approverDal.SaveApprover(approver);

                totalRowAffected = totalRowAffected + rowAffected;
            }

            return totalRowAffected;

        }

        public int DeleteApprover(int approverMasterId)
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();

            return approverDal.DeleteApprover(approverMasterId);
        }

        public ApproverMaster GetApproverUserByApproverUserId(int UserId)
        {
            ApproverMasterDAL dAL = new ApproverMasterDAL();
            return dAL.GetApproverUserByApproverUserId(UserId);
        }


        #region --DropDown--
        public List<Ship> GetAllShipForDropDown()
        {
            ShipDAL shipDal = new ShipDAL();
            return shipDal.GetAllShipForDropDown();
        }
        public List<RankPOCO> GetAllRanksForDropDown()
        {
            RankDAL shipDal = new RankDAL();
            return shipDal.GetAllRanksForDropDown();
        }
        public List<UserMasterPOCO> GetAllUserForDropDown()
        {
            UserMasterDAL shipDal = new UserMasterDAL();
            return shipDal.GetAllUserListForDropDown();
        }
        public List<UserMasterPOCO> GetAllUserByShipForDropDown(int shipId)
        {
            UserMasterDAL shipDal = new UserMasterDAL();
            return shipDal.GetAllUserListByShipForDropDown(shipId);
        }

        public List<ApproverLevel> GetApproverLevelForDopDown()
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();
            return approverDal.GetApproverLevelForDopDown();
        }
        public List<UserMasterPOCO> GetApproverListByShipForDopDown(int shipId)
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();
            return approverDal.GetApproverListByShipForDopDown(shipId);
        }

        /// <summary>
        /// Added on 23th jul 2021 @bk
        /// </summary>
        /// <returns></returns>
        public List<ApproverMaster> GetApproverUserForDopDown(int shipNo)
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();
            return approverDal.GetApproverUserForDopDown(shipNo);
        }

        #endregion
    }
}
