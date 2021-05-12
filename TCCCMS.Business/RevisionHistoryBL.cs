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

        public int SaveRevisionHistory(RevisionHistory pOCO /*,int VesselID*/)
        {
            RevisionHistoryDAL dAL = new RevisionHistoryDAL();
            return dAL.SaveRevisionHistory(pOCO/*, VesselID*/);
        }

        public RevisionHeaderHistoryViewModel GetAllRevisionDetails()
        {
            RevisionHistoryDAL rhDal = new RevisionHistoryDAL();

            return rhDal.GetAllRevisionDetails();

        }
        public List<RevisionHeader> GetRevisionHeaderForDrp()
        {
            RevisionHistoryDAL rhDal = new RevisionHistoryDAL();

            return rhDal.GetRevisionHeaderForDrp();
        }
        public int SaveRevisionHeader(RevisionHeader rHeader )
        {
            RevisionHistoryDAL rhDal = new RevisionHistoryDAL();
            return rhDal.SaveRevisionHeader(rHeader);
        }

        public int SaveRevisionViewer(RevisionViewer rViewer)
        {
            RevisionHistoryDAL rhDal = new RevisionHistoryDAL();
            return rhDal.SaveRevisionViewer(rViewer);
        }

        public List<RevisionViewer> GetAllRevisionViewers(int revisionId)
        {
            RevisionHistoryDAL rhDal = new RevisionHistoryDAL();

            return rhDal.GetAllRevisionViewers(revisionId);

        }
    }
}
