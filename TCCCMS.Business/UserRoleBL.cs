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
    public class UserRoleBL
    {
        public int GetUserRoleByUserID(int UserID/*, ref string oUTPUT*/)
        {
            UserRoleDAL dAL = new UserRoleDAL();
            return dAL.GetUserRoleByUserID(UserID/*, ref oUTPUT*/);
        }
    }
}
