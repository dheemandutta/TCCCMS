using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
using TCCCMS.Models;
using TCCCMS.Data;


namespace TCCCMS.Business
{
    public class LogForUserExcelBL
    {
        public LogForUserExcelPOCO GetLogForUserExcel(/*int Id*/)
        {
            LogForUserExcelDAL dAL = new LogForUserExcelDAL();
            return dAL.GetLogForUserExcel(/*Id*/);
        }

        public int SaveUpdateRank(LogForUserExcelPOCO pOCO)
        {
            LogForUserExcelDAL dAL = new LogForUserExcelDAL();
            return dAL.SaveLogForUserExcel(pOCO);
        }
    }
}
