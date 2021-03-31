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
    public class HomeDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;

        #region Login
        public UserMasterPOCO CheckUserLogin(UserMasterPOCO aUser, ref string asReturnMessage)
        {
            UserMasterPOCO lUser = new UserMasterPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckUserLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserCode", aUser.UserCode);
                    cmd.Parameters.AddWithValue("@Password", aUser.Password);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lUser.UserId        = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"].ToString());
                        lUser.UserName      = ds.Tables[0].Rows[0]["UserName"].ToString();
                        lUser.UserCode      = ds.Tables[0].Rows[0]["UserCode"].ToString();

                        if(ds.Tables[0].Rows[0]["Email"] != null)
                        lUser.Email         = ds.Tables[0].Rows[0]["Email"].ToString();

                        if (ds.Tables[0].Rows[0]["ShipId"] != null)
                            lUser.ShipId = Convert.ToInt32(ds.Tables[0].Rows[0]["ShipId"].ToString());
                        else
                            lUser.ShipId = 0;

                        if (ds.Tables[0].Rows[0]["ShipName"] != null)
                            lUser.ShipName      = ds.Tables[0].Rows[0]["ShipName"].ToString();

                        if (ds.Tables[0].Rows[0]["VesselIMO"] != null)
                            lUser.VesselIMO     = ds.Tables[0].Rows[0]["VesselIMO"].ToString();

                        lUser.UserType      = Convert.ToInt32(ds.Tables[0].Rows[0]["UserType"].ToString());

                        //lUser.IsAdmin       = Convert.ToInt32(ds.Tables[0].Rows[0]["IsAdmin"].ToString());

                        asReturnMessage = "1";
                    }
                    else
                        asReturnMessage = "0";



                    con.Close();
                }
            }

            return lUser;
        }

        #endregion
    }
}
