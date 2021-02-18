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
    public class RevisionHistoryBL
    {
        public List<RevisionHistory> GetRevisionHistoryPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            RevisionHistoryDAL dAL = new RevisionHistoryDAL();
            return dAL.GetRevisionHistoryPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public List<RevisionHistory> GetFormIdForModifiedSection()
        {
            RevisionHistoryDAL dAL = new RevisionHistoryDAL();
            return dAL.GetFormIdForModifiedSection();
        }
    }
}
