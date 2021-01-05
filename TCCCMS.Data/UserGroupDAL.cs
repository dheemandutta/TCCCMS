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
    public class UserGroupDAL
    {
        public int SaveUpdateUserGroup(UserGroupPOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveUpdateUserGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", pOCO.UserId);

            //cmd.Parameters.AddWithValue("@GroupId", pOCO.GroupId);
            cmd.Parameters.AddWithValue("@SelectedGroup", pOCO.SelectedGroup.ToString());

            //cmd.Parameters.AddWithValue("@IsActive", pOCO.IsActive);

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


            if (pOCO.UserGroupId > 0)
            {
                cmd.Parameters.AddWithValue("@UserGroupId ", pOCO.UserGroupId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UserGroupId ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }


        public List<UserGroupPOCO> GetAllUserGroupByUserID(int UserId)
        {
            List<UserGroupPOCO> prodPOList = new List<UserGroupPOCO>();
            List<UserGroupPOCO> prodPO = new List<UserGroupPOCO>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllUserGroupByUserID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToUserGroupList(ds);
        }

        private List<UserGroupPOCO> ConvertDataTableToUserGroupList(DataSet ds)
        {
            List<UserGroupPOCO> pcList = new List<UserGroupPOCO>();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserGroupPOCO pPOCOPC = new UserGroupPOCO();

                    //if (item["ID"] != null)
                    //    pPOCOPC.ID = Convert.ToInt32(item["ID"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.UserName = item["UserName"].ToString();

                    if (item["GroupName"] != System.DBNull.Value)
                        pPOCOPC.GroupName = item["GroupName"].ToString();    

                    if (item["CreatedBy"] != System.DBNull.Value)
                        pPOCOPC.CreatedBy = item["CreatedBy"].ToString();

                    if (item["ModifiedBy"] != System.DBNull.Value)
                        pPOCOPC.ModifiedBy = item["ModifiedBy"].ToString();

                    pcList.Add(pPOCOPC);
                }
            }
            return pcList;
        }


        public int DeleteUserGroup(int UserGroupId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeleteUserGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserGroupId", UserGroupId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

    }
}
