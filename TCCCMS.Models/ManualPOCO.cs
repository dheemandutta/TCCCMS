using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ManualPOCO
    {
        public int ManualId { get; set; }
        public int VolumeId { get; set; }
        public string ManualFileName { get; set; }
        public string ManualHtml { get; set; }
        public string ManualHeader { get; set; }
        public string ManualBodyText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }


        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}
