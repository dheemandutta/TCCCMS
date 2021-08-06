using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class DownloadableFroms
    {
        public int ID { get; set; }
        public string FormName { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }

        public string IsUpload { get; set; }

        public int CategoryId { get; set; }
    }
}
