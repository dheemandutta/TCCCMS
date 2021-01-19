using TCCCMS.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace TCCCMS.Data
{
    public class ApproverMasterDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;
        public List<ApproverMaster> GetAllApproverListPageWise(int pageIndex, ref int totalCount, int pageSize)
        {
            List<ApproverMaster> approverList = new List<ApproverMaster>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllApproverListPageWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                    cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                    con.Open();

                    DataSet ds          = new DataSet();
                    SqlDataAdapter da   = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ApproverMaster approver     = new ApproverMaster();
                        approver.ID                 = Convert.ToInt32(dr["ID"]);
                        approver.ShipId             = Convert.ToInt32(dr["ShipId"]);
                        approver.RankId             = Convert.ToInt32(dr["RankId"]);
                        approver.UserId             = Convert.ToInt32(dr["UserId"]);
                        approver.ApproverId         = Convert.ToInt32(dr["ApproverId"]);
                        approver.VesselIMONumber    = Convert.ToInt32(dr["VesselIMONumber"]);

                        Ship ship                   = new Ship();
                        ship.ID                     = Convert.ToInt32(dr["ID"]);
                        ship.ShipName               = Convert.ToString(dr["ShipName"]);
                        approver.Ship               = ship;

                        RankPOCO rank               = new RankPOCO();
                        rank.RankId                 = Convert.ToInt32(dr["RankId"]);
                        rank.RankName               = Convert.ToString(dr["RankName"]);
                        approver.Rank               = rank;

                        UserMasterPOCO user         = new UserMasterPOCO();
                        user.UserId                 = Convert.ToInt32(dr["UserId"]);
                        user.UserName               = Convert.ToString(dr["UserName"]);
                        approver.User               = user; 

                        approver.ApproverDescription = Convert.ToString(dr["ApproverDescription"]);
                        approver.RowNumber          = Convert.ToInt32(dr["RowNumber"]);
                        

                        approverList.Add(approver);
                    }
                    totalCount = Convert.ToInt32(cmd.Parameters["@TotalCount"].Value);

                    con.Close();
                }
            }


            return approverList;

        }
        public int SaveApprover(ApproverMaster approver)
        {
            int recorSaved = 0;

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveApprover", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", approver.ID);
            cmd.Parameters.AddWithValue("@ShipId", approver.ShipId);
            cmd.Parameters.AddWithValue("@IMONumber", approver.VesselIMONumber);
            cmd.Parameters.AddWithValue("@RankId", approver.RankId);
            cmd.Parameters.AddWithValue("@UserId", approver.UserId);
            cmd.Parameters.AddWithValue("@ApproverId", approver.ApproverId);
            cmd.Parameters.AddWithValue("@CreatedBy", 1);
            recorSaved = cmd.ExecuteNonQuery();
            con.Close();


            return recorSaved;
        }
    }
}
