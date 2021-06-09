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
    public class TicketDAL
    {
        public string SaveTicket(Ticket ticket,int userType)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveTicketDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Error", ticket.Error.ToString());
            cmd.Parameters.AddWithValue("@Description", ticket.Description.ToString());

            cmd.Parameters.AddWithValue("@FilePath", ticket.FilePath.ToString());
            cmd.Parameters.AddWithValue("@CreatedBy", 1);
            cmd.Parameters.Add("@TicketNumber", SqlDbType.VarChar,50);
            cmd.Parameters["@TicketNumber"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@UserType", userType);
            cmd.Parameters.AddWithValue("@shipId", ticket.ShipId);


            cmd.ExecuteScalar();
            string tktNumber = cmd.Parameters[4].Value.ToString();
            con.Close();

            return tktNumber;
        }


        public List<Ticket> GetAllTicketPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            List<Ticket> pOList = new List<Ticket>();
            List<Ticket> equipmentsPO = new List<Ticket>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stpGetAllTicketPageWise", con))
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
                        pOList.Add(new Ticket
                        {
                            //Id = Convert.ToInt32(dr["Id"]),
                            TicketNumber = Convert.ToString(dr["TicketNumber"]),
                            Error = Convert.ToString(dr["Error"]),
                            Description = Convert.ToString(dr["Description"]),
                            IsSolved = Convert.ToInt32(dr["IsSolved"])
                        });
                    }
                    recordCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }
    }
}
