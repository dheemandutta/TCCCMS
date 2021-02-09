using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ManualViewModel
    {
        public string SearchText { get; set; }
        public Manual Manual { get; set; }
        public Pagination Pagination { get; set; }

        public List<Manual> ManualList { get; set; }
        public int VolumeId { get; set; }
    }
}
