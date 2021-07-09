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
    public class ImportDAL
    {
        public void ImportCrew(object dataTable/*, int vesselId*/)
        {
            DataTable dTable = (DataTable)dataTable;
            DataSet ds = new DataSet("CrewImport");
            ds = dTable.DataSet;
            //string strXMl = ds.GetXml();



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))

            {

                con.Open();
                SqlCommand command  = new SqlCommand("stpImportCrew", con);
                command.CommandType = CommandType.StoredProcedure;

                for (int r = 0; r < dTable.Rows.Count; r++)
                {

                    if (dTable.Rows[r][1].ToString() != String.Empty || dTable.Rows[r][2].ToString() != String.Empty || dTable.Rows[r][3].ToString() != String.Empty)
                    {

                        command.Parameters.Clear();
                        string CrewName = dTable.Rows[r][1].ToString();
                        string Rank = dTable.Rows[r][2].ToString();
                        //int ShipNumber  = Convert.ToInt32(dTable.Rows[r][3].ToString());
                        string ShipNumber = dTable.Rows[r][3].ToString();
                        string Email = dTable.Rows[r][4].ToString();


                        command.Parameters.AddWithValue("@UserName", CrewName);
                        command.Parameters.AddWithValue("@RankName", Rank);
                        command.Parameters.AddWithValue("@ShipNo", ShipNumber);
                        if (!string.IsNullOrEmpty(Email))
                        command.Parameters.AddWithValue("@Email", Email);
                        else
                            command.Parameters.AddWithValue("@Email", DBNull.Value);

                        int i = command.ExecuteNonQuery();
                    }          
                }

                con.Close();
            }
        }
    }
}
