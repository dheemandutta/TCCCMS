using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
    public class HomeBL
    {

        #region Login
        public UserMasterPOCO CheckUserLogin(UserMasterPOCO aUser, ref string asReturnMessage)
        {
            HomeDAL homeDal = new HomeDAL();
            return homeDal.CheckUserLogin(aUser, ref asReturnMessage);
        }
            #endregion
        }
}
