using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ApproverMaster
    {
        public int ID { get; set; }
        public int VesselIMONumber { get; set; }
        public int ShipId { get; set; }
        public int RankId { get; set; }
        public int UserId { get; set; }//--Approver Id
        public string UserName { get; set; }
        public int ApproverId { get; set; }//--Approver Level id
        public string ApproverDescription { get; set; }
        public string IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreateedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public int RowNumber { get; set; }

        public Ship Ship { get; set; }
        public RankPOCO Rank { get; set; }
        public UserMasterPOCO User { get; set; }

        //Added on 19th Jul 2021 @BK 
        public int ApprovedCount { get; set; }





        public int Id { get; set; }//--Approver Id
        public int ApproverUserId { get; set; }//--Approver Level id
        public string SignImagePath { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string CreatedOn1 { get; set; }
        public string ModifiedOn1 { get; set; }

        public int SL { get; set; }

        public enum ApproverTask
        {
            Approve,
            Review 
        }
    }

    public class ApproverLevel
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
