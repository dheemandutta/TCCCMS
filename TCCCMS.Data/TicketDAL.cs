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
        public int SaveTicket(Ticket ticket)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveTicketDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Error", ticket.Error.ToString());
            cmd.Parameters.AddWithValue("@Description", ticket.Description.ToString());

            cmd.Parameters.AddWithValue("@FilePath", ticket.FilePath.ToString());
            cmd.Parameters.AddWithValue("@CreatedBy", 1);
            cmd.Parameters.Add("@TicketNumber", SqlDbType.Int);
            cmd.Parameters["@TicketNumber"].Direction = ParameterDirection.Output;

            
            cmd.ExecuteScalar();
            int tktNumber = Convert.ToInt32(cmd.Parameters[4].Value);
            con.Close();

            return tktNumber;
        }
    }
}
