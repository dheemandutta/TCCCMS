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
        public static void UpdateLog(string Message, LogMessageType MessageType,string messageSource)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpSaveOperationLog", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Message", Message.ToString());
                     
            cmd.Parameters.AddWithValue("@MessageType", Enum.GetName(typeof(LogMessageType),MessageType));

            cmd.Parameters.AddWithValue("@MessageSource", messageSource);

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            //return recordsAffected;
        }


    }

    public enum LogMessageType
    {
        Info,
        Warn,
        Error
    }
}
