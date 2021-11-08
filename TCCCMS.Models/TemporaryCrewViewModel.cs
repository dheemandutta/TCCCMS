using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class TemporaryCrewViewModel
    {
        public UserMasterPOCO TemporaryCrew { get; set; }
        public List<UserMasterPOCO> TemporaryCrewList { get; set; }
    }
}
