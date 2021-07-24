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
            //cmd.Parameters.AddWithValue("@ApproverId", approver.ApproverId);
            cmd.Parameters.AddWithValue("@CreatedBy", 1);
            if (approver.ApproverId != 0)
            {
                cmd.Parameters.AddWithValue("@ApproverId", approver.ApproverId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ApproverId", 0);
            }
            recorSaved = cmd.ExecuteNonQuery();
            con.Close();


            return recorSaved;
        }

        public int DeleteApprover(int approverMasterId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DeleteApprover", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApproverMasterId", approverMasterId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

        /// <summary>
        /// Added on 23th jul 2021 @BK
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ApproverMaster GetApproverUserByApproverUserId(int UserId)
        {
           
            ApproverMaster approver = new ApproverMaster();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetApproverUserByUserId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApproverUserId", UserId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();

                    approver.ApproverUserId = Convert.ToInt32(ds.Tables[0].Rows[0]["ApproverUserId"].ToString());
                    approver.Position       = ds.Tables[0].Rows[0]["Position"].ToString();
                    approver.Name           = ds.Tables[0].Rows[0]["Name"].ToString();
                    approver.UserName       = ds.Tables[0].Rows[0]["UserName"].ToString();
                }
            }
            return approver;
        }


        #region --DropDown
        public List<UserMasterPOCO> GetApproverListByShipForDopDown(int shipId)
        {
            List<UserMasterPOCO> userByShipList = new List<UserMasterPOCO>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetApproverListByShipForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShipId", shipId);


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        userByShipList.Add(new UserMasterPOCO
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            UserCode = Convert.ToString(dr["UserCode"]),

                        });
                    }
                    con.Close();
                }
            }
            userByShipList.Add(new UserMasterPOCO { UserId = -1, UserName = "Please Select One" });
            return userByShipList;

        }

        public List<ApproverLevel> GetApproverLevelForDopDown()
        {
            List<ApproverLevel> approverLevelList = new List<ApproverLevel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetApproverLevelForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType     = CommandType.StoredProcedure;
                    DataSet ds          = new DataSet();
                    SqlDataAdapter da   = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        approverLevelList.Add(new ApproverLevel
                        {
                            ID          = Convert.ToInt32(dr["ID"]),
                            Description = Convert.ToString(dr["Descripton"]),

                        });
                    }
                    con.Close();
                }
            }
            approverLevelList.Add(new ApproverLevel { ID = -1, Description = "Please Select One" });
            return approverLevelList;
        }

        /// <summary>
        /// added on 23th jul 2021 @BK
        /// </summary>
        /// <returns></returns>
        public List<ApproverMaster> GetApproverUserForDopDown()
        {
            List<ApproverMaster> approverUserList = new List<ApproverMaster>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetApproverUserListForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        approverUserList.Add(new ApproverMaster
                        {
                            UserId = Convert.ToInt32(dr["ApproverUserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            Position= Convert.ToString(dr["Position"]),

                        });
                    }
                    con.Close();
                }
            }
            approverUserList.Add(new ApproverMaster { UserId = -1, UserName = "Select Approver",Position= "Select Approver" });
            return approverUserList;
        }


        #endregion
    }
}
