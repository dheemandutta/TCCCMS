using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCCCMS.Models
{

    public class UserMasterPOCO
    {

        private int _hasChange = 0;
        public int UserId { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
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

        //--Below 3 Properties Added on 13th Jul 2021 @BK
        public RoleGroup RoleGroup { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }//May not reqiured @BK

        public int UploadPermission { get; set; }

        public int RoleType { get; set; }

        public int IsApprover { get; set; }

        /// <summary>
        /// when need Change password
        /// </summary>

        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }//--Added on 20th JAN 2021 @BK

        public int hasChange {get;set;  }

        public int IsAllowSign { get; set; }
    }
}
