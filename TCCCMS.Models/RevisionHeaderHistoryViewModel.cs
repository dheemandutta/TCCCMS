using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class RevisionHeaderHistoryViewModel
    {
        public RevisionHeader RevisionHeader { get; set; }
        public RevisionHistory RevisionHistory { get; set; }

        public List<RevisionHeader> RevisionHeaderList { get; set; }

        public List<RevisionHistory> RevisionHistoryList { get; set; }

        public RevisionViewer RevisionViewer { get; set; }
        public List<RevisionViewer> RevisionViewers { get; set; }

    }
}
