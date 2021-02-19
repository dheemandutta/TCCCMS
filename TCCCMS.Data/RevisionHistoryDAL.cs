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
    public class RevisionHistoryDAL
    {
        public List<RevisionHistory> GetRevisionHistoryPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<RevisionHistory> pOList = new List<RevisionHistory>();
            List<RevisionHistory> equipmentsPO = new List<RevisionHistory>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetRevisionHistoryPageWise", con))
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
                        pOList.Add(new RevisionHistory
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FormName = Convert.ToString(dr["FormName"]),
                            ModifiedSection = Convert.ToString(dr["ModifiedSection"]),
                            UpdatedOn1 = Convert.ToString(dr["UpdatedOn"]),
                            Version = Convert.ToString(dr["Version"])
                        });
                    }
                    recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }

        public List<RevisionHistory> GetFormIdForModifiedSection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetFormIdForModifiedSection", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<RevisionHistory> List = myTable.AsEnumerable().Select(m => new RevisionHistory()
            {
                FormId = m.Field<int>("FormId")

            }).ToList();

            con.Close();
            return List;
        }
    }
}
