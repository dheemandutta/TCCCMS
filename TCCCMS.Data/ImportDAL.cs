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

                    if (dTable.Rows[r][1].ToString() != String.Empty || !dTable.Rows[r][0].ToString().Contains("signature"))
                    {

                        command.Parameters.Clear();
                        string watchkeeper = String.Empty;//dTable.Rows[r][0].ToString();
                        string notes = String.Empty;//dTable.Rows[r][1].ToString();
                        string overtime = String.Empty;//dTable.Rows[r][2].ToString();
                        string employeeNum = dTable.Rows[r][0].ToString();
                        string rank = dTable.Rows[r][3].ToString();
                        string firstname = dTable.Rows[r][2].ToString();
                        string middlename = String.Empty;//dTable.Rows[r][6].ToString();
                        string lastname = dTable.Rows[r][1].ToString();
                        string dob = dTable.Rows[r][5].ToString();
                        string pob = dTable.Rows[r][7].ToString();
                        string passportbook = string.Empty;
                        string seaman = string.Empty;

                        ////////////////////////////////////////////
                        string issuingStateOfIdentityDocument = dTable.Rows[r][14].ToString();
                        string expiryDateOfIdentityDocument = dTable.Rows[r][15].ToString();
                        string createdOn = dTable.Rows[r][16].ToString();
                        string latestUpdate = dTable.Rows[r][17].ToString();
                        string department = dTable.Rows[r][18].ToString();
                        //////////////////////////////////////////////////////





                        if (dTable.Rows[r][11].ToString().ToUpper().Trim() == "PASSPORT")
                            passportbook = dTable.Rows[r][12].ToString();
                        else
                            seaman = dTable.Rows[r][12].ToString();
                        string countryname = dTable.Rows[r][4].ToString();
                        string gender = dTable.Rows[r][8].ToString();
                        bool bwatchkeeper = false;
                        bool bovertime = false;
                        DateTime dateofbirth = new DateTime();

                        DateTime ExpiryDateOfIdentityDocument = new DateTime();
                        DateTime CreatedOn = new DateTime();
                        DateTime LatestUpdate = new DateTime();

                        if (rank != String.Empty && firstname != String.Empty && lastname != string.Empty & dob != string.Empty && rank != string.Empty && countryname != string.Empty)
                        {
                            if (watchkeeper != string.Empty)
                            {
                                try
                                {
                                    bwatchkeeper = Convert.ToBoolean(watchkeeper);
                                }
                                catch
                                {
                                    bwatchkeeper = false;
                                }
                            }

                            if (overtime != string.Empty)
                            {
                                try
                                {
                                    bovertime = Convert.ToBoolean(overtime);
                                }
                                catch
                                {
                                    bovertime = false;
                                }
                            }

                            //if (dob != string.Empty)
                            //{
                            //    try
                            //    {
                            //        dob = dob.Replace('.', '-');
                            //        dateofbirth = dob.FormatDate(ConfigurationManager.AppSettings["InputDateFormat"].ToString(), ConfigurationManager.AppSettings["InputDateSeperator"].ToString(), ConfigurationManager.AppSettings["OutputDateFormat"].ToString(), ConfigurationManager.AppSettings["OutputDateSeperator"].ToString());
                            //    }
                            //    catch
                            //    {
                            //        dateofbirth = new DateTime(2000, 01, 01);
                            //    }
                            //}

                            if (gender.Trim().ToUpper() == "MALE") gender = "Male";
                            else if (gender.Trim().ToUpper() == "FEMALE") gender = "Female";


                            command.Parameters.AddWithValue("@WatchKeeper", bwatchkeeper);
                            if (!String.IsNullOrEmpty(notes))
                                command.Parameters.AddWithValue("@notes", notes);
                            else
                                command.Parameters.AddWithValue("@notes", DBNull.Value);
                            command.Parameters.AddWithValue("@Overtime", bovertime);
                            if (!String.IsNullOrEmpty(employeeNum))
                                command.Parameters.AddWithValue("@EmployeeNum", employeeNum);
                            else
                                command.Parameters.AddWithValue("@EmployeeNum", DBNull.Value);
                            command.Parameters.AddWithValue("@Rank", rank);
                            command.Parameters.AddWithValue("@FirstName", firstname);
                            if (!String.IsNullOrEmpty(middlename))
                                command.Parameters.AddWithValue("@MiddleName", middlename);
                            else
                                command.Parameters.AddWithValue("@MiddleName", DBNull.Value);
                            command.Parameters.AddWithValue("@LastName", lastname);
                            command.Parameters.AddWithValue("@DOB", dateofbirth);
                            if (!string.IsNullOrEmpty(pob))
                                command.Parameters.AddWithValue("@POB", pob);
                            else
                                command.Parameters.AddWithValue("@POB", DBNull.Value);

                            if (!String.IsNullOrEmpty(passportbook))
                                command.Parameters.AddWithValue("@PassportBook", passportbook);
                            else
                                command.Parameters.AddWithValue("@PassportBook", DBNull.Value);

                            if (!String.IsNullOrEmpty(seaman))
                                command.Parameters.AddWithValue("@Seaman", seaman);
                            else
                                command.Parameters.AddWithValue("@Seaman", DBNull.Value);
                            command.Parameters.AddWithValue("@Country", countryname);

                            //command.Parameters.AddWithValue("@VesselId", vesselId);

                            //if (!String.IsNullOrEmpty(gender))
                            //    command.Parameters.AddWithValue("@Gender", gender);
                            //else
                            //    command.Parameters.AddWithValue("@Gender", DBNull.Value);










                            /////////////////////////////////////////////////////////////////////////////////////////////////
                            if (!String.IsNullOrEmpty(issuingStateOfIdentityDocument))
                                command.Parameters.AddWithValue("@IssuingStateOfIdentityDocument", issuingStateOfIdentityDocument);
                            else
                                command.Parameters.AddWithValue("@IssuingStateOfIdentityDocument", DBNull.Value);




                            //if (expiryDateOfIdentityDocument != string.Empty)
                            //{
                            //    try
                            //    {
                            //        expiryDateOfIdentityDocument = expiryDateOfIdentityDocument.Replace('.', '-');
                            //        ExpiryDateOfIdentityDocument = expiryDateOfIdentityDocument.FormatDate(ConfigurationManager.AppSettings["InputDateFormat"].ToString(), ConfigurationManager.AppSettings["InputDateSeperator"].ToString(), ConfigurationManager.AppSettings["OutputDateFormat"].ToString(), ConfigurationManager.AppSettings["OutputDateSeperator"].ToString());
                            //    }
                            //    catch
                            //    {
                            //        ExpiryDateOfIdentityDocument = new DateTime(2000, 01, 01);
                            //    }
                            //}
                            //command.Parameters.AddWithValue("@ExpiryDateOfIdentityDocument", ExpiryDateOfIdentityDocument);




                            //if (createdOn != string.Empty)
                            //{
                            //    try
                            //    {
                            //        createdOn = createdOn.Replace('.', '-');
                            //        CreatedOn = createdOn.FormatDate(ConfigurationManager.AppSettings["InputDateFormat"].ToString(), ConfigurationManager.AppSettings["InputDateSeperator"].ToString(), ConfigurationManager.AppSettings["OutputDateFormat"].ToString(), ConfigurationManager.AppSettings["OutputDateSeperator"].ToString());
                            //    }
                            //    catch
                            //    {
                            //        CreatedOn = new DateTime(2000, 01, 01);
                            //    }
                            //}
                            //command.Parameters.AddWithValue("@CreatedOn", CreatedOn);




                            //if (latestUpdate != string.Empty)
                            //{
                            //    try
                            //    {
                            //        latestUpdate = latestUpdate.Replace('.', '-');
                            //        LatestUpdate = latestUpdate.FormatDate(ConfigurationManager.AppSettings["InputDateFormat"].ToString(), ConfigurationManager.AppSettings["InputDateSeperator"].ToString(), ConfigurationManager.AppSettings["OutputDateFormat"].ToString(), ConfigurationManager.AppSettings["OutputDateSeperator"].ToString());
                            //    }
                            //    catch
                            //    {
                            //        LatestUpdate = new DateTime(2000, 01, 01);
                            //    }
                            //}
                            //command.Parameters.AddWithValue("@LatestUpdate", LatestUpdate);




                            if (!String.IsNullOrEmpty(department))
                                command.Parameters.AddWithValue("@Department", department);
                            else
                                command.Parameters.AddWithValue("@Department", DBNull.Value);
                            ////////////////////////////////////////////////////////////////////////////////////////////////////

                            int i = command.ExecuteNonQuery();
                        }

                    }
                }


                //command.Parameters.Add(new SqlParameter("@XMLDoc", SqlDbType.VarChar));
                //command.Parameters[0].Value = strXMl; //passing the string form of XML generated above


            }


        }



    }
}
