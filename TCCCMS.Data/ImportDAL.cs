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
                SqlCommand command = new SqlCommand("stpImportCrew", con);
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


        public void ImportTemporaryCrew(object dataTable, string shipNumber)
        {
            DataTable dTable = (DataTable)dataTable;
            DataSet ds = new DataSet("CrewImport");
            ds = dTable.DataSet;
            //string strXMl = ds.GetXml();
            StringBuilder sb = new StringBuilder();
            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))

            {

                con.Open();
                SqlCommand command = new SqlCommand("ImportTempCrewListFromExcel", con);
                command.CommandType = CommandType.StoredProcedure;

                for (int r = 0; r < dTable.Rows.Count; r++)
                {

                    if (dTable.Rows[r][1].ToString() != String.Empty || dTable.Rows[r][2].ToString() != String.Empty || dTable.Rows[r][3].ToString() != String.Empty)
                    {
                        var sArry = dTable.Rows[r].ItemArray.Select(x => x.ToString()).Skip(1).Take(3).ToArray();
                        var s = string.Join(",", sArry);
                        sb.Append(s);
                        sb.Append(";");

                    }
                }
                //sb.Append(";");
                string crew = sb.ToString();
                command.Parameters.AddWithValue("@ShipId", shipNumber);
                command.Parameters.AddWithValue("@TempCrews", sb.ToString());
                int i = command.ExecuteNonQuery();
                con.Close();
            }
        }


        public TemporaryCrewViewModel GetAllTemporaryCrews()
        {
            TemporaryCrewViewModel tcVM = new TemporaryCrewViewModel();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTempCrewAndExistingCrew", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<UserMasterPOCO> tcList = new List<UserMasterPOCO>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserMasterPOCO tc = new UserMasterPOCO();
                        tc.UserId = Convert.ToInt32(dr["ID"]);
                        tc.UserName = Convert.ToString(dr["CrewName"]);
                        tc.RankName = Convert.ToString(dr["RankName"]);
                        tc.Email = Convert.ToString(dr["Email"]);
                        tc.ShipId = Convert.ToInt32(dr["ShipId"]);

                        var histRows = ds.Tables[1].Rows
                              .Cast<DataRow>()
                              .Where(x => x.Field<string>(3) == tc.RankName && x.Field<int>(4) == tc.ShipId).ToList();

                        List<UserMasterPOCO> replaceableUserList = new List<UserMasterPOCO>();
                        UserMasterPOCO initial = new UserMasterPOCO();
                        initial.UserId = 0;
                        initial.UserName = "--Add as New--";
                        replaceableUserList.Add(initial);
                        foreach (DataRow drh in histRows)
                        {
                            UserMasterPOCO replaceableUser = new UserMasterPOCO();
                            replaceableUser.UserId = Convert.ToInt32(drh["UserId"]);
                            replaceableUser.UserName = Convert.ToString(drh["UserName"])+"(" + Convert.ToString(drh["UserCode"]) + ")";
                            replaceableUser.UserCode = Convert.ToString(drh["UserCode"]);

                            replaceableUserList.Add(replaceableUser);
                        }
                        tc.ReplaceableCrews = replaceableUserList;


                        tcList.Add(tc);
                    }
                    tcVM.TemporaryCrewList = tcList;
                }

            }


            return tcVM;
        }


        public string ImportCrewList(DataTable dt, ref string FailureMessage, ref int FailureCount , ref int SuccessCount)
        {
            DataTable dTable = dt;
            string returnMessage = "";
            StringBuilder sb = new StringBuilder();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))

            {

                con.Open();
                SqlCommand command = new SqlCommand("ImportCrewListFromTemporaryCrews", con);
                command.CommandType = CommandType.StoredProcedure;

                for (int r = 0; r < dTable.Rows.Count; r++)
                {

                    if (dTable.Rows[r][1].ToString() != String.Empty || dTable.Rows[r][2].ToString() != String.Empty || dTable.Rows[r][3].ToString() != String.Empty)
                    {
                        var sArry = dTable.Rows[r].ItemArray.Select(x => x.ToString()).Skip(1).ToArray();
                        var s = string.Join(",", sArry);
                        sb.Append(s);
                        sb.Append(";");

                    }
                }
                //sb.Append(";");
                string crew = sb.ToString();
                command.Parameters.AddWithValue("@TempCrews", sb.ToString());
                command.Parameters.Add("@FailureMessage", SqlDbType.NVarChar, 5000);
                command.Parameters["@FailureMessage"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@FailureCount", SqlDbType.Int, 4);
                command.Parameters["@FailureCount"].Direction = ParameterDirection.Output;
                command.Parameters.Add("@SuccessCount", SqlDbType.Int, 4);
                command.Parameters["@SuccessCount"].Direction = ParameterDirection.Output;


                command.ExecuteScalar();

                FailureCount = Convert.ToInt32(command.Parameters["@FailureCount"].Value);
                SuccessCount = Convert.ToInt32(command.Parameters["@SuccessCount"].Value);
                FailureMessage = Convert.ToString(command.Parameters["@FailureMessage"].Value);
                //int i = command.ExecuteNonQuery();
                con.Close();

                return returnMessage;
            }
        }

    }
}
