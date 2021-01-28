using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    //[TraceFilterAttribute]
    public class ImportController : Controller
    {
        // GET: Import
        //[TraceFilterAttribute]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[TraceFilterAttribute]
        //public ActionResult Index(HttpPostedFileBase postedFile)
        //{
        //    object dtData = new object();
        //    CrewImportBL crewImportBL = new CrewImportBL();

        //    if (postedFile != null)
        //    {
        //        //string path = Server.MapPath("~/Uploads/");
        //        string path = Server.MapPath(ConfigurationManager.AppSettings["CrewUploadPath"].ToString());
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string fileName = string.Empty;
        //        fileName = Path.GetFileName(postedFile.FileName);

        //        if (System.IO.File.Exists(path + fileName))
        //        {
        //            System.IO.File.Delete(path + fileName);
        //        }

        //        postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
        //        ViewBag.Message = "File uploaded successfully.";

        //        string filePath = path + fileName;

        //        // read file 
        //        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //        {

        //            // Auto-detect format, supports:
        //            //  - Binary Excel files (2.0-2003 format; *.xls)
        //            //  - OpenXml Excel files (2007 format; *.xlsx)
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {

        //                // 2. Use the AsDataSet extension method
        //                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        //                {

        //                    // Gets or sets a callback to determine which row is the header row. 
        //                    // Only called when UseHeaderRow = true.
        //                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
        //                    {
        //                        UseHeaderRow = false,// Use first row is ColumnName here :D
        //                        FilterRow = rowHeader => rowHeader.Depth >= 10
        //                    }
        //                });

        //                if (dataSet.Tables.Count > 0)
        //                {
        //                    dtData = dataSet.Tables[0];
        //                    // Do Something
        //                }
        //                // The result of each spreadsheet is in result.Tables
        //            }
        //            crewImportBL.ImportCrew(dtData, int.Parse(Session["VesselID"].ToString()));        ////////////////////////////////////////
        //        }
        //    }

        //    return View();
        //}
   
    }
}