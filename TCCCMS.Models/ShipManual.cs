using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class ShipManual
    {
        public int Id { get; set; }
        public int ShipNo { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }

        public string BodyHeader { get; set; }
        public string BodyText { get; set; }
        public string BodyHtml { get; set; }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ShipName { get; set; }

        //public List<Volume> Volumes { get; set; }
        //[NotMapped]
        //public IEnumerable<SelectListItem> Ships { get; set; }

        //[NotMapped]
        //public List<String> ErrorMessage { get; set; }
        //[NotMapped]
        //public bool FileCheck { get; set; }
    }
}
