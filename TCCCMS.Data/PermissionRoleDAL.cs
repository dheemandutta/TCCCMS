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
    public class PermissionRoleDAL
    {
        public int SavePermissionRole(PermissionRolePOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSavePermissionRole", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PermssionId", pOCO.PermssionId);

            cmd.Parameters.AddWithValue("@RoleId", pOCO.RoleId);

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


            if (pOCO.PermissionRoleId > 0)
            {
                cmd.Parameters.AddWithValue("@PermissionRoleId", pOCO.PermissionRoleId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PermissionRoleId", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }


        public int DeletePermissionRole(int PermissionRoleId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpDeletePermissionRole", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PermissionRoleId", PermissionRoleId);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

    }
}
