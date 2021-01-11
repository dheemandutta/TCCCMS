﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;


namespace TCCCMS.Models
{
    public class Forms
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public string FormName { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreateedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public int RowNumber { get; set; }


    }
}
