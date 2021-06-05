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

namespace TCCCMS.Ship.ExportData
{
    public class Export : IJob
    {
        static String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "xml");
        static String zippath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "ZipFile");
        static String ziparchivePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "Archive");
        public static bool isMailSendSuccessful = false;
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            logger.Info("Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Export Process Started", LogMessageType.Info, "Export");
            //DB logger added..
            if (ZipDirectoryContainsFiles())
            {
                TccLog.UpdateLog("Send Mail Process Started", LogMessageType.Info, "Export");
                SendMail();
                if (isMailSendSuccessful)
                {
                    ArchiveZipFiles();
                    //redo the whole process again
                    isMailSendSuccessful = false;
                    ExportData();
                    CreateZip();
                    SendMail();
                    if (isMailSendSuccessful)
                    {
                        ArchiveZipFiles();
                    }
                }
            }
            else
            {
                isMailSendSuccessful = false;
                ExportData();
                CreateZip();
                //SendMail();
                if (isMailSendSuccessful)
                {
                    ArchiveZipFiles();
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

                TccLog.UpdateLog("ArchiveZipFiles Process Complete", LogMessageType.Info, "Export");
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export");
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
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-ZipDirectoryContainsZipFiles");
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
                //string[] xmlPaths = Directory.GetFiles(path + "\\");

                //foreach (string xmlfilePath in xmlPaths)
                //{
                //    //xml file copy to temp folder and then zip that file
                //    string xmlFile = Path.GetFileName(xmlfilePath);
                //    string tmpxPath = Path.Combine(Path.GetDirectoryName(xmlfilePath), "temp");
                //    File.Copy(xmlfilePath, Path.Combine(tmpxPath, xmlFile));

                //    string fileName = Path.GetFileNameWithoutExtension(xmlfilePath);
                //    fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                //    fileName = fileName + ".zip";
                //    using (ZipFile zip = new ZipFile())
                //    {
                //        zip.AddDirectory(tmpxPath + "\\");
                //        zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                //        zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                //        zip.Save(zippath + "\\" + fileName);

                //        //Delete file from temp foldes
                //        File.Delete(Path.Combine(tmpxPath, xmlFile));
                //    }

                //    File.Delete(xmlfilePath);

                //}

                //------------------------------------------------------------------------------------------------------------
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());
                con.Open();
                SqlCommand cmd = new SqlCommand("GetShipDetailsById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipId", ShipId);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //string fileName = ds.Tables[0].Rows[0]["IMONumber"].ToString();
                string fileName = ds.Tables[0].Rows[0]["ID"].ToString();
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
                string[] filePaths = Directory.GetFiles(path + "\\");

                foreach (string filePath in filePaths)

                    File.Delete(filePath);
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-CreateZip");
                logger.Error("Error in CreateZip. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in CreateZip.");
                //Environment.Exit(0);
            }
        }

        public static void CreateUploadedFileZip(string partName, string xmlFile)
        {
            try
            {

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
                    if(partName == "TICKET")
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
                    relPath = relPath.Replace("~", "").Replace("/","");

                    //sourcePath = Path.Combine(sourcePath, relPath);
                    sourcePath = sourcePath + relPath;
                    sourcePath = Path.Combine(sourcePath, uploadedFileName);
                    if(File.Exists(sourcePath))
                        File.Copy(sourcePath, Path.Combine(tmpPath, uploadedFileName));



                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());
                con.Open();
                SqlCommand cmd = new SqlCommand("GetShipDetailsById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipId", ShipId);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //string fileName = ds.Tables[0].Rows[0]["IMONumber"].ToString();
                string zipName = ds.Tables[0].Rows[0]["ID"].ToString();
                //zipName = zipName + "_TICKET_" + DateTime.Now.ToString("MMddyyyyhhmm");
                zipName = zipName + "_"+ partName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                zipName = zipName + ".zip";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(tmpPath + "\\");
                    zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                    zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                    zip.Save(zippath + "\\" + zipName);
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

        #region Export Data From DB
        public static void ExportData()
        {
            logger.Info("Import Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Export Process Started", LogMessageType.Info, "Export");
            try
            {
                FillupFormsUploaded();
                TccLog.UpdateLog("FillUpFormsUploaded Complete", LogMessageType.Info, "Export");
                FillupFormApproverMapper();
                TccLog.UpdateLog("FillUpFormApproverMapper Process Complete", LogMessageType.Info, "Export");
                Ticket();
                TccLog.UpdateLog("Ticket Process Complete", LogMessageType.Info, "Export");
                RevisionViewer();
                TccLog.UpdateLog("Revision Viewer Process Complete", LogMessageType.Info, "Export");

            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-ExportData");
                logger.Error("Error in ExportData. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ExportData.");
                //Environment.Exit(0);
            }
        }

        public static void Ticket()
        {
            string uploadedFileName = string.Empty;
            string relativePath = string.Empty;
            string filePath = string.Empty;

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                //SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
                SqlCommand cmd = new SqlCommand("stpExportTicketFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString(), XmlWriteMode.WriteSchema);
                }
                con.Close();


                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();
                CreateUploadedFileZip("TICKET", xmlFile);
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-Ticket");
            }

        }

        public static void FillupFormsUploaded()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
                SqlCommand cmd = new SqlCommand("stpExportFillupUoloadedFormsFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString(), XmlWriteMode.WriteSchema);
                }
                con.Close();
                string xmlFile = path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();
                CreateUploadedFileZip("FILLUPUPLOADEDFILE", xmlFile);
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-FillupFoormsUploaded");
            }
        }

        public static void FillupFormApproverMapper()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                //SqlCommand cmd = new SqlCommand("stpExporttblFormsUploadedApproverMapping", con);
                SqlCommand cmd = new SqlCommand("stpExportFillupFormApproverFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString(), XmlWriteMode.WriteSchema);
                }
                con.Close();
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-FillupFormApproverMapper");
            }
        }

        public static void RevisionViewer()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                //SqlCommand cmd = new SqlCommand("stpExporttblRevisionViewerFromShip", con);
                SqlCommand cmd = new SqlCommand("stpExportRevisionViewerFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlRevisionViewer"].ToString(), XmlWriteMode.WriteSchema);
                }
                con.Close();
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-RevisionViewer");
            }
        }


        #endregion

        #region Sending Mail


        public static void SendMail()
        {

            
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.Subject = GetConfigData("tccSsubject");
                    mail.From = new MailAddress(GetConfigData("mailfrom"));
                    mail.To.Add(GetConfigData("mailto"));

                    //mail.From = new MailAddress(GetConfigData("shipemail"));
                    //mail.To.Add(GetConfigData("admincenteremail"));

                    if (ZipDirectoryContainsZipFiles())
                    {
                        mail.Attachments.Add(new Attachment(zippath + "\\" + GetZipFileName()));
                    }

                    SmtpClient smtp = new SmtpClient(GetConfigData("smtp"));
                    smtp.EnableSsl = true;
                    smtp.Port = int.Parse(GetConfigData("port"));
                    smtp.Credentials = new System.Net.NetworkCredential(GetConfigData("mailfrom").Trim(), GetConfigData("frompwd").Trim());

                    smtp.Send(mail);
                    TccLog.UpdateLog("Send Mail Successfull", LogMessageType.Info, "Export");
                    isMailSendSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
                isMailSendSuccessful = false;
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Export-SendMail");
                logger.Error("Mail send failed - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully.");
                Environment.Exit(0);
            }

        }


        #endregion
    }
}
