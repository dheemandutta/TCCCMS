using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCCCMS.Business
{
    class ShipManualBL
    {
        public Manual GetManual(string controllerName, string actionName)
        {
            Manual file = new Manual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetManual(controllerName, actionName);
            return file;
        }
    }
}
