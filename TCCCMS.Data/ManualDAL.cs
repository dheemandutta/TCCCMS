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
    public class ManualDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;
        //SqlConnection con ;

        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId,string serachText)
        {
            List<Manual> manualsList = new List<Manual>();

            using(SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SearchManuals", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    //cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    //cmd.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                    //cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@VolumeId", volumeId);
                    cmd.Parameters.AddWithValue("@SearchText", serachText);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        manualsList.Add(new Manual
                        {
                            ManualId = Convert.ToInt32(dr["ManualId"]),
                            //VolumeId = Convert.ToInt32(dr["VolumeId"]),
                            ManualFileName = Convert.ToString(dr["ManualFileName"]),
                            ManualHtml = Convert.ToString(dr["ManualHtml"]),
                            ManualHeader = Convert.ToString(dr["ManualHeader"]),
                            ManualBodyText = Convert.ToString(dr["ManualBodyText"]),
                            ActionName = Convert.ToString(dr["ActionName"]),
                            ControllerName = Convert.ToString(dr["ControllerName"])
                        });
                    }
                    // totalCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                    con.Close();
                }
            }


            return manualsList;

        }

        public Manual GetManual(string controllerName,string actionName)
        {
            Manual file = new Manual();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetManualByControllerAction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ControllerName", controllerName);
                    cmd.Parameters.AddWithValue("@ActionName", actionName);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);


                    file.ManualId = Convert.ToInt32(ds.Tables[0].Rows[0]["ManualId"]);
                    //VolumeId = Convert.ToInt32(dr["VolumeId"]),
                    file.ManualFileName = Convert.ToString(ds.Tables[0].Rows[0]["ManualFileName"]);
                    file.ManualHtml = Convert.ToString(ds.Tables[0].Rows[0]["ManualHtml"]);
                    file.ManualHeader = Convert.ToString(ds.Tables[0].Rows[0]["ManualHeader"]);
                    file.ManualBodyText = Convert.ToString(ds.Tables[0].Rows[0]["ManualBodyText"]);
                    file.ManualBodyHtml = Convert.ToString(ds.Tables[0].Rows[0]["ManualBodyHtml"]);
                    file.ActionName = Convert.ToString(ds.Tables[0].Rows[0]["ActionName"]);
                    file.ControllerName = Convert.ToString(ds.Tables[0].Rows[0]["ControllerName"]);
                    

                    con.Close();
                }
            }

            return file;
        }
    }
}
