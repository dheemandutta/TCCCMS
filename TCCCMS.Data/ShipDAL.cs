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
    public class ShipDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;

        public List<Ship> GetAllShipsPageWise(int pageIndex, ref int totalCount, int pageSize)
        {
            List<Ship> shipsList = new List<Ship>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllShipDetailsPageWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                    cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@VolumeId", volumeId);
                    //cmd.Parameters.AddWithValue("@SearchText", serachText);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        shipsList.Add(new Ship
                        {
                            ID                      = Convert.ToInt32(dr["ID"]),
                            RowNumber               = Convert.ToInt32(dr["RowNumber"]),
                            ShipName                = Convert.ToString(dr["ShipName"]),
                            FlagOfShip              = Convert.ToString(dr["FlagOfShip"]),
                            IMONumber               = Convert.ToInt32(dr["IMONumber"]),
                            ShipEmail1              = Convert.ToString(dr["ShipEmail"]),
                            Mobile1                 = Convert.ToString(dr["Mobile1"]),
                            VesselTypeId            = Convert.ToInt32(dr["VesselTypeId"]),
                            //VesselTypeName          = Convert.ToString(dr["VesselTypeName"]),
                            VesselSubTypeId         = Convert.ToInt32(dr["VesselSubTypeID"]),
                           // VesselSubTypeName       = Convert.ToString(dr["VesselSubTypeName"]),
                            VesselSubSubTypeId      = Convert.ToInt32(dr["ID"]),
                            //VesselSubSubTypeName    = Convert.ToString(dr["VesselSubSubTypeName"])
                        });
                    }
                    totalCount = Convert.ToInt32(cmd.Parameters["@TotalCount"].Value);

                    con.Close();
                }
            }


            return shipsList;

        }

        public int SaveShipDetails(Ship  ship)
        {
            int recorSaved = 0;

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveShipDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Id", ship.ID);
            cmd.Parameters.AddWithValue("@Name", ship.ShipName);
            cmd.Parameters.AddWithValue("@IMONumber", ship.IMONumber);
            cmd.Parameters.AddWithValue("@Flag", ship.FlagOfShip);
            cmd.Parameters.AddWithValue("@CompanyId", ship.CompanyId);
            cmd.Parameters.AddWithValue("@VesselTypeId", ship.VesselTypeId);
            cmd.Parameters.AddWithValue("@VesselSubTypeId", ship.VesselSubTypeId);
            cmd.Parameters.AddWithValue("@VesselSubSubTypeId", ship.VesselSubSubTypeId);
            cmd.Parameters.AddWithValue("@User", ship.CreateedBy);
            if(ship.ID != 0)
            {
                cmd.Parameters.AddWithValue("@Id", ship.ID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Id", DBNull.Value);
            }
            if (!String.IsNullOrEmpty(ship.ShipEmail1))
            {
                cmd.Parameters.AddWithValue("@ShipEmail1", ship.ShipEmail1.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShipEmail1", DBNull.Value);
            }
            if (!String.IsNullOrEmpty(ship.ShipEmail2))
            {
                cmd.Parameters.AddWithValue("@ShipEmail2", ship.ShipEmail2.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShipEmail2", DBNull.Value);
            }
            if (!String.IsNullOrEmpty(ship.Mobile1))
            {
                cmd.Parameters.AddWithValue("@Mobile1", ship.Mobile1.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mobile1", DBNull.Value);
            }
            if (!String.IsNullOrEmpty(ship.Mobile2))
            {
                cmd.Parameters.AddWithValue("@Mobile2", ship.Mobile2.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mobile2", DBNull.Value);
            }

            if (!String.IsNullOrEmpty(ship.Fax1))
            {
                cmd.Parameters.AddWithValue("@Fax1", ship.Fax1.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fax1", DBNull.Value);
            }
            if (!String.IsNullOrEmpty(ship.Fax2))
            {
                cmd.Parameters.AddWithValue("@Fax2", ship.Fax2.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fax2", DBNull.Value);
            }
            recorSaved = cmd.ExecuteNonQuery();
                con.Close();
           

            return recorSaved;
        }

        public Ship GetShipDetailsById(int shipId)
        {
            Ship ship = new Ship();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetShipDetailsById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShipId", shipId);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    #region mapping dt to ship model
                    ship.ID                     = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    ship.ShipName               = Convert.ToString(ds.Tables[0].Rows[0]["ShipName"]);
                    ship.IMONumber              = Convert.ToInt32(ds.Tables[0].Rows[0]["IMONumber"]);
                    ship.FlagOfShip             = Convert.ToString(ds.Tables[0].Rows[0]["Flag"]);
                    ship.ShipEmail1             = Convert.ToString(ds.Tables[0].Rows[0]["ShipEmail"]);
                    ship.ShipEmail2             = Convert.ToString(ds.Tables[0].Rows[0]["ShipEmail2"]);
                    ship.Mobile1                = Convert.ToString(ds.Tables[0].Rows[0]["Mobile1"]);
                    ship.Mobile2                = Convert.ToString(ds.Tables[0].Rows[0]["Mobile2"]);
                    ship.Fax1                   = Convert.ToString(ds.Tables[0].Rows[0]["Fax1"]);
                    ship.Fax2                   = Convert.ToString(ds.Tables[0].Rows[0]["Fax2"]);
                    ship.Voices1                = Convert.ToString(ds.Tables[0].Rows[0]["Voices1"]);
                    ship.Voices2                = Convert.ToString(ds.Tables[0].Rows[0]["Voices2"]);
                    ship.VOIP1                  = Convert.ToString(ds.Tables[0].Rows[0]["VOIP1"]);
                    ship.VOIP2                  = Convert.ToString(ds.Tables[0].Rows[0]["VOIP2"]);
                    ship.VesselTypeId           = Convert.ToInt32(ds.Tables[0].Rows[0]["VesselTypeID"]);
                    //ship.VesselTypeName         = Convert.ToString(ds.Tables[0].Rows[0]["VesselTypeName"]);
                    ship.VesselSubTypeId        = Convert.ToInt32(ds.Tables[0].Rows[0]["VesselSubTypeID"]);
                    //ship.VesselSubTypeName      = Convert.ToString(ds.Tables[0].Rows[0]["VesselSubTypeName"]);
                    ship.VesselSubSubTypeId     = Convert.ToInt32(ds.Tables[0].Rows[0]["VesselSubSubTypeID"]);
                    //ship.VesselSubSubTypeName   = Convert.ToString(ds.Tables[0].Rows[0]["VesselSubSubTypeName"]);
                    #endregion
                    ship.ApproversCount = Convert.ToInt32(ds.Tables[1].Rows[0]["ApproversCount"]);//Added on 22nd JAN 2021 (for Approvers count checking on approver master)

                    con.Close();
                }
            }
                  
            return ship;
        }

        #region DropDown
        public List<Ship> GetAllShipForDropDown()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllShipForDrp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<Ship> shipList = myTable.AsEnumerable().Select(m => new Ship()
            {
                ID = m.Field<int>("ID"),
                ShipName = m.Field<string>("ShipName"),

            }).ToList();
            con.Close();
            shipList.Add(new Ship { ID = -1, ShipName = "Please Select One"});
            return shipList;

        }
        public List<VesselType> GetVesselTypeListForDopDown()
        {
            List<VesselType> vslTypList = new List<VesselType>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVesselTypeListForDopDown", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        vslTypList.Add(new VesselType
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Description = Convert.ToString(dr["Name"]),

                        });
                    }
                    con.Close();
                }
            }
            return vslTypList;

        }

        public List<VesselSubType> GetVesselSubTypeListByTypeForDopDown( int vesselTypeId)
        {
            List<VesselSubType> vslSubTypList = new List<VesselSubType>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVesselSubTypeListByTypeForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VesselTypeId", vesselTypeId);


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        vslSubTypList.Add(new VesselSubType
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Description = Convert.ToString(dr["Name"]),

                        });
                    }
                    con.Close();
                }
            }
            return vslSubTypList;

        }
        public List<VesselSubSubType> GetVesselSubSubTypeListBySubTypeForDopDown(int vesselSubTypeId)
        {
            List<VesselSubSubType> vslSubSubTypList = new List<VesselSubSubType>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVesselSubSubTypeListBySubTypeForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VesselSubTypeId", vesselSubTypeId);


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        vslSubSubTypList.Add(new VesselSubSubType
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Description = Convert.ToString(dr["Name"]),

                        });
                    }
                    con.Close();
                }
            }
            return vslSubSubTypList;

        }

        #endregion

        #region Manual

        public ShipManual GetManual(string controllerName, string actionName)
        {
            ShipManual file = new ShipManual();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetShipManualByControllerAction", con))
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
