using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TCCCMS.Models;

namespace TCCCMS.Data
{
    public class RankDAL
    {
        public int SaveUpdateRank(RankPOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveUpdateRank", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RankName", pOCO.RankName.ToString());

            cmd.Parameters.AddWithValue("@Description", pOCO.Description.ToString());

            //cmd.Parameters.AddWithValue("@IsActive", pOCO.IsActive);

            if (pOCO.RankId > 0)
            {
                cmd.Parameters.AddWithValue("@RankId ", pOCO.RankId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RankId ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }



        public List<RankPOCO> GetAllRankPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<RankPOCO> pOList = new List<RankPOCO>();
            List<RankPOCO> equipmentsPO = new List<RankPOCO>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllRankPageWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", length);
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                    cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@VesselID", VesselID);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //prodPOList = Common.CommonDAL.ConvertDataTable<ProductPOCO>(ds.Tables[0]);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        pOList.Add(new RankPOCO
                        {
                            RankId = Convert.ToInt32(dr["RankId"]),
                            RankName = Convert.ToString(dr["RankName"]),
                            Description = Convert.ToString(dr["Description"])
                        });
                    }
                    recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }



        public RankPOCO GetRankByRankId(int RankId)
        {
            RankPOCO prodPOList = new RankPOCO();
            RankPOCO prodPO = new RankPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetRankByRankId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RankId", RankId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToRankByRankIdList(ds);
        }

        private RankPOCO ConvertDataTableToRankByRankIdList(DataSet ds)
        {
            RankPOCO pPOCOPC = new RankPOCO();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //RankPOCO pPOCOPC = new RankPOCO();

                    if (item["RankId"] != null)
                        pPOCOPC.RankId = Convert.ToInt32(item["RankId"].ToString());

                    if (item["RankName"] != System.DBNull.Value)
                        pPOCOPC.RankName = item["RankName"].ToString();

                    if (item["Description"] != System.DBNull.Value)
                        pPOCOPC.Description = item["Description"].ToString();

                    //pcList.Add(pPOCOPC);
                }
            }
            return pPOCOPC;
        }



        public int DeleteRank(int RankId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeleteRank", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RankId", RankId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

        #region DropDown
        public List<RankPOCO> GetAllRanksForDropDown()
        {/*--Added on 16th Jan 2021 @BK */
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllRanksForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<RankPOCO> ranksList = myTable.AsEnumerable().Select(m => new RankPOCO()
            {
                RankId = m.Field<int>("RankId"),
                RankName = m.Field<string>("RankName"),

            }).ToList();
            con.Close();
            ranksList.Add(new RankPOCO { RankId = -1,RankName = "Please Select One" });
            return ranksList;

        }

        #endregion

    }
}
