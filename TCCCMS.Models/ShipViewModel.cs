using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ShipViewModel
    {
        public Ship Ship { get; set; }
        public List<Ship> ShipList { get; set; }
        public VesselType VesselType { get; set; }
        public VesselSubType VessleSubType { get; set; }
        public VesselSubSubType VesselSubSubType { get; set; }


        public List<VesselType> VesselTypeList { get; set; }
        public List<VesselSubType> VesselSubTypeList { get; set; }
        public List<VesselSubSubType> VesselSubSubTypeList { get; set; }

    }
}
