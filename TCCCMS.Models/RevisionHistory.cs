using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;

namespace TCCCMS.Models
{
    public class RevisionHistory
    {
        public int ID { get; set; }
        public string FormName { get; set; }
        public string ModifiedSection { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedOn1 { get; set; }
        public string Version { get; set; }


        public int FormId { get; set; }



        public string Chapter { get; set; }
        public string Section { get; set; }
        public string ChangeComment { get; set; }


        //public DateTime ModificationDate { get; set; }
        public string ModificationDate { get; set; }
    }
}
