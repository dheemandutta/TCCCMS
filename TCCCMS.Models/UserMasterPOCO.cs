using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{

    public class UserMasterPOCO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOn1 { get; set; }
        public int IsActive { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Gender { get; set; }
        public string VesselIMO { get; set; }
        public int RankId { get; set; }
        public string RankName { get; set; }//May not reqiured @BK



        public int ShipId { get; set; }
        public string ShipName { get; set; }//May not reqiured @BK



        public string UserCode { get; set; }
        public int UserType { get; set; }
        public int IsAdmin { get; set; }

        public RankPOCO Rank { get; set; } //--Added on 20th JAN 2021 @BK
        public Ship Ship { get; set; }//--Added on 20th JAN 2021 @BK

        public int UploadPermission { get; set; }
        
    }
}
