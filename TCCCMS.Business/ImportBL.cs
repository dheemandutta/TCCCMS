using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Configuration;
using System.Data;
//using System.Data.SqlClient;
using TCCCMS.Models;
using TCCCMS.Data;

namespace TCCCMS.Business
{
    public class ImportBL
    {
        public void ImportCrew(object dataTable, string shipNumber)
        {
            ImportDAL crewImportDAL = new ImportDAL();
            //crewImportDAL.ImportCrew(dataTable, shipNumber);
            crewImportDAL.ImportTemporaryCrew(dataTable, shipNumber);
        }

        public TemporaryCrewViewModel GetAllTemporaryCrews()
        {
            ImportDAL crewImportDAL = new ImportDAL();
            return crewImportDAL.GetAllTemporaryCrews();
        }

        public string ImportCrewList(DataTable dt, ref string FailureMessage, ref int FailureCount, ref int SuccessCount)
        {
            string s = "";

            ImportDAL importDal = new ImportDAL();

            DataTable dtBL = new DataTable();
            foreach(DataRow dr in dt.Rows)
            {
                string sr = dr[1].ToString();
                string st = sr.Replace("\\", "");
            }

            s = importDal.ImportCrewList(dt, ref FailureMessage, ref FailureCount, ref SuccessCount);


            return s;
        }
    }
}
