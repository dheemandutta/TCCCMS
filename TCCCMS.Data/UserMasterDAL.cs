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
    public class UserMasterDAL
    {
        public int SaveUpdateUser(UserMasterPOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveUpdateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RankId", pOCO.RankId);

            cmd.Parameters.AddWithValue("@ShipId", pOCO.ShipId);

            cmd.Parameters.AddWithValue("@UserName", pOCO.UserName.ToString());

            cmd.Parameters.AddWithValue("@Password", pOCO.Password.ToString());

            //if (!String.IsNullOrEmpty(pOCO.CreatedOn))
            //{
            //    cmd.Parameters.AddWithValue("@CreatedOn", pOCO.CreatedOn.ToString());
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
            //}

            //cmd.Parameters.AddWithValue("@IsActive", pOCO.IsActive);

            if (!String.IsNullOrEmpty(pOCO.Email))
            {
                cmd.Parameters.AddWithValue("@Email", pOCO.Email.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Email", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(pOCO.CreatedBy))
            {
                cmd.Parameters.AddWithValue("@CreatedBy", pOCO.CreatedBy.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(pOCO.ModifiedBy))
            {
                cmd.Parameters.AddWithValue("@ModifiedBy", pOCO.ModifiedBy.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@ModifiedBy", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(pOCO.Gender))
            {
                cmd.Parameters.AddWithValue("@Gender", pOCO.Gender.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Gender", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(pOCO.VesselIMO))
            {
                cmd.Parameters.AddWithValue("@VesselIMO", pOCO.VesselIMO.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@VesselIMO", DBNull.Value);
            }


            if (pOCO.UserId > 0)
            {
                cmd.Parameters.AddWithValue("@UserId ", pOCO.UserId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UserId ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }



        public List<UserMasterPOCO> GetAllUserPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<UserMasterPOCO> pOList = new List<UserMasterPOCO>();
            List<UserMasterPOCO> equipmentsPO = new List<UserMasterPOCO>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllUserPageWise", con))
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
                        pOList.Add(new UserMasterPOCO
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            CreatedOn1 = Convert.ToString(dr["CreatedOn"]),
                            Email = Convert.ToString(dr["Email"]),
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            VesselIMO = Convert.ToString(dr["VesselIMO"]),
                            RankName = Convert.ToString(dr["RankName"]),
                            ShipName = Convert.ToString(dr["ShipName"]),
                        });
                    }
                    recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }





        public List<UserMasterPOCO> GetAllUser()
        {
            List<UserMasterPOCO> prodPOList = new List<UserMasterPOCO>();
            List<UserMasterPOCO> prodPO = new List<UserMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToGetAllUserList(ds);
        }

        private List<UserMasterPOCO> ConvertDataTableToGetAllUserList(DataSet ds)
        {
            List<UserMasterPOCO> pcList = new List<UserMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    //if (item["ID"] != null)
                    //    pPOCOPC.ID = Convert.ToInt32(item["ID"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["CreatedOn"] != System.DBNull.Value)
                        pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    if (item["Email"] != System.DBNull.Value)
                        pPOCOPC.Email = item["Email"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["Gender"] != System.DBNull.Value)
                        pPOCOPC.Gender = item["Gender"].ToString();

                    if (item["VesselIMO"] != System.DBNull.Value)
                        pPOCOPC.VesselIMO = item["VesselIMO"].ToString();

                    if (item["RankName"] != System.DBNull.Value)
                        pPOCOPC.RankName = item["RankName"].ToString();

                    pcList.Add(pPOCOPC);
                }
            }
            return pcList;
        }



        public UserMasterPOCO GetUserByUserId(int UserId)
        {
            UserMasterPOCO prodPOList = new UserMasterPOCO();
            UserMasterPOCO prodPO = new UserMasterPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetUserByUserId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToUserByUserIdList(ds);
        }

        private UserMasterPOCO ConvertDataTableToUserByUserIdList(DataSet ds)
        {
            UserMasterPOCO pPOCOPC = new UserMasterPOCO();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //UserMasterPOCO pPOCOPC = new UserMasterPOCO();
                    RankPOCO rank   = new RankPOCO();
                    //Ship ship       = new Ship();

                    if (item["UserId"] != null)
                        pPOCOPC.UserId = Convert.ToInt32(item["UserId"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["UserCode"] != System.DBNull.Value)//Added on 20th JAN 2021 @BK
                        pPOCOPC.UserCode = item["UserCode"].ToString();

                    if (item["Password"] != System.DBNull.Value)
                        pPOCOPC.Password = item["Password"].ToString();

                    if (item["CreatedOn"] != System.DBNull.Value)
                        pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    if (item["Email"] != System.DBNull.Value)
                        pPOCOPC.Email = item["Email"].ToString();

                    //if (item["CreatedBy"] != System.DBNull.Value)
                    //    pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    //if (item["ModifiedBy"] != System.DBNull.Value)
                    //    pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["Gender"] != System.DBNull.Value)
                        pPOCOPC.Gender = item["Gender"].ToString();

                    if (item["VesselIMO"] != System.DBNull.Value)
                        pPOCOPC.VesselIMO = item["VesselIMO"].ToString();

                    if (item["RankId"] != null)
                    {
                        pPOCOPC.RankId  = Convert.ToInt32(item["RankId"].ToString());
                        rank.RankId     = Convert.ToInt32(item["RankId"].ToString());
                        rank.RankName   = item["RankName"].ToString();
                        pPOCOPC.Rank    = rank;
                    }
                    if (item["ShipId"] != null)
                    {
                        pPOCOPC.ShipId = Convert.ToInt32(item["ShipId"].ToString());
                    }
                       

                    //pcList.Add(pPOCOPC);
                }
            }
            return pPOCOPC;
        }



        public List<UserMasterPOCO> GetUserByIMO(string VesselIMO)
        {
            List<UserMasterPOCO> prodPOList = new List<UserMasterPOCO>();
            List<UserMasterPOCO> prodPO = new List<UserMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetUserByIMO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VesselIMO", VesselIMO);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToUserByIMOList(ds);
        }

        private List<UserMasterPOCO> ConvertDataTableToUserByIMOList(DataSet ds)
        {
            List<UserMasterPOCO> pcList = new List<UserMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    //if (item["ID"] != null)
                    //    pPOCOPC.ID = Convert.ToInt32(item["ID"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["CreatedOn"] != System.DBNull.Value)
                        pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    if (item["Email"] != System.DBNull.Value)
                        pPOCOPC.Email = item["Email"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["Gender"] != System.DBNull.Value)
                        pPOCOPC.Gender = item["Gender"].ToString();

                    if (item["VesselIMO"] != System.DBNull.Value)
                        pPOCOPC.VesselIMO = item["VesselIMO"].ToString();

                    if (item["RankName"] != System.DBNull.Value)
                        pPOCOPC.RankName = item["RankName"].ToString();

                    pcList.Add(pPOCOPC);
                }
            }
            return pcList;
        }



        public List<UserMasterPOCO> GetUserByEmailId(string Email)
        {
            List<UserMasterPOCO> prodPOList = new List<UserMasterPOCO>();
            List<UserMasterPOCO> prodPO = new List<UserMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetUserByEmailId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToUserByEmailList(ds);
        }

        private List<UserMasterPOCO> ConvertDataTableToUserByEmailList(DataSet ds)
        {
            List<UserMasterPOCO> pcList = new List<UserMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    //if (item["ID"] != null)
                    //    pPOCOPC.ID = Convert.ToInt32(item["ID"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["CreatedOn"] != System.DBNull.Value)
                        pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    if (item["Email"] != System.DBNull.Value)
                        pPOCOPC.Email = item["Email"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["Gender"] != System.DBNull.Value)
                        pPOCOPC.Gender = item["Gender"].ToString();

                    if (item["VesselIMO"] != System.DBNull.Value)
                        pPOCOPC.VesselIMO = item["VesselIMO"].ToString();

                    if (item["RankName"] != System.DBNull.Value)
                        pPOCOPC.RankName = item["RankName"].ToString();

                    pcList.Add(pPOCOPC);
                }
            }
            return pcList;
        }



        public List<UserMasterPOCO> GetUserByRank(int RankId)
        {
            List<UserMasterPOCO> prodPOList = new List<UserMasterPOCO>();
            List<UserMasterPOCO> prodPO = new List<UserMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetUserByRank", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RankId", RankId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToUserByRankIdList(ds);
        }

        private List<UserMasterPOCO> ConvertDataTableToUserByRankIdList(DataSet ds)
        {
            List<UserMasterPOCO> pcList = new List<UserMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    //if (item["ID"] != null)
                    //    pPOCOPC.ID = Convert.ToInt32(item["ID"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["CreatedOn"] != System.DBNull.Value)
                        pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    if (item["Email"] != System.DBNull.Value)
                        pPOCOPC.Email = item["Email"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["Gender"] != System.DBNull.Value)
                        pPOCOPC.Gender = item["Gender"].ToString();

                    if (item["VesselIMO"] != System.DBNull.Value)
                        pPOCOPC.VesselIMO = item["VesselIMO"].ToString();

                    if (item["RankName"] != System.DBNull.Value)
                        pPOCOPC.RankName = item["RankName"].ToString();

                    pcList.Add(pPOCOPC);
                }
            }
            return pcList;
        }



        public int DeleteUserMaster(int UserId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeleteUserMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }



        #region DropDown
        public List<UserMasterPOCO> GetAllUserListForDropDown()
        {/*--Added on 16th Jan 2021 @BK--*/
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetAllUserForDropDown", con);
            cmd.Parameters.AddWithValue("@ShipId", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<UserMasterPOCO> userList = myTable.AsEnumerable().Select(m => new UserMasterPOCO()
            {
                UserId = m.Field<int>("ID"),
                UserName = m.Field<string>("Name"),

            }).ToList();
            con.Close();
            userList.Add(new UserMasterPOCO { UserId = -1, UserName = "Please Select One" });
            return userList;

        }
        public List<UserMasterPOCO> GetAllUserListByShipForDropDown(int shipId)
        {/*--Added on 18th Jan 2021 @BK--*/
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetAllUserForDropDown", con);
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<UserMasterPOCO> userList = myTable.AsEnumerable().Select(m => new UserMasterPOCO()
            {
                UserId = m.Field<int>("ID"),
                UserName = m.Field<string>("Name"),

            }).ToList();
            con.Close();
            userList.Add(new UserMasterPOCO { UserId = -1, UserName = "Please Select One" });
            return userList;

        }
        //for Ranks drp
        public List<UserMasterPOCO> GetAllRanksForDrp(/*int VesselID*/)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllRanksForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<UserMasterPOCO> ranksList = myTable.AsEnumerable().Select(m => new UserMasterPOCO()
            {
                RankId = m.Field<int>("RankId"),
                RankName = m.Field<string>("RankName"),

            }).ToList();
            con.Close();
            return ranksList;

        }


        //for Ship drp
        public List<UserMasterPOCO> GetAllShipForDrp(/*int VesselID*/)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllShipForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<UserMasterPOCO> ranksList = myTable.AsEnumerable().Select(m => new UserMasterPOCO()
            {
                ShipId = m.Field<int>("ID"),
                ShipName = m.Field<string>("ShipName"),

            }).ToList();
            con.Close();
            return ranksList;

        }
        #endregion


    }
}
