using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.LOG
{
    public class TccLog
    {
        public static void UpdateLog(string Message, DateTime OperationDate, string MessageType)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveOperationLog", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Message", Message.ToString());

            //if (!String.IsNullOrEmpty(OperationDate))
            //{
            //    cmd.Parameters.AddWithValue("@OperationDate", OperationDate.ToString());
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@OperationDate", DBNull.Value);
            //}

            if (!String.IsNullOrEmpty(MessageType))
            {
                cmd.Parameters.AddWithValue("@MessageType", MessageType.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@MessageType", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            //return recordsAffected;
        }


    }
}
