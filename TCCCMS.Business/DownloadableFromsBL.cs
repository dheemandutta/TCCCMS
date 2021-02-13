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
    public class DownloadableFromsBL
    {
        public List<DownloadableFroms> GetDownloadableFromsPageWise(int pageIndex, ref int recordCount, int length, int CategoryId)
        {
            DownloadableFromsDAL dAL = new DownloadableFromsDAL();
            return dAL.GetDownloadableFromsPageWise(pageIndex, ref recordCount, length, CategoryId);
        }
    }
}
