using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ApprovedFilledupFormAndApproverViewModel
    {
        public Forms ApprovedForm { get; set; }
        public UserMasterPOCO ApproverUser { get; set; }
        public FormsApproval ApprovalForm { get; set; }
        public List<Forms> ApprovedFormList { get; set; }
        public List<UserMasterPOCO> ApproverUserList { get; set; }
        public List<FormsApproval> ApproverList { get; set; }

    }

    public class FormsApproval
    {
        public int ApprovalId { get; set; }
        public int UploadedFormId { get; set; }
        public int IsApprove { get; set; }
        public string ApprovedOn { get; set; }
        public int ApproverUserId { get; set; }
        public string ApproverUserName { get; set; }

    }
}
