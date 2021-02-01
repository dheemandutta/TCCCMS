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
    public class ImportBL
    {
        public void ImportCrew(object dataTable, int vesselId)
        {
            ImportDAL crewImportDAL = new ImportDAL();
            crewImportDAL.ImportCrew(dataTable, vesselId);
        }
    }
}
