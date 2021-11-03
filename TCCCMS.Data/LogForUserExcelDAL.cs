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
    public class LogForUserExcelDAL
    {
        public LogForUserExcelPOCO GetLogForUserExcel(/*int Id*/)
        {
            LogForUserExcelPOCO prodPOList = new LogForUserExcelPOCO();
            LogForUserExcelPOCO prodPO = new LogForUserExcelPOCO();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetLogForUserExcel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToLogForUserExcelList(ds);
        }

        private LogForUserExcelPOCO ConvertDataTableToLogForUserExcelList(DataSet ds)
        {
            LogForUserExcelPOCO pPOCOPC = new LogForUserExcelPOCO();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //if (item["Id"] != null)
                    //    pPOCOPC.Id = Convert.ToInt32(item["Id"].ToString());

                    if (item["LogData"] != System.DBNull.Value)
                        pPOCOPC.LogData = item["LogData"].ToString();

                    if (item["Count"] != System.DBNull.Value)
                        pPOCOPC.Count = item["Count"].ToString();

                    //pcList.Add(pPOCOPC);
                }
            }
            return pPOCOPC;
        }



        public int SaveLogForUserExcel(LogForUserExcelPOCO pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveLogForUserExcel", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LogData", pOCO.LogData.ToString());

            if (pOCO.Id > 0)
            {
                cmd.Parameters.AddWithValue("@Id ", pOCO.Id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Id ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }

    }
}
