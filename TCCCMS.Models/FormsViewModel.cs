using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class FormsViewModel
    {
        public Forms Forms { get; set; }
        public FormsCategory FormsCategory { get; set; }
        public List<FormsCategory> CategoryList { get; set; }
        public List<Forms> FormsList { get; set; }
    }
}
