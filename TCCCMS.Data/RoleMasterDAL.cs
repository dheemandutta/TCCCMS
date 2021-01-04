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

                    //if (item["CountryID"] != System.DBNull.Value)
                    //    groupMasterPOCO.CountryID = Convert.ToInt16(item["CountryID"]);

                    crewtimesheetList.Add(groupMasterPOCO);
                }
            }
            return crewtimesheetList;
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
