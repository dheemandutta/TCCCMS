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
    public class GroupMasterDAL
    {

        public List<GroupMasterPOCO> GetAllGroupMasterPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<GroupMasterPOCO> pOList = new List<GroupMasterPOCO>();
            List<GroupMasterPOCO> equipmentsPO = new List<GroupMasterPOCO>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllGroupMasterPageWise", con))
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
                        pOList.Add(new GroupMasterPOCO
                        {
                            GroupId = Convert.ToInt32(dr["GroupId"]),
                            GroupName = Convert.ToString(dr["GroupName"]),
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



        public int SaveUpdateGroupMaster(GroupMasterPOCO groupMaster)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveUpdateGroupMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@GroupName", groupMaster.GroupName.ToString());

            //cmd.Parameters.AddWithValue("@IsActive", crew.IsActive);

            if (!String.IsNullOrEmpty(groupMaster.CreatedBy))
            {
                cmd.Parameters.AddWithValue("@CreatedBy", groupMaster.CreatedBy.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(groupMaster.ModifiedBy))
            {
                cmd.Parameters.AddWithValue("@ModifiedBy", groupMaster.ModifiedBy.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@ModifiedBy", DBNull.Value);
            }


            if (groupMaster.GroupId > 0)
            {
                cmd.Parameters.AddWithValue("@GroupId", groupMaster.GroupId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@GroupId", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }


        public List<GroupMasterPOCO> GetAllGroupMaster()
        {
            List<GroupMasterPOCO> prodPOList = new List<GroupMasterPOCO>();
            List<GroupMasterPOCO> prodPO = new List<GroupMasterPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllGroupMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToGetAllGroupMasterList(ds);
        }

        public List<RoleGroup> GetAllGroupsNotInRoles()
        {
            List<RoleGroup> prodPOList = new List<RoleGroup>();
            List<RoleGroup> prodPO = new List<RoleGroup>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllGroupsNotInRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToGetAllGroupMasterList2(ds);
        }

        private List<GroupMasterPOCO> ConvertDataTableToGetAllGroupMasterList(DataSet ds)
        {
            List<GroupMasterPOCO> crewtimesheetList = new List<GroupMasterPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    GroupMasterPOCO groupMasterPOCO = new GroupMasterPOCO();

                    //if (item["GroupId"] != System.DBNull.Value)
                    //    groupMasterPOCO.GroupId = Convert.ToInt32(item["GroupId"].ToString());

                    if (item["GroupName"] != System.DBNull.Value)
                        groupMasterPOCO.GroupName = item["GroupName"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        groupMasterPOCO.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        groupMasterPOCO.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["GroupId"] != System.DBNull.Value)
                        groupMasterPOCO.GroupId = Convert.ToInt16(item["GroupId"]);

                    crewtimesheetList.Add(groupMasterPOCO);
                }
            }
            return crewtimesheetList;
        }

        private List<RoleGroup> ConvertDataTableToGetAllGroupMasterList2(DataSet ds)
        {
            List<RoleGroup> crewtimesheetList = new List<RoleGroup>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    RoleGroup groupMasterPOCO = new RoleGroup();

                    //if (item["GroupId"] != System.DBNull.Value)
                    //    groupMasterPOCO.GroupId = Convert.ToInt32(item["GroupId"].ToString());

                    if (item["GroupName"] != System.DBNull.Value)
                        groupMasterPOCO.GroupName = item["GroupName"].ToString();

                    if (item["CreatedBy"] != System.DBNull.Value)
                        groupMasterPOCO.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        groupMasterPOCO.ModifiedBy = item["ModifiedBy"].ToString();

                    if (item["GroupId"] != System.DBNull.Value)
                        groupMasterPOCO.GroupId = Convert.ToInt16(item["GroupId"]);

                    crewtimesheetList.Add(groupMasterPOCO);
                }
            }
            return crewtimesheetList;
        }

        public GroupMasterPOCO GetGroupMasterByGroupId(int GroupId)
        {
            GroupMasterPOCO prodPOList = new GroupMasterPOCO();
            GroupMasterPOCO prodPO = new GroupMasterPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetGroupMasterByGroupId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GroupId", GroupId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToGroupMasterByGroupIdList(ds);
        }

        private GroupMasterPOCO ConvertDataTableToGroupMasterByGroupIdList(DataSet ds)
        {
            GroupMasterPOCO pPOCOPC = new GroupMasterPOCO();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //UserMasterPOCO pPOCOPC = new UserMasterPOCO();

                    if (item["GroupId"] != null)
                        pPOCOPC.GroupId = Convert.ToInt32(item["GroupId"].ToString());

                    if (item["GroupName"] != System.DBNull.Value)
                        pPOCOPC.GroupName = item["GroupName"].ToString();

                    //if (item["CreatedBy"] != System.DBNull.Value)
                    //    pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    //if (item["ModifiedBy"] != System.DBNull.Value)
                    //    pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    //pcList.Add(pPOCOPC);
                }
            }
            return pPOCOPC;
        }




        public int DeleteGroupMaster(int GroupId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeleteGroupMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", GroupId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

    }
}
