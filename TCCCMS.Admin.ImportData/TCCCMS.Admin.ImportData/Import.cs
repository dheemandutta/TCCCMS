using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Quartz;
using Ionic.Zip;
using TCCCMS.LOG;

namespace TCCCMS.Admin.ImportData
{
    public class Import : IJob
    {
        static String extractPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "xml");
        static String zipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "ZipFile");
        static String zipArchivePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "Archive");
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static bool isMailReadSuccessful = false;

        public async Task Execute(IJobExecutionContext context)
        {
            //ImportMail();
            //if (isMailReadSuccessful)
            //{
                if (ZipDirectoryContainsFiles())
                 {

                    StartImport();
                }

            //}

            //throw new NotImplementedException();
        }


        #region Import Data To Database

        static void UnzipDoloadedFile()
        {


        }
        static void StartImport()
        {
            logger.Info("Import Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Import Process Started", LogMessageType.Info, "Admin Import");
            String TargetDirectory = zipPath + "\\";
            string[] filePaths = null;

            string tmpPath = Path.Combine(Path.GetDirectoryName(zipPath), "temp");

            try
            {
                filePaths = Directory.GetFiles(TargetDirectory);
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import");
                logger.Error("Directory not found. - {0}", DateTime.Now.ToString(), ex.Message);
                logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }

            foreach (string filePath in filePaths)
            {
                //read file name
                string fileName = Path.GetFileName(filePath);

                ////extract IMO number 
                //string[] vesselIMONumber = fileName.Split('_');
                //////check for valid IMO
                ////bool isValidIMO = CheckValidIMO(int.Parse(vesselIMONumber[0].ToString())) == 0 ? false : true;

                //logger.Info("IMO- {0}", vesselIMONumber[0].ToString());

                //------------------------------------------
                string[] fileNameParts = fileName.Split('_');
                string fileCategory = fileNameParts[1].ToString();
                //------------------------------------------


                //unzip the file
                using (ZipFile zip1 = ZipFile.Read(filePath))
                {
                    //Unzip a file
                    try
                    {
                        if(fileCategory == "TICKET" || fileCategory == "FILLUPUPLOADEDFILE")
                        {
                            zip1.ExtractAll(tmpPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                            TccLog.UpdateLog("Unzip Complete for Ticket And FILLUPUPLOADEDFILE", LogMessageType.Info, "Admin Import");
                        }
                        else
                        {
                            zip1.ExtractAll(extractPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                            TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Admin Import");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-StartImport");
                        logger.Error("Could not unzip file {0}", extractPath);
                        logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                        //Environment.Exit(0);
                    }

                }

                logger.Info("UnZip Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Admin Import");

                // start DB sync process
                ImportData();
                logger.Info("Data Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Data Import Complete", LogMessageType.Info, "Admin Import");
                //Update last stnc date for IMO
                //UpdateLastSyncDate(int.Parse(vesselIMONumber[0].ToString()));
                //logger.Info("Update Sync Date Complete. - {0}", DateTime.Now.ToString());

                //Delete extracted files
                string[] extractedFiles = Directory.GetFiles(extractPath + "\\");
                foreach (string files in extractedFiles)
                {
                    File.Delete(files);
                }
                logger.Info("Files Deleted . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("File Deletion Complete", LogMessageType.Info, "Admin Import");
                //Delete temp files
                string[] tempFiles = Directory.GetFiles(tmpPath + "\\");
                foreach (string files in tempFiles)
                {
                    File.Delete(files);
                }
                logger.Info("Temp Files Deleted . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Temporary File Deletion Complete", LogMessageType.Info, "Admin Import");
                //Archive zip file
                ArchiveZipFiles(fileName);
                logger.Info("Archive Complete . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Archive Complete", LogMessageType.Info, "Admin Import");

            }
        }

        public static void ArchiveZipFiles(string f)
        {
            string sourceFilePath = zipPath + "\\";
            string destinationFilePath = zipArchivePath + "\\";

            string[] sourceFiles = Directory.GetFiles(sourceFilePath);

            foreach (string sourceFile in sourceFiles)
            {
                try
                {
                    string fName = Path.GetFileName(sourceFile);
                    if (fName == f)
                    {
                        string destFile = Path.Combine(destinationFilePath, fName);

                        File.Move(sourceFile, destFile);
                    }
                }
                catch (Exception ex)
                {
                    TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-ArchiveFiles");
                    logger.Error(ex.Message);
                    logger.Info("Import process terminated unsuccessfully in ArchiveZipFiles. - {0}", DateTime.Now.ToString());
                    //Environment.Exit(0);
                }

            }
            isMailReadSuccessful = false;
            //System.IO.File.Move(sourceFilePath, destinationFilePath);
        }

        public static void CopyUploadedFiles(string partName, string xmlFile)
        {
            
            string sourceFilePath = Path.Combine(Path.GetDirectoryName(extractPath), "temp");

            DataSet dataSet = new DataSet();
            dataSet.ReadXmlSchema(xmlFile);
            dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);
            string[] sourceFiles = Directory.GetFiles(sourceFilePath);


            //----------------------------------------------------------------
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string destinationFilePath = @"C\\inetpub\\wwwroot\\TCCCMS";
                string tempSourcePath = Path.Combine(Path.GetDirectoryName(extractPath), "temp");
                string uploadedFileName = string.Empty;
                string relPath = string.Empty;
                string filePath = string.Empty;
                if (partName == "TICKET")
                {
                    filePath = row["FilePath"].ToString();
                    uploadedFileName = Path.GetFileName(filePath);
                   // destinationFilePath = destinationFilePath + "\\TicketFiles";
                }
                else
                {
                    filePath = row["FormsPath"].ToString();
                    uploadedFileName = row["FormsName"].ToString();
                    //destinationFilePath = destinationFilePath + "\\UploadFilledUpFormForApproval";
                }

                relPath = Path.GetDirectoryName(filePath);
                relPath = relPath.Replace("~", "").Replace("/", "");

                destinationFilePath = destinationFilePath + relPath;
                tempSourcePath = Path.Combine(tempSourcePath, uploadedFileName);
                if (File.Exists(tempSourcePath))
                    File.Copy(tempSourcePath, Path.Combine(destinationFilePath, uploadedFileName));



            }

            isMailReadSuccessful = false;
            //System.IO.File.Move(sourceFilePath, destinationFilePath);
        }

        public static void CopyUploadedFiles2(string f, string relativePath)
        {
            //string sourceFilePath = zipPath + "\\";
            string sourceFilePath = extractPath + "\\";
            string destinationFilePath = @"C\\inetpub\\wwwroot\\TCCCMS" + "\\";

            destinationFilePath = destinationFilePath + relativePath.Replace("~/", "").Replace("/", "\\");

            string[] sourceFiles = Directory.GetFiles(sourceFilePath);

            foreach (string sourceFile in sourceFiles)
            {
                try
                {
                    string fName = Path.GetFileName(sourceFile);
                    if (fName == f)
                    {
                        string destFile = Path.Combine(destinationFilePath, fName);

                        //File.Move(sourceFile, destFile);
                        File.Copy(sourceFile, destFile);
                    }
                }
                catch (Exception ex)
                {

                    logger.Error(ex.Message);
                    logger.Info("Import process terminated unsuccessfully in CopyUploadedFiles. - {0}", DateTime.Now.ToString());
                    //Environment.Exit(0);
                }

            }
            isMailReadSuccessful = false;
            //System.IO.File.Move(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Returns true if Zip files directory contains zip files
        /// </summary>
        /// <returns></returns>
        public static bool ZipDirectoryContainsFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(zipPath + "\\");
                return di.GetFiles("*.zip").Length > 0;
            }
            catch (Exception ex)
            {

                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Import process terminated unsuccessfully in ZipDirectoryContainsZipFiles.");
                return false;
                //Environment.Exit(0);
            }
        }
        /// <summary>
        public static void ImportData()
        {
            try
            {

                
                Ticket();
                logger.Info("Ticket Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Ticket Import Complete", LogMessageType.Info, "Admin Import");
                RevisionViewer();
                logger.Info("Revision Viewers Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Revision Viewers Import Complete", LogMessageType.Info, "Admin Import");
                FillupFormsUploaded();
                logger.Info("Fillup Forms  Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Forms  Import Complete", LogMessageType.Info, "Admin Import");
                FillupFormApproverMapper();
                logger.Info("Fillup Form Approver Mapper Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Form Approver Mapper Import Complete", LogMessageType.Info, "Admin Import");

            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-ImportData");
                logger.Error(ex.Message);
                logger.Info("Import process terminated unsuccessfully. - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }

            ////Groups();
            ////GroupRank();
            ////Users();
            ////UserGroups();
            ////FirstRun();
            ////Regimes();

        }


        public static int CheckValidIMO(int IMONumber)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["RestHourDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("stpGetShipByIMONumber", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IMONumber", IMONumber);

            int recordsAffected = (int)cmd.ExecuteScalar();
            con.Close();

            return recordsAffected;
        }

        public static void UpdateLastSyncDate(int IMONumber)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["RestHourDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpUpdateShipByLastSyncDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IMONumber", IMONumber);

                int recordsAffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-UpdateLastSyncDate");
                logger.Error(ex.Message);
                logger.Info("Import process terminated unsuccessfully while updating last sysn date. - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }


        }



        public static void Ticket()
        {
            try
            {
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpImportTicketInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    //cmd.Parameters.AddWithValue("@ID", int.Parse(row["ID"].ToString()));

                    string uploadedFileName = string.Empty;
                    string relativePath = string.Empty;
                    string filePath = string.Empty;

                    cmd.Parameters.AddWithValue("@TicketNumber", row["TicketNumber"].ToString());
                    cmd.Parameters.AddWithValue("@Error", row["Error"].ToString());
                    if (row["Description"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@Description", row["Description"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@FilePath", row["FilePath"].ToString());
                    filePath = row["FilePath"].ToString();

                    if (row["Email"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@Email", row["Email"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    }

                    if (row["IsSolved"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@IsSolved", Boolean.Parse(row["IsSolved"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsSolved", DBNull.Value);
                    }

                    if (row["CreatedAt"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Parse(row["CreatedAt"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedAt", DBNull.Value);
                    }

                    if (row["CreatedBy"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", int.Parse(row["CreatedBy"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@ShipId", int.Parse(row["ShipId"].ToString()));


                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //if (!String.IsNullOrEmpty(filePath))
                    //{
                    //    uploadedFileName = Path.GetFileName(filePath);
                    //    relativePath = Path.GetDirectoryName(filePath);
                    //    relativePath = relativePath.Replace("\\", "/") + "/";
                    //    CopyUploadedFiles(uploadedFileName, relativePath);
                    //}
                    CopyUploadedFiles("TICKET", xmlFile);
                    // CopyUploadedFiles(row["FilePath"].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ticket Import");
                //throw;
            }
        }

        public static void RevisionViewer()
        {
            try
            {
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlRevisionViewer"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpImportRevisionViewerInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@RevisionId", int.Parse(row["RevisionId"].ToString()));

                    cmd.Parameters.AddWithValue("@UserId", int.Parse(row["UserId"].ToString()));

                    cmd.Parameters.AddWithValue("@ShipId", int.Parse(row["ShipId"].ToString()));
                    if (row["CreatedAt"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Parse(row["CreatedAt"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedAt", DBNull.Value);
                    }



                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Crew Import");
                //throw;
            }
        }

        public static void FillupFormsUploaded()
        {
            try
            {
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpImportFillupUoloadedFormsInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    string uploadedFileName = string.Empty;
                    string relativePath = string.Empty;

                    cmd.Parameters.AddWithValue("@FormId", int.Parse(row["FormId"].ToString()));

                    cmd.Parameters.AddWithValue("@ShipId", int.Parse(row["ShipId"].ToString()));
                    cmd.Parameters.AddWithValue("@FormsPath", row["FormsPath"].ToString());
                    cmd.Parameters.AddWithValue("@FormsName", row["FormsName"].ToString());
                    uploadedFileName = row["FormsName"].ToString();
                    relativePath = row["FormsPath"].ToString();
                    if (row["CreatedOn"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Parse(row["CreatedOn"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@CreatedBy", int.Parse(row["CreatedBy"].ToString()));


                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //if(!String.IsNullOrEmpty(uploadedFileName) && !String.IsNullOrEmpty(relativePath))
                    //{
                    //    CopyUploadedFiles(uploadedFileName, relativePath);
                    //}

                    CopyUploadedFiles("FILLUPUPLOADEDFILE", xmlFile);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "FillupFormsUploaded Import");
                //throw;
            }
        }

        public static void FillupFormApproverMapper()
        {
            try
            {
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString();

                DataSet dataSet = new DataSet();
                dataSet.ReadXmlSchema(xmlFile);
                dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpImportFillupFormApproverInAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    cmd.Parameters.AddWithValue("@UploadedFormName", row["UploadedFormName"].ToString());
                    cmd.Parameters.AddWithValue("@ApproverUserId", int.Parse(row["ApproverUserId"].ToString()));
                    if (row["CreatedOn"] != DBNull.Value)
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Parse(row["CreatedOn"].ToString()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CreatedOn", DBNull.Value);
                    }



                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "FillupFormApproverMapper Import");
                //throw;
            }
        }

        #endregion

        #region Import Zip from Mail

        static void ImportMail()
        {
            try
            {
                logger.Info("Import Zip from Mailbox Started. - {0}", DateTime.Now.ToString());
                string mTyp = GetConfigData("protocol");
                //Creating Mail configuration 
                MailServiceConfiguration serviceconf = new MailServiceConfiguration
                {
                    MailId = GetConfigData("admincenteremail"),
                    MailPassword = GetConfigData("admincenteremailpwd"),

                    //SubjectLine         = "DATASYNCFILE",
                    SubjectLine = GetConfigData("tccSsubject"),

                    MailServerDomain = GetConfigData("imappopserver"),
                    Port = int.Parse(GetConfigData("imappopport")),
                    MailServerType = mTyp,

                    AttachmentPath = zipPath,

                    ///---------------------------
                    //MailId              = "cableman24x7@gmail.com",
                    //MailPassword        = "cableman24x712345",
                    //MailServerDomain    = "imap.gmail.com",
                    //Port                = 993,      
                    //AttachmentPath      = zipPath,
                    ////SubjectLine = "RHDATASYNC",
                    //SubjectLine         = "DATASYNCFILE",
                    //MailServerType      = MailType.IMAP
                    //---------------------------------
                };

                MailOps mailops = new MailOps
                {
                    MailServerType = serviceconf.MailServerType
                };
                //mailops.Connect(serviceconf.MailId, Security.DecryptString(serviceconf.MailPassword), serviceconf.MailServerDomain, serviceconf.Port);
                mailops.Connect(serviceconf.MailId, serviceconf.MailPassword, serviceconf.MailServerDomain, serviceconf.Port);
                mailops.DownloadAllNewMails(serviceconf.SubjectLine, serviceconf.AttachmentPath);

                isMailReadSuccessful = true;
                logger.Info("Import Zip from Mailbox process Successfully Completed. - {0}", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("Import Zip from Mailbox. - {0}", DateTime.Now.ToString(), ex.Message + " :" + ex.InnerException);
                logger.Info("Import Zip from Mailbox process terminated unsuccessfully in Importmail. - {0}", DateTime.Now.ToString());
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

        }

        private static string GetShipEmail()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetShipEmailWithOutIMO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds.Tables[0].Rows[0]["ShipEmail"].ToString();
        }

        private static string GetShipEmail(int vesselId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("GetShipEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VesselId", vesselId);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds.Tables[0].Rows[0]["ShipEmail"].ToString();
        }

        #endregion
    }
}
