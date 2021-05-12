using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class RevisionViewer
    {
        public int Id { get; set; }
        public int RevisionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RankId { get; set; }
        public string RankName { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; }

        
        public string CreatedAt { get; set; }
    }
}
