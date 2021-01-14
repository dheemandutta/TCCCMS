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
    public class RankBL
    {
        public int SaveUpdateRank(RankPOCO pOCO /*,int VesselID*/)
        {
            RankDAL dAL = new RankDAL();
            return dAL.SaveUpdateRank(pOCO/*, VesselID*/);
        }

        public List<RankPOCO> GetAllRankPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            RankDAL dAL = new RankDAL();
            return dAL.GetAllRankPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }

        public RankPOCO GetRankByRankId(int RankId/*, int VesselID*/)
        {
            RankDAL dAL = new RankDAL();
            return dAL.GetRankByRankId(RankId/*, VesselID*/);
        }

        public int DeleteRank(int RankId/*, ref string oUTPUT*/)
        {
            RankDAL dAL = new RankDAL();
            return dAL.DeleteRank(RankId/*, ref oUTPUT*/);
        }


    }
}
