using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using ExcelDataReader;
using System.Data;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace TCCCMS.Controllers
{
    //[TraceFilterAttribute]
    public class ImportController : Controller
    {
        // GET: Import
        //[TraceFilterAttribute]
        public ActionResult Index()
        {
            if(Session["Role"].ToString() == "SupportUser" || Session["Role"].ToString() == "ShipAdmin")
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Home");
            
        }

        [HttpPost]
        //[TraceFilterAttribute]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            object dtData = new object();
            ImportBL crewImportBL = new ImportBL();
            int i = 0;
            if (postedFile != null)
            {
                //string path = Server.MapPath("~/Uploads/");
                string path = Server.MapPath(ConfigurationManager.AppSettings["CrewUploadPath"].ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = string.Empty;
                fileName = Path.GetFileName(postedFile.FileName);

                if (System.IO.File.Exists(path + fileName))
                {
                    System.IO.File.Delete(path + fileName);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";

                string filePath = path + fileName;

                // read file 
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        // 2. Use the AsDataSet extension method
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {

                            // Gets or sets a callback to determine which row is the header row. 
                            // Only called when UseHeaderRow = true.
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = false,// Use first row is ColumnName here :D
                                FilterRow = rowHeader => rowHeader.Depth >= 2
                            }
                        });

                        if (dataSet.Tables.Count > 0)
                        {
                            dtData = dataSet.Tables[0];
                            // Do Something
                        }
                        // The result of each spreadsheet is in result.Tables
                    }
                  i=  crewImportBL.ImportCrew(dtData, ConfigurationManager.AppSettings["ShipNumber"]); 
                    
                }
            }
            if (i > 0)
            {
                return RedirectToAction("ImportCrew","Import");
            }
            else
                return View();
        }


        public ActionResult ImportCrew()
        {
            ImportBL imBl = new ImportBL();
            TemporaryCrewViewModel crewImportVM = new TemporaryCrewViewModel();
            if (Session["Role"].ToString() == "SupportUser" || Session["Role"].ToString() == "ShipAdmin")
            {
                crewImportVM = imBl.GetAllTemporaryCrews();

                return View(crewImportVM);
            }
            else
                return RedirectToAction("Login", "Home");
            
            
        }

        [HttpPost]
       
        public ActionResult ImportCrewList(object crews)
        {
            ImportBL importBl = new ImportBL();
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(crews);
            DataTable dt = JsonStringToDataTable(jsonString);
            string FailureMessage ="";
            int FailureCount = 0;
            int SuccessCount = 0;

            string s = importBl.ImportCrewList(dt,ref FailureMessage,ref FailureCount,ref SuccessCount);
            if(FailureMessage != "" || FailureCount != 0 || SuccessCount != 0)
            {
               return RedirectToAction("LogForUserExcel", "LogForUserExcel");
            }
            else
            {
                return Json("File Uploaded Successfully!");
            }

            
        }

        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "").Replace("\\","");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            string s = ColumnsNameString.Replace("\"", "");
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName.Replace("\"", "").Replace("\\",""));
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "").Replace("\\", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Replace("\\", "");
                        nr[RowColumns] = RowDataString.Replace("\"", "").Replace("\\", "");
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

    }
}