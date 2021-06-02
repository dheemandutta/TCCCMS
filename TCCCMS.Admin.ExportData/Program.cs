using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Ionic.Zip;
using System.Net.Mail;

using System.Globalization;
using System.Diagnostics;
using Quartz;

namespace TCCCMS.Admin.ExportData
{
    public class Program : IJob
    {
        static String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "xml");
        static String zippath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "ZipFile");
        static String ziparchivePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "Archive");
        public static bool isMailSendSuccessful = false;
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            logger.Info("Process Started. - {0}", DateTime.Now.ToString());

            ExportData();
            CreateZip();

            if (ZipDirectoryContainsFiles())
            {
                //SendMail();
                //if (isMailSendSuccessful)
                //{
                ArchiveZipFiles();
                //redo the whole process again
                isMailSendSuccessful = false;
                ExportData();
                CreateZip();
                //SendMail();
                //if (isMailSendSuccessful)
                //{
                //    ArchiveZipFiles();
                //}
                //}
            }
            else
            {
                isMailSendSuccessful = false;
                ExportData();
                CreateZip();
                //SendMail();
                //if (isMailSendSuccessful)
                //{
                //    ArchiveZipFiles();
                //}
            }

        }


        public static void SendMail()
        {

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.Subject = GetConfigData("subject");
                    mail.From = new MailAddress(GetConfigData("mailfrom"));

                    mail.To.Add(GetConfigData("mailto"));

                    if (ZipDirectoryContainsZipFiles())
                    {
                        mail.Attachments.Add(new Attachment(zippath + "\\" + GetZipFileName()));
                    }

                    SmtpClient smtp = new SmtpClient(GetConfigData("smtp"));
                    smtp.EnableSsl = true;
                    smtp.Port = int.Parse(GetConfigData("port"));
                    smtp.Credentials = new System.Net.NetworkCredential(GetConfigData("mailfrom").Trim(), GetConfigData("frompwd").Trim());

                    smtp.Send(mail);

                    isMailSendSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
                isMailSendSuccessful = false;
                logger.Error("Mail send failed - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully.");
                Environment.Exit(0);
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
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["RestHourDBConnectionString"].ConnectionString);
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
            }
            catch (Exception ex)
            {

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
                string[] xmlPaths = Directory.GetFiles(path + "\\");

                foreach (string xmlfilePath in xmlPaths)
                {
                    //xml file copy to temp folder and then zip that file
                    string xmlFile = Path.GetFileName(xmlfilePath);
                    string tmpxPath = Path.Combine(Path.GetDirectoryName(xmlfilePath), "temp");
                    File.Copy(xmlfilePath, Path.Combine(tmpxPath, xmlFile));

                    string fileName = Path.GetFileNameWithoutExtension(xmlfilePath);
                    fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyyhhmm");
                    fileName = fileName + ".zip";
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(tmpxPath + "\\");
                        zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                        zip.MaxOutputSegmentSize = int.Parse(ConfigurationManager.AppSettings["OutputSize"].ToString());
                        zip.Save(zippath + "\\" + fileName);

                        //Delete file from temp foldes
                        File.Delete(Path.Combine(tmpxPath, xmlFile));
                    }

                    File.Delete(xmlfilePath);

                }
            }
            catch (Exception ex)
            {


                logger.Error("Error in CreateZip. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in CreateZip.");
                //Environment.Exit(0);
            }
        }

        public static void ExportData()
        {
            try
            {
                FormsUploadedApproverMapping();
                FormsUploaded();
                RevisionHeader();
                RevisionHistory();
                Ticket();

            }
            catch (Exception ex)
            {

                logger.Error("Error in ExportData. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ExportData.");
                //Environment.Exit(0);
            }
        }



        public static void FormsUploadedApproverMapping()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            SqlCommand cmd = new SqlCommand("stpExportFillupFormApproverFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFormsUploadedApproverMapping"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void FormsUploaded()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            SqlCommand cmd = new SqlCommand("stpExportFillupUoloadedFormsFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFormsUploaded"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void RevisionHeader()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            SqlCommand cmd = new SqlCommand("stpExportRevisionHeaderFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHeader"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void RevisionHistory()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            SqlCommand cmd = new SqlCommand("stpExportRevisionHistoryFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlRevisionHistory"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

        public static void Ticket()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormUploaded", con);
            SqlCommand cmd = new SqlCommand("stpExportTicketFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();
        }

    }
}
