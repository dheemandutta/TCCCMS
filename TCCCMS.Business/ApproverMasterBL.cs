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
        public int SaveApprever(ApproverMaster approver)
        {
            ApproverMasterDAL approverDal = new ApproverMasterDAL();

            return approverDal.SaveApprover(approver);

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

        #endregion
    }
}
