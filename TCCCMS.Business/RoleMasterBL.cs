using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
using TCCCMS.Models;
using TCCCMS.Data;

namespace TCCCMS.Business
{
    public class RoleMasterBL
    {
        public int SaveUpdateRoleMaster(RoleMasterPOCO pOCO /*,int VesselID*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.SaveUpdateRoleMaster(pOCO/*, VesselID*/);
        }

        public int DeleteRoleMaster(int RoleId/*, ref string oUTPUT*/)
        {
            RoleMasterDAL dAL = new RoleMasterDAL();
            return dAL.DeleteRoleMaster(RoleId/*, ref oUTPUT*/);
        }
    }
}
