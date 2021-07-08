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



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["RestHourDBConnectionString"].ConnectionString))

            {

                con.Open();
                SqlCommand command = new SqlCommand("stpImportCrew", con);
                command.CommandType = CommandType.StoredProcedure;

                for (int r = 0; r < dTable.Rows.Count; r++)
                {
                        command.Parameters.Clear();
                        string CrewName = dTable.Rows[r][2].ToString();
                        string Rank = dTable.Rows[r][3].ToString();
                        string ShipNumber = dTable.Rows[r][4].ToString();
                        string Email = dTable.Rows[r][5].ToString();


                    command.Parameters.AddWithValue("@UserName", CrewName);
                    command.Parameters.AddWithValue("@Rank", Rank);
                    command.Parameters.AddWithValue("@ShipId", ShipNumber);
                    command.Parameters.AddWithValue("@Email", Email);


                    //if (!String.IsNullOrEmpty(department))
                    //    command.Parameters.AddWithValue("@Department", department);
                    //else
                    //    command.Parameters.AddWithValue("@Department", DBNull.Value);

                    int i = command.ExecuteNonQuery();
                                
                }
                //command.Parameters.Add(new SqlParameter("@XMLDoc", SqlDbType.VarChar));
                //command.Parameters[0].Value = strXMl; //passing the string form of XML generated above
            }
        }
    }
}
