﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class RoleMasterPOCO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
