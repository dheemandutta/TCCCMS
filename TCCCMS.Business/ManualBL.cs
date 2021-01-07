using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
    public class ManualBL
    {
        
        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId,string searchText)
        {
            List<Manual> manualsList = new List<Manual>();

            ManualDAL manualDAL = new ManualDAL();

            manualsList = manualDAL.SearchManuals(pageIndex, ref totalCount, pageSize, volumeId,searchText);

            return manualsList;
        }

        public Manual GetManual(string controllerName, string actionName)
        {
            Manual file = new Manual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetManual(controllerName,  actionName);
            return file;
        }
    }
}
