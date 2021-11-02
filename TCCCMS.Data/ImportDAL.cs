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
        public void ImportCrew(object dataTable, string shipNumber)
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
                    //if (dTable.Rows[r][1].ToString() != String.Empty ||
                    //    dTable.Rows[r][2].ToString() != String.Empty ||
                    //    dTable.Rows[r][3].ToString() != String.Empty ||
                    //    dTable.Rows[r][4].ToString() != String.Empty ||
                    //    dTable.Rows[r][5].ToString() != String.Empty ||
                    //    dTable.Rows[r][6].ToString() != String.Empty ||
                    //    dTable.Rows[r][7].ToString() != String.Empty ||
                    //    dTable.Rows[r][8].ToString() != String.Empty ||
                    //    dTable.Rows[r][9].ToString() != String.Empty ||
                    //    dTable.Rows[r][12].ToString() != String.Empty
                    //    )
                    {

                        command.Parameters.Clear();
                        string CrewName = dTable.Rows[r][1].ToString();
                        string Rank = dTable.Rows[r][2].ToString();
                        ///////////////////////////////////////////////////////////////////
                        //string Nationality = dTable.Rows[r][4].ToString();
                        //string DateOfBirth = dTable.Rows[r][5].ToString();
                        //string PlaceOfBirth = dTable.Rows[r][6].ToString();
                        //string DateOfJoining = dTable.Rows[r][7].ToString();
                        //string PlaceOfJoining = dTable.Rows[r][8].ToString();
                        //string PassportNo = dTable.Rows[r][9].ToString();
                        //string ExpiryDate = dTable.Rows[r][12].ToString();
                        ///////////////////////////////////////////////////////////////////
                        string ShipNumber = shipNumber;
                        string Email = dTable.Rows[r][3].ToString();



                        command.Parameters.AddWithValue("@UserName", CrewName);
                        command.Parameters.AddWithValue("@RankName", Rank);
                        ///////////////////////////////////////////////////////////////////
                        //command.Parameters.AddWithValue("@xyzzzz", Nationality);
                        //command.Parameters.AddWithValue("@xyzzzz", DateOfBirth);
                        //command.Parameters.AddWithValue("@xyzzzz", PlaceOfBirth);
                        //command.Parameters.AddWithValue("@xyzzzz", DateOfJoining);
                        //command.Parameters.AddWithValue("@xyzzzz", PlaceOfJoining);
                        //command.Parameters.AddWithValue("@xyzzzz", PassportNo);
                        //command.Parameters.AddWithValue("@xyzzzz", ExpiryDate);
                        ///////////////////////////////////////////////////////////////////
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
