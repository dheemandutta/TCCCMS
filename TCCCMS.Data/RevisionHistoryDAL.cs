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



        public int SaveRevisionHistory(RevisionHistory pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveRevisionHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Chapter", pOCO.Chapter.ToString());
            cmd.Parameters.AddWithValue("@Section", pOCO.Section.ToString());
            cmd.Parameters.AddWithValue("@ChangeComment", pOCO.ChangeComment.ToString());
            cmd.Parameters.AddWithValue("@ModificationDate", pOCO.ModificationDate);
            cmd.Parameters.AddWithValue("@HeaderId", pOCO.HeaderId);

            if (pOCO.ID > 0)
            {
                cmd.Parameters.AddWithValue("@RevisionHistoryId", pOCO.ID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RevisionHistoryId ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }


        public RevisionHeaderHistoryViewModel GetAllRevisionDetails()
        {
            RevisionHeaderHistoryViewModel rhhVM = new RevisionHeaderHistoryViewModel();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRevisionDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    List<RevisionHeader> rHeaderList = new List<RevisionHeader>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RevisionHeader rHeader = new RevisionHeader();
                        rHeader.Id              = Convert.ToInt32(dr["Id"]);
                        rHeader.RevisionNo      = Convert.ToString(dr["RevisionNo"]);
                        rHeader.RevisionDate    = Convert.ToString(dr["RevisionDate"]);

                        var histRows = ds.Tables[1].Rows
                              .Cast<DataRow>()
                              .Where(x => x.Field<int>(5) == rHeader.Id).ToList();

                        List<RevisionHistory> rhList = new List<RevisionHistory>();
                        foreach (DataRow drh in histRows)
                        {
                            RevisionHistory rh  = new RevisionHistory();
                            rh.ID               = Convert.ToInt32(drh["RevisionHistoryId"]);
                            rh.Chapter          = Convert.ToString(drh["Chapter"]);
                            rh.Section          = Convert.ToString(drh["Section"]);
                            rh.ChangeComment    = Convert.ToString(drh["ChangeComment"]);
                            rh.ModificationDate = Convert.ToString(drh["ModificationDate"]);
                            rh.HeaderId         = Convert.ToInt32(drh["HeaderId"]);

                            rhList.Add(rh);
                        }
                        rHeader.RevisionHistoryList = rhList;


                        rHeaderList.Add(rHeader);
                    }
                    rhhVM.RevisionHeaderList = rHeaderList;
                }

            }


                return rhhVM;

        }

        public List<RevisionHeader> GetRevisionHeaderForDrp()
        {
            List<RevisionHeader> rHeaderList = new List<RevisionHeader>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRevisionHeaderForDropDown", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RevisionHeader rHeader = new RevisionHeader();
                        rHeader.Id = Convert.ToInt32(dr["Id"]);
                        //rHeader.RevisionNo = Convert.ToString(dr["RevisionNo"]);
                        //rHeader.RevisionDate = Convert.ToString(dr["RevisionDate"]);
                        rHeader.RevisionName = Convert.ToString(dr["RevisionNo"]) + " " + Convert.ToString(dr["RevisionDate"]);

                        rHeaderList.Add(rHeader);
                    }
                }

            }

            return rHeaderList;

        }

        public int SaveRevisionHeader(RevisionHeader rHeader)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveRevisionHeader", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RevisionNo", rHeader.RevisionNo.ToString());
            cmd.Parameters.AddWithValue("@RevisionDate", rHeader.RevisionDate.ToString());
            cmd.Parameters.AddWithValue("@CreatedBy", 1);

            if (rHeader.Id > 0)
            {
                cmd.Parameters.AddWithValue("@RevisionHeaderId", rHeader.Id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RevisionHeaderId ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }

    }
}
