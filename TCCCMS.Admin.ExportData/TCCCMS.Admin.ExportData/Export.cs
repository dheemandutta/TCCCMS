﻿using System;
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

namespace TCCCMS.Admin.ExportData
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
            if (ZipDirectoryContainsFiles())
            {
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
                SendMail();
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

        private static string GetShipEmail(int vesselId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetShipEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ShipId", vesselId);

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

        #region Export Data From DB
        public static void ExportData()
        {
            try
            {
                
                Ticket();
                logger.Info("Ticket Export Complete. - {0}", DateTime.Now.ToString());
                RevisionHeader();
                logger.Info("Revision Header Export Complete. - {0}", DateTime.Now.ToString());
                RevisionDetails();
                logger.Info("Revision History Export Complete. - {0}", DateTime.Now.ToString());
                FillupFormsUploaded();
                logger.Info("Fillup Forms  Export Complete. - {0}", DateTime.Now.ToString());
                FillupFormApproverMapper();
                logger.Info("Fillup Form Approver Mapper Export Complete. - {0}", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {

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


            //---------------------------------------------------------------------------------------------------------------------
            DataSet dsVessels = new DataSet();
            dsVessels = GetRegistrerdVessels();

            for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
                SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipId", int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString()));
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
        }

        public static void FillupFormsUploaded()
        {
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

            //---------------------------------------------------------------------------------------------------------------------
            DataSet dsVessels = new DataSet();
            dsVessels = GetRegistrerdVessels();

            for (int i = 0; i < dsVessels.Tables[0].Rows.Count; i++)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
                SqlCommand cmd = new SqlCommand("stpExporttblTicketFromShip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipId", int.Parse(dsVessels.Tables[0].Rows[i]["ShipId"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ds.WriteXml(path + "\\" + dsVessels.Tables[0].Rows[i]["ShipId"].ToString() + ".xml", XmlWriteMode.WriteSchema);
                    ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString(), XmlWriteMode.WriteSchema);
                }
                con.Close();
            }
        }

        public static void FillupFormApproverMapper()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblFormsUploadedApproverMapping", con);
            SqlCommand cmd = new SqlCommand("stpExportFillupFormApproverFromAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXml(path + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString(), XmlWriteMode.WriteSchema);
            }
            con.Close();

            //------------------------------------------Alternate Code---------------------------------------------------------------------------
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
            SqlCommand cmd = new SqlCommand("stpExportRevisionViewerFromShip", con);
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

        public static void RevisionDetails()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            //SqlCommand cmd = new SqlCommand("stpExporttblRevisionViewerFromShip", con);
            SqlCommand cmd = new SqlCommand("stpExportRevisionViewerFromAdmin", con);
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

        #endregion

        #region Sending Mail


        public static void SendMail()
        {
            logger.Info("Sending Mail Process Start. - {0}", DateTime.Now.ToString());

            //string sourceFilePath = path + "\\"; // old code line
            string sourceFilePath = zippath + "\\";
            string[] sourceFiles = Directory.GetFiles(sourceFilePath);

            string adminEmail = GetConfigData("admincenteremail").Trim();
            string adminEmailpwd = GetConfigData("admincenteremailpwd").Trim();

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
                        smtp.Credentials = new System.Net.NetworkCredential(adminEmail, adminEmailpwd);
                        //smtp.Credentials = new System.Net.NetworkCredential("cableman24x7@gmail.com", "cableman24x712345");

                        smtp.Send(mail);
                        logger.Info("Mail send Successfully to the Vessel_" + vesselIMO[0].ToString() + ". - {0}", DateTime.Now.ToString());
                        isMailSendSuccessful = true;
                    }

                }
                catch (Exception ex)
                {
                    //EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
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
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            // Prasenjit // "r" is a Typo "stpExportCrewApprovalData" to "stpExpotrCrewApprovalData"
            SqlCommand cmd = new SqlCommand("stpGETAllRegisteredVessels", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }
    }

}
