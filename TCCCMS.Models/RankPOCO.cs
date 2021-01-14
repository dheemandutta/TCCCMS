using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class RankPOCO
    {
        public int RankId { get; set; }
        public string RankName { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
    }
}
