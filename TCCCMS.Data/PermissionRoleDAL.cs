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
        public int SavePermissionRole(int permissionId, string permissionRole)   
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSavePermissionRole", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PermissionId", permissionId);
            if (!String.IsNullOrEmpty(permissionRole))
                cmd.Parameters.AddWithValue("@MappingRoles", permissionRole);
            else
                cmd.Parameters.AddWithValue("@MappingRoles", DBNull.Value);

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }


        public string GetAllCommaSeperatedPermissionRoleByPermissionId(int permissionId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllPermissionRoleByPermissionId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PermissionId", permissionId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }

            var roleIds = ds.Tables[0].AsEnumerable().Select(s => s.Field<int>("RoleId")).ToArray();
            string commaSeperatedRoles = String.Join(",", roleIds);

            return commaSeperatedRoles;

        }





        //for Permission drp
        public List<PermissionRolePOCO> GetAllPermissionForDrp(/*int VesselID*/)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllPermissionForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<PermissionRolePOCO> ranksList = myTable.AsEnumerable().Select(m => new PermissionRolePOCO()
            {
                PermissionId = m.Field<int>("PermissionId"),
                PermissionName = m.Field<string>("PermissionName"),

            }).ToList();
            con.Close();
            return ranksList;

        }


        //for Role drp
        public List<PermissionRolePOCO> GetAllRoleForDrp(/*int VesselID*/)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllRolesForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<PermissionRolePOCO> ranksList = myTable.AsEnumerable().Select(m => new PermissionRolePOCO()
            {
                RoleId = m.Field<int>("RoleId"),
                RoleName = m.Field<string>("RoleName"),

            }).ToList();
            con.Close();
            return ranksList;

        }

    }
}
