using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using Ionic.Zip;
using Quartz;
using System.Threading;
using TCCCMS.LOG;
using TCCCMS.CryptoUtility;

namespace TCCCMS.Admin.ExportData
{

    public class Export : IJob
    {
        static String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "xml");
        static String zippath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "ZipFile");
        static String ziparchivePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "Archive");
        public static bool isMailSendSuccessful = false;
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static int shipId = 0;

        public async Task Execute(IJobExecutionContext context)
        {
            logger.Info("Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Export Process Started", LogMessageType.Info, "Admin Export");
            if (ZipDirectoryContainsFiles())
            {
                TccLog.UpdateLog("Send Mail Process Started", LogMessageType.Info, "Admin Export");
                SendMail();
                if (isMailSendSuccessful)
                {
                    shipId = 0;
                    TccLog.UpdateLog("ArchiveZipFiles Process Started", LogMessageType.Info, "Admin Export");
                    ArchiveZipFiles();
                    TccLog.UpdateLog("ArchiveZipFiles Process Completed", LogMessageType.Info, "Admin Export");
                    //redo the whole process again
                    isMailSendSuccessful = false;
                    TccLog.UpdateLog("Export Process Started", LogMessageType.Info, "Admin Export");
                    ExportData();
                    TccLog.UpdateLog("Export Process Completed", LogMessageType.Info, "Admin Export");
                    //TccLog.UpdateLog("Zip Process Started", LogMessageType.Info, "Admin Export");
                    //CreateZip();
                    //TccLog.UpdateLog("Zip Process Completed", LogMessageType.Info, "Admin Export");
                    TccLog.UpdateLog("SendMail Process Started", LogMessageType.Info, "Admin Export");
                    SendMail();
                    TccLog.UpdateLog("SendMail Process Completed", LogMessageType.Info, "Admin Export");
                    if (isMailSendSuccessful)
                    {
                        TccLog.UpdateLog("Archive Process Started", LogMessageType.Info, "Admin Export");
                        ArchiveZipFiles();
                        TccLog.UpdateLog("Archive Process Completed", LogMessageType.Info, "Admin Export");
                    }
                }
            }
            else
            {
                shipId = 0;
                isMailSendSuccessful = false;
                TccLog.UpdateLog("Export Process Started", LogMessageType.Info, "Admin Export");
                ExportData();
                TccLog.UpdateLog("Export Process Completed", LogMessageType.Info, "Admin Export");
                //CreateZip();
                TccLog.UpdateLog("SendMail Process Started", LogMessageType.Info, "Admin Export");
                SendMail();
                TccLog.UpdateLog("SendMail Process Completed", LogMessageType.Info, "Admin Export");
                if (isMailSendSuccessful)
                {
                    TccLog.UpdateLog("Archive Process Started", LogMessageType.Info, "Admin Export");
                    ArchiveZipFiles();
                    TccLog.UpdateLog("Archive Process Completed", LogMessageType.Info, "Admin Export");
                }
            }

        }

        public static bool ZipDirectoryContainsZipFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(zippath + "\\");
                return di.GetFiles("*.zip").Length > 0;
            }
            catch (Exception ex)
            {

                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ZipDirectoryContainsZipFiles.");
                TccLog.UpdateLog("Directory not found", LogMessageType.Error, "Admin Export");
                TccLog.UpdateLog("Export process terminated unsuccessfully in ZipDirectoryContainsZipFiles", LogMessageType.Info, "Admin Export");
                return false;
                //Environment.Exit(0);
            }
        }

        public static string GetConfigData(string KeyName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpGetAlltblConfig", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@KeyName", KeyName);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds.Tables[0].Rows[0]["ConfigValue"].ToString();

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["Crewxml"].ToString(), XmlWriteMode.WriteSchema);
            //}
        }

        private static string GetShipEmail(int shipId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetShipEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ShipId", shipId);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds.Tables[0].Rows[0]["ShipEmail"].ToString();
        }

        public static string GetZipFileName()
        {
            if (ZipDirectoryContainsZipFiles())
            {
                DirectoryInfo di = new DirectoryInfo(zippath + "\\");
                return di.GetFiles("*.zip")[0].Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void ArchiveZipFiles()
        {
            string sourceFilePath = zippath + "\\";
            string destinationFilePath = ziparchivePath + "\\";

            try
            {
                string[] sourceFiles = Directory.GetFiles(sourceFilePath);

                foreach (string sourceFile in sourceFiles)
                {
                    string fName = Path.GetFileName(sourceFile);
                    string destFile = Path.Combine(destinationFilePath, fName);

                    File.Move(sourceFile, destFile);
                }
                TccLog.UpdateLog("ArchiveZipFiles Process Complete", LogMessageType.Info, "Admin Export");
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Directory not found Admin Export-ArchiveZipFiles");
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export process terminated unsuccessfully in Admin Export-ArchiveZipFiles");
                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ArchiveZipFiles.");
                Environment.Exit(0);
            }


        }

        public static bool ZipDirectoryContainsFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(zippath + "\\");
                return di.GetFiles("*.zip").Length > 0;
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export - ZipDirectoryContainsZipFiles");
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export process terminated unsuccessfully in  Admin Export - ZipDirectoryContainsZipFiles");
                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ZipDirectoryContainsZipFiles.");
                return false;
                //Environment.Exit(0);
            }
        }
        public static bool RevisionDirectoryContainsFiles()
        {
            try
            {
                //string sourcePath = @"C:\\inetpub\\wwwroot\\TCCCMS";
                string sourcePath = ConfigurationManager.AppSettings["revisionPath"].ToString();
                DirectoryInfo di = new DirectoryInfo(sourcePath);
                return di.GetFiles("*.*").Length > 0;
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export - RevisionDirectoryContainsFiles");
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export process terminated unsuccessfully in  Admin Export - RevisionDirectoryContainsFiles");
                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ZipDirectoryContainsZipFiles.");
                return false;
                //Environment.Exit(0);
            }
        }
        public static void CreateZip()
        {

            try
            {
                //    string[] xmlPaths = Directory.GetFiles(path + "\\");

                //    foreach (string xmlfilePath in xmlPaths)
                //    {
                //        //xml file copy to temp folder and then zip that file
                //        string xmlFile = Path.GetFileName(xmlfilePath);
                //        string tmpxPath = Path.Combine(Path.GetDirectoryName(xmlfilePath), "temp");
                //        File.Copy(xmlfilePath, Path.Combine(tmpxPath, xmlFile));

                //        string fileName = Path.GetFileNameWithoutExtension(xmlfilePath);
                //        fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                //        fileName = fileName + ".zip";
                //        using (ZipFile zip = new ZipFile())
                //        {
                //            zip.AddDirectory(tmpxPath + "\\");
                //            zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                //            zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                //            zip.Save(zippath + "\\" + fileName);

                //            //Delete file from temp foldes
                //            File.Delete(Path.Combine(tmpxPath, xmlFile));
                //        }

                //        File.Delete(xmlfilePath);

                //    }
                //-------------------------------------------------------------------------------------
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                ////int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());
                //con.Open();
                //SqlCommand cmd = new SqlCommand("GetShipDetailsById", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ShipId", shipId);
                //DataSet ds = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(ds);
                //string fileName = ds.Tables[0].Rows[0]["IMONumber"].ToString();
                //string fileName = ds.Tables[0].Rows[0]["ID"].ToString();
                string fileName = shipId.ToString();
                fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                fileName = fileName + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(path + "\\");
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                    zip.Save(zippath + "\\" + fileName);
                    // SegmentsCreated = zip.NumberOfSegmentsForMostRecentSave;
                }

                //delete xml files 
                //string[] filePaths = Directory.GetFiles(path + "\\");
                //foreach (string filePath in filePaths)
                //    File.Delete(filePath);


            }
            catch (Exception ex)
            {


                logger.Error("Error in CreateZip. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in CreateZip.");
                //Environment.Exit(0);
            }
        }

        public static void CreateUploadedFileZip(string partName, string xmlFile)
        {
            try
            {
                TccLog.UpdateLog("Uploaded File Zip Creation Started", LogMessageType.Info, "Admin Export-CreateUploadedFileZip");
                logger.Info("Uploaded File Zip Creation Started.- {0}", DateTime.Now.ToString());

                //string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();
                string tmpPath = Path.Combine(Path.GetDirectoryName(path), "temp");


                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    string sourcePath = @"C:\\inetpub\\wwwroot\\TCCCMS";
                    string uploadedFileName = string.Empty;
                    string relPath = string.Empty;
                    string filePath = string.Empty;
                    if (partName == "TICKET")
                    {
                        filePath = row["FilePath"].ToString();
                        uploadedFileName = Path.GetFileName(filePath);
                    }
                    else
                    {
                        filePath = row["FormsPath"].ToString();
                        uploadedFileName = row["FormsName"].ToString();
                    }

                    relPath = Path.GetDirectoryName(filePath);
                    relPath = relPath.Replace("~", "").Replace("/", "");

                    //sourcePath = Path.Combine(sourcePath, relPath);
                    sourcePath = sourcePath + relPath;
                    sourcePath = Path.Combine(sourcePath, uploadedFileName);
                    if (File.Exists(sourcePath))
                        File.Copy(sourcePath, Path.Combine(tmpPath, uploadedFileName));



                }

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                ////int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());
                //con.Open();
                //SqlCommand cmd = new SqlCommand("GetShipDetailsById", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ShipId", shipId);
                //DataSet ds = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(ds);
                //string fileName = ds.Tables[0].Rows[0]["IMONumber"].ToString();
                //string zipName = ds.Tables[0].Rows[0]["ID"].ToString();
                string zipName = shipId.ToString();
                //zipName = zipName + "_TICKET_" + DateTime.Now.ToString("MMddyyyyhhmm");
                zipName = zipName + "_" + partName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                zipName = zipName + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(tmpPath + "\\");
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                    zip.Save(path + "\\" + zipName);//---Create Zip of Uploaded file in 'xml' folder. ---
                    // SegmentsCreated = zip.NumberOfSegmentsForMostRecentSave;
                }

                //delete xml files 
                string[] filePaths = Directory.GetFiles(tmpPath + "\\");

                foreach (string filePath in filePaths)

                    File.Delete(filePath);


                ///------------------------------------------------

            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-CreateUploadedZipFile");
                logger.Error("Error in CreateZip. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in CreateZip.");
                //Environment.Exit(0);
            }
        }

        public static void CreateUpdateRevisionZip()
        {
            try
            {
                TccLog.UpdateLog("Revision File Zip Creation Started", LogMessageType.Info, "Admin Export-CreateUpdateRevisionZip");
                logger.Info("Revision File Zip Creation Started.- {0}", DateTime.Now.ToString());

                //string revisionPath = @"C:\\inetpub\\wwwroot\\TCCCMS" + "\\" + "UpdateRevisions" + "\\";
                string revisionPath = ConfigurationManager.AppSettings["revisionPath"].ToString() +"\\";
                string fileName = ConfigurationManager.AppSettings["revisionfilesuffixname"].ToString();
                fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                fileName = fileName + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(revisionPath);
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                    zip.Save(path + "\\" + fileName);//Zip file will be created into xml folder
                    // SegmentsCreated = zip.NumberOfSegmentsForMostRecentSave;
                }
            }
            catch(Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-CreateUpdateRevisionZip");
                logger.Error("Error in CreateZip. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in CreateZip.");
            }
            
        }

        #region Export Data From DB
        public static void ExportData()
        {
            try
            {
                DataSet dsVessels = new DataSet();
                dsVessels = GetRegistrerdVessels();
                for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
                {
                    shipId = int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString());
                    TccLog.UpdateLog("Export process started for Ship_"+ shipId, LogMessageType.Info, "Admin Export");

                    Ticket();
                    logger.Info("Ticket Export Complete. - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog("Ticket Export Completed", LogMessageType.Info, "Admin Export");
                    RevisionHeader();
                    logger.Info("Revision Header Export Complete. - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog("Revision Header Export Complete", LogMessageType.Info, "Admin Export");
                    RevisionDetails();
                    logger.Info("Revision History Export Complete. - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog("Revision Hostory Export Complete", LogMessageType.Info, "Admin Export");
                    FillupFormsUploaded();
                    logger.Info("Fillup Forms  Export Complete. - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog("Fillup Forms Export Complete", LogMessageType.Info, "Admin Export");
                    FillupFormApproverMapper();
                    logger.Info("Fillup Form Approver Mapper Export Complete. - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog("Fillup form approver mapper export complete", LogMessageType.Info, "Admin Export");

                    TccLog.UpdateLog("Export process Completed for Ship_" + shipId, LogMessageType.Info, "Admin Export");

                    if (RevisionDirectoryContainsFiles())
                    {
                        CreateUpdateRevisionZip();
                    }

                    TccLog.UpdateLog("Zip process started for Ship_" + shipId, LogMessageType.Info, "Admin Export");
                    CreateZip();
                    TccLog.UpdateLog("Zip process completed for Ship_" + shipId, LogMessageType.Info, "Admin Export");

                    UpdateExportedData(shipId);
                    TccLog.UpdateLog("Update Exported Data Completed --Ship_"+ shipId, LogMessageType.Info, "Admin Export");
                }

               
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-ExportData");
                logger.Error("Error in ExportData. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ExportData.");
                //Environment.Exit(0);
            }
        }

        public static void Ticket()
        {
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            //con.Open();
            ////SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
            //SqlCommand cmd = new SqlCommand("stpExportTicketFromAdmin", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString(), XmlWriteMode.WriteSchema);
            //}
            //con.Close();


            //----------------------------------------way 2-----------------------------------------------------------------------------
            //DataSet dsVessels = new DataSet();
            //dsVessels = GetRegistrerdVessels();

            //for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
            //{
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            //    con.Open();
            //    // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
            //    SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@ShipId", int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString()));
            //    DataSet ds = new DataSet();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(ds);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
            //        ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString(), XmlWriteMode.WriteSchema);
            //    }
            //    con.Close();
            //}

            //-----------------------way 3------------------------------------

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
            SqlCommand cmd = new SqlCommand("stpExportTicketFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void FillupFormsUploaded()
        {
            //----------------------------Way 1--------Not used--------------------------------------------------------------------------
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            //con.Open();
            ////SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            //SqlCommand cmd = new SqlCommand("stpExportFillupUoloadedFormsFromAdmin", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString(), XmlWriteMode.WriteSchema);
            //}
            //con.Close();

            //------------------------------------------------Way 2---------- not used-----------------------------------------------------------
            //DataSet dsVessels = new DataSet();
            //dsVessels = GetRegistrerdVessels();

            //for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
            //{
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            //    con.Open();
            //    // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExportFillupUoloadedFormsFromAdmin"
            //    SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@ShipId", int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString()));
            //    DataSet ds = new DataSet();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(ds);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
            //        ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString(), XmlWriteMode.WriteSchema);
            //    }
            //    con.Close();
            //}

            //-----------------------------------Way 3-------------------------------------------------------------------
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
            SqlCommand cmd = new SqlCommand("stpExportFillupUoloadedFormsFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();

            string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();
            if(File.Exists(xmlFile))
            {
                CreateUploadedFileZip("FILLUPUPLOADEDFILE", xmlFile);
            }
            
        }

        public static void FillupFormApproverMapper()
        {
            //----------------------------Way 1---------------------------------------------------------------------------------
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormsUploadedApproverMapping", con);
            SqlCommand cmd = new SqlCommand("stpExportFillupFormApproverFromAdmin", con);
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();

            //---------------------------------Way 2 ---------Alternate Code---------------------------------------------------------------------------
            //DataSet dsVessels = new DataSet();
            //dsVessels = GetRegistrerdVessels();

            //for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
            //{
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            //    con.Open();
            //    // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
            //    SqlCommand cmd = new SqlCommand("stpExportFillupFormApproverFromAdmin", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@ShipId", int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString()));
            //    DataSet ds = new DataSet();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(ds);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
            //        ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString(), XmlWriteMode.WriteSchema);
            //    }
            //    con.Close();
            //}
        }

        public static void RevisionHeader()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblRevisionViewerFromShip", con);
            SqlCommand cmd = new SqlCommand("stpExportRevisionHeaderFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHeader"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void RevisionDetails()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblRevisionViewerFromShip", con);
            SqlCommand cmd = new SqlCommand("stpExportRevisionHistoryFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShipId", shipId);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHistory"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        #endregion

        #region Sending Mail


        public static void SendMail()
        {
            logger.Info("Sending Mail Process Start. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Send Mail Process Started", LogMessageType.Info, "Admin Export");

            //string sourceFilePath = path + "\\"; // old code line
            string sourceFilePath = zippath + "\\";
            string[] sourceFiles = Directory.GetFiles(sourceFilePath);

            string adminEmail = GetConfigData("admincenteremail").Trim();
            string adminEmailpwd = GetConfigData("admincenteremailpwd").Trim();
            //string adminEmailpwd = EncodeDecode.DecryptString(GetConfigData("admincenteremailpwd"));

            foreach (string sourceFile in sourceFiles)
            {
                //read file name
                string fName = Path.GetFileName(sourceFile);

                //string vesselIMO    = Path.GetFileNameWithoutExtension(sourceFile);// old code line
                //extract IMO number 
                string[] vesselIMO = fName.Split('_');

                string shipEmail = string.Empty;
                shipEmail = GetShipEmail(int.Parse(vesselIMO[0].ToString()));

                try
                {
                    using (MailMessage mail = new MailMessage())
                    {


                        //mail.Subject = "RHDATASYNC";
                        mail.Subject = GetConfigData("tccAsubject");

                        //mail.From = new MailAddress(GetConfigData("mailfrom"));
                        mail.From = new MailAddress(adminEmail);
                        mail.To.Add(shipEmail);

                        if (ZipDirectoryContainsFiles())
                        {
                            mail.Attachments.Add(new Attachment(sourceFile));
                        }



                        SmtpClient smtp = new SmtpClient(GetConfigData("smtp"));
                        smtp.EnableSsl = true;
                        smtp.Port = int.Parse(GetConfigData("port"));

                        //smtp.Credentials = new System.Net.NetworkCredential(GetConfigData("mailfrom").Trim(), GetConfigData("frompwd").Trim());
                        //smtp.Credentials = new System.Net.NetworkCredential(adminEmail, adminEmailpwd);
                        smtp.Credentials = new System.Net.NetworkCredential("cableman24x7@gmail.com", "cableman24x712345");

                        smtp.Send(mail);
                        logger.Info("Mail send Successfully to the Vessel_" + vesselIMO[0].ToString() + ". - {0}", DateTime.Now.ToString());
                        TccLog.UpdateLog("Mail Sent Successfully to the Ship_" + vesselIMO[0].ToString(), LogMessageType.Info, "Admin Export - SendMail");
                        isMailSendSuccessful = true;
                    }

                }
                catch (Exception ex)
                {
                    TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-SendMail");
                    isMailSendSuccessful = false;
                    logger.Error("Mail send failed - {0}", DateTime.Now.ToString(), ex.Message + " :" + ex.InnerException);
                    logger.Info("Export process terminated unsuccessfully. - {0}", DateTime.Now.ToString());
                    // Environment.Exit(0);
                }


            }



        }
        //------------------------------------------------------------------------------------------------
        //public static void SendMail()
        //{

        //    try
        //    {
        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.Subject = GetConfigData("tccAsubject");
        //            mail.From = new MailAddress(GetConfigData("mailfrom"));

        //            mail.To.Add(GetConfigData("mailto"));

        //            if (ZipDirectoryContainsZipFiles())
        //            {
        //                mail.Attachments.Add(new Attachment(zippath + "\\" + GetZipFileName()));
        //            }

        //            SmtpClient smtp = new SmtpClient(GetConfigData("smtp"));
        //            smtp.EnableSsl = true;
        //            smtp.Port = int.Parse(GetConfigData("port"));
        //            smtp.Credentials = new System.Net.NetworkCredential(GetConfigData("mailfrom").Trim(), GetConfigData("frompwd").Trim());

        //            smtp.Send(mail);

        //            isMailSendSuccessful = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
        //        isMailSendSuccessful = false;
        //        logger.Error("Mail send failed - {0}", ex.Message + " :" + ex.InnerException);
        //        logger.Info("Export process terminated unsuccessfully.");
        //        Environment.Exit(0);
        //    }

        //}


        #endregion

        private static DataSet GetRegistrerdVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
                SqlCommand cmd = new SqlCommand("stpGetAllRegisteredVessels", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-GetRegistrerdVessels");
            }

            return ds;
        }


        public static void UpdateExportedData(int aShipId)
        {
            logger.Info("Import Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Update Export Data Process Started", LogMessageType.Info, "UpdateExportedData");
            try
            {
                UpdateTicket();
                TccLog.UpdateLog("Update Ticket IsExport Succesfully", LogMessageType.Info, "Admin Export-UpdateTicket");
                UpdateRevisionHeader(aShipId);
                TccLog.UpdateLog("Update RevisionHeader IsExport Succesfully", LogMessageType.Info, "Admin Export-UpdateRevisionHeader");
                UpdateRevisionDetails(aShipId);
                TccLog.UpdateLog("Update RevisionDetails IsExport Succesfully", LogMessageType.Info, "Admin Export-UpdateRevisionDetails");
                UpdateFillupFormsUploaded();
                TccLog.UpdateLog("Update FillupFormsUploaded IsExport Succesfully", LogMessageType.Info, "Admin Export-UpdateFillupFormsUploaded");
                UpdateFillupFormApproverMapper();
                TccLog.UpdateLog("Update FillupFormApproverMapper IsExport Succesfully", LogMessageType.Info, "Admin Export-UpdateFillupFormApproverMapper");

                // delete all xml files
                string[] xmlfilePaths = Directory.GetFiles(path + "\\");
                foreach (string filePath in xmlfilePaths)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-UpdateFillupFormApproverMapper");
                logger.Error("Error in ExportData. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ExportData.");
                //Environment.Exit(0);
            }
        }
        public static void UpdateTicket()
        {
            try
            {
                // Here your xml file
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateTicketExportInShip", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    //cmd.Parameters.AddWithValue("@ID", int.Parse(row["ID"].ToString()));

                    string uploadedFileName = string.Empty;
                    string relativePath = string.Empty;
                    string filePath = string.Empty;

                    cmd.Parameters.AddWithValue("@TicketNumber", row["TicketNumber"].ToString());
                   
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-UpdateTicket");
                logger.Error(ex, "Ticket Import");
                //throw;
            }
        }

        public static void UpdateRevisionHeader(int aShipId)
        {
            try
            {
                // Here your xml file
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHeader"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateRevisionHeaderExportInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@RevisionId", int.Parse(row["Id"].ToString()));
                    cmd.Parameters.AddWithValue("@ShipId", aShipId);

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-UpdateRevisionHeader");
                logger.Error(ex, "Crew Import");
                //throw;
            }
        }

        public static void UpdateRevisionDetails(int aShipId)
        {
            try
            {
                // Here your xml file
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHistory"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateRevisionHistoryExportInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@RevisionHistoryId", int.Parse(row["RevisionHistoryId"].ToString()));
                    cmd.Parameters.AddWithValue("@RevisionId", int.Parse(row["HeaderId"].ToString()));
                    cmd.Parameters.AddWithValue("@ShipId", aShipId);

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-UpdateRevisionDetails");
                logger.Error(ex, "Admin Export-UpdateRevisionDetails");
                //throw;
            }
        }

        public static void UpdateFillupFormsUploaded()
        {
            try
            {
                // Here your xml file
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();
                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateFormsUploadExportInShip", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@FormsName", row["FormsName"].ToString());
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }

            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-UpdateFillupFormsUploaded");
                logger.Error(ex, "Admin Export-UpdateFillupFormsUploaded");
                //throw;
            }
        }

        public static void UpdateFillupFormApproverMapper()
        {
            try
            {
                // Here your xml file
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateFormsUploadApproverMappingExportInShip", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@FormsName", row["UploadedFormName"].ToString());
                    cmd.Parameters.AddWithValue("@ApproverUserId", int.Parse(row["ApproverUserId"].ToString()));

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Export-UpdateFillupFormApproverMapper");
                logger.Error(ex, "Admin Export-UpdateFillupFormApproverMapper");
                //throw;
            }
        }
    }

}
