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
        /// <summary>
        /// 'category' Added on 19th jun 2021 @BK
        /// for category wise search
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="volumeId"></param>
        /// <param name="serachText"></param>
        /// <param name="shipId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId,string serachText,int shipId,string category)
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
                    cmd.Parameters.AddWithValue("@ShipId", shipId);
                    cmd.Parameters.AddWithValue("@Category", category);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        manualsList.Add(new Manual
                        {
                            ManualId        = Convert.ToInt32(dr["ManualId"]),
                            //VolumeId = Convert.ToInt32(dr["VolumeId"]),
                            ManualFileName  = Convert.ToString(dr["ManualFileName"]),
                            ManualHtml      = Convert.ToString(dr["ManualHtml"]),
                            ManualHeader    = Convert.ToString(dr["ManualHeader"]),
                            ManualBodyText  = Convert.ToString(dr["ManualBodyText"]),
                            ActionName      = Convert.ToString(dr["ActionName"]),
                            ControllerName  = Convert.ToString(dr["ControllerName"])
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

        public Manual GetActionNameByFileName(string fileName)
        {
            Manual file = new Manual();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetActionByFileName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    con.Open();

                    DataSet ds          = new DataSet();
                    SqlDataAdapter da   = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        file.ManualId           = Convert.ToInt32(ds.Tables[0].Rows[0]["ManualId"]);
                        file.ManualFileName     = Convert.ToString(ds.Tables[0].Rows[0]["ManualFileName"]);
                        file.ActionName         = Convert.ToString(ds.Tables[0].Rows[0]["ActionName"]);
                        file.ControllerName     = Convert.ToString(ds.Tables[0].Rows[0]["ControllerName"]);

                    }



                    con.Close();
                }
            }

            return file;
        }

        public Volume GetVolumeById(string volumeId)
        {
            Volume vol = new Volume();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVolumeById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VolumeId", Convert.ToInt32(volumeId));
                    con.Open();

                    DataSet ds          = new DataSet();
                    SqlDataAdapter da   = new SqlDataAdapter(cmd);
                    da.Fill(ds);


                    vol.VolumeId            = Convert.ToInt32(ds.Tables[0].Rows[0]["VolumeId"]);
                    vol.Name                = Convert.ToString(ds.Tables[0].Rows[0]["VolumeName"]);
                    vol.Description         = Convert.ToString(ds.Tables[0].Rows[0]["VolumeMasterDesc"]);
                    vol.ControllerName      = Convert.ToString(ds.Tables[0].Rows[0]["ControllerName"]);


                    con.Close();
                }
            }

            return vol;
        }

        #region Common to All Manual

        public ShipManual GetCommonToAllManual(string controllerName, string actionName)
        {
            ShipManual file = new ShipManual();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCommonToAllManualByControllerAction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ControllerName", controllerName);
                    cmd.Parameters.AddWithValue("@ActionName", actionName);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);


                    file.Id             = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                    file.Name           = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    file.Content        = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    file.BodyHeader     = Convert.ToString(ds.Tables[0].Rows[0]["BodyHeader"]);
                    file.BodyText       = Convert.ToString(ds.Tables[0].Rows[0]["BodyText"]);
                    file.BodyHtml       = Convert.ToString(ds.Tables[0].Rows[0]["BodyHtml"]);
                    file.ActionName     = Convert.ToString(ds.Tables[0].Rows[0]["ActionName"]);
                    file.ControllerName = Convert.ToString(ds.Tables[0].Rows[0]["ControllerName"]);


                    con.Close();
                }
            }

            return file;
        }

        #endregion

        #region Reference Material Manual

        public ShipManual GetReferenceMaterialManual(string controllerName, string actionName)
        {
            ShipManual file = new ShipManual();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetReferenceMaterialManualByControllerAction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ControllerName", controllerName);
                    cmd.Parameters.AddWithValue("@ActionName", actionName);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);


                    file.Id             = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                    file.Name           = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    file.Content        = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    file.BodyHeader     = Convert.ToString(ds.Tables[0].Rows[0]["BodyHeader"]);
                    file.BodyText       = Convert.ToString(ds.Tables[0].Rows[0]["BodyText"]);
                    file.BodyHtml       = Convert.ToString(ds.Tables[0].Rows[0]["BodyHtml"]);
                    file.ActionName     = Convert.ToString(ds.Tables[0].Rows[0]["ActionName"]);
                    file.ControllerName = Convert.ToString(ds.Tables[0].Rows[0]["ControllerName"]);


                    con.Close();
                }
            }

            return file;
        }

        #endregion
    }
}
