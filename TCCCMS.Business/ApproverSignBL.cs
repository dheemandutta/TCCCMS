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
    public class ApproverSignBL
    {
        public int SaveApproverSign(ApproverMaster pOCO)
        {
            ApproverSignDAL dAL = new ApproverSignDAL();
            return dAL.SaveApproverSign(pOCO);
        }

        public ApproverMaster GetAllApproverSign(int ApproverUserId, string uploadedFormName = null)
        {
            ApproverSignDAL dAL = new ApproverSignDAL();
            return dAL.GetAllApproverSign(ApproverUserId, uploadedFormName);
        }

        //for ApproverSignUser drp
        public List<ApproverMaster> GetAllUserForDrpApproverSign(/*int VesselID*/)
        {
            ApproverSignDAL dAL = new ApproverSignDAL();
            return dAL.GetAllUserForDrpApproverSign(/*VesselID*/);
        }
    }
}
