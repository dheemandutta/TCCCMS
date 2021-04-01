using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class GroupUser
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }




        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOn1 { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string VesselIMO { get; set; }
        public int RankId { get; set; }
        public string RankName { get; set; }//May not reqiured @BK
    }
}
