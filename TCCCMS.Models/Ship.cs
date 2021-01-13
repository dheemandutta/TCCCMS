using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class Ship
    {
        public int ID { get; set; }
        public string ShipName { get; set; }
        public int IMONumber { get; set; }
        public string FlagOfShip { get; set; }
        public int Regime { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime LastSyncDate { get; set; }
        public string ShipEmail1 { get; set; }
        public string ShipEmail2 { get; set; }
        public int CompanyId { get; set; }
        public int VesselTypeId { get; set; }
        public string VesselTypeName { get; set; }
        public int VesselSubTypeId { get; set; }
        public string VesselSubTypeName { get; set; }

        public int VesselSubSubTypeId { get; set; }
        public string VesselSubSubTypeName { get; set; }
        public string Voices1 { get; set; }
        public string Voices2 { get; set; }
        public string Fax1 { get; set; }
        public string Fax2 { get; set; }
        public string VOIP1 { get; set; }
        public string VOIP2 { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string CommunicationsResources { get; set; }
        public int HelicopterDeck { get; set; }
        public int HelicopterWinchingArea { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreateedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public int RowNumber { get; set; }

    }

    public class VesselType
    {
        public int ID { get; set; }
        public string Description { get; set; }

    }
    public class VesselSubType
    {
        public int ID { get; set; }
        public int VesselTypeId { get; set; }
        public string Description { get; set; }


    }
    public class VesselSubSubType
    {
        public int ID { get; set; }
        public int VesselSubTypeId { get; set; }
        public string Description { get; set; }

    }
}
