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
    public class RoleMasterDAL
    {
        public List<RoleMasterPOCO> GetAllRoleMasterPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<RoleMasterPOCO> pOList = new List<RoleMasterPOCO>();
            List<RoleMasterPOCO> equipmentsPO = new List<RoleMasterPOCO>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllRoleMasterPageWise", con))
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
                        pOList.Add(new RoleMasterPOCO
                        {
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            ModifiedBy = Convert.ToString(dr["ModifiedBy"])
                        });
                    }
                    recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }

        public int SaveUpdateRoleMaster(RoleMasterPOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveUpdateRoleMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleName", pOCO.RoleName.ToString());

            //cmd.Parameters.AddWithValue("@IsActive", crew.IsActive);

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


            if (pOCO.RoleId > 0)
            {
                cmd.Parameters.AddWithValue("@RoleId", pOCO.RoleId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RoleId", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }



        public RoleMasterPOCO GetRoleMasterByRoleId(int RoleId)
        {
            RoleMasterPOCO prodPOList = new RoleMasterPOCO();
            RoleMasterPOCO prodPO = new RoleMasterPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetRoleMasterByRoleId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleId", RoleId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToRoleMasterByRoleIdList(ds);
        }

        private RoleMasterPOCO ConvertDataTableToRoleMasterByRoleIdList(DataSet ds)
        {
            RoleMasterPOCO pPOCOPC = new RoleMasterPOCO();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    if (item["RoleId"] != null)
                        pPOCOPC.RoleId = Convert.ToInt32(item["RoleId"].ToString());

                    if (item["RoleName"] != System.DBNull.Value)
                        pPOCOPC.RoleName = item["RoleName"].ToString();

                    //if (item["CreatedBy"] != System.DBNull.Value)
                    //    pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    //if (item["ModifiedBy"] != System.DBNull.Value)
                    //    pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    //pcList.Add(pPOCOPC);
                }
            }
            return pPOCOPC;
        }




        public int DeleteRoleMaster(int RoleId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeleteRoleMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoleId", RoleId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }


        public List<RoleGroup> GetAllRoles()
        {
            RoleGroup roleMasterPOCO = new RoleGroup();
            List<RoleGroup> roleMasterPOCOs = new List<RoleGroup>();

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllRole", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }

            return ConvertDataTableToRole2(ds);


        }

        private List<RoleMasterPOCO> ConvertDataTableToRole(DataSet ds)
        {
            
            List<RoleMasterPOCO> roleMasterPOCOs = new List<RoleMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RoleMasterPOCO pPOCOPC = new RoleMasterPOCO();

                    if (item["Id"] != null)
                        pPOCOPC.RoleId = Convert.ToInt32(item["Id"].ToString());

                    if (item["RoleName"] != System.DBNull.Value)
                        pPOCOPC.RoleName = item["RoleName"].ToString();
                   
                    roleMasterPOCOs.Add(pPOCOPC);
                   
                }
            }
            return roleMasterPOCOs;
        }

        public int SaveRoleGroup(RoleGroup pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveRoleGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleId", pOCO.RoleId);
            cmd.Parameters.AddWithValue("@GroupId", pOCO.GroupId);

            

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }

        public RoleMasterPOCO GetRoleByGroupId(int GroupId)
        {
            RoleMasterPOCO roleMasterPOCO = new RoleMasterPOCO();
            List<RoleMasterPOCO> roleMasterPOCOs = new List<RoleMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetRoleNameByGroupId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GroupId", GroupId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToRole(ds).FirstOrDefault();
        }



        private List<RoleGroup> ConvertDataTableToRole2(DataSet ds)
        {

            List<RoleGroup> roleMasterPOCOs = new List<RoleGroup>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RoleGroup pPOCOPC = new RoleGroup();

                    if (item["Id"] != null)
                        pPOCOPC.RoleId = Convert.ToInt32(item["Id"].ToString());

                    if (item["RoleName"] != System.DBNull.Value)
                        pPOCOPC.RoleName = item["RoleName"].ToString();

                    roleMasterPOCOs.Add(pPOCOPC);

                }
            }
            return roleMasterPOCOs;
        }
    }
}
