using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class RevisionHeader
    {
        public int Id { get; set; }
        public string RevisionNo { get; set; }
        public string RevisionDate { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public List<RevisionHistory> RevisionHistoryList { get; set; }

    }
}
