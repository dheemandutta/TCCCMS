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
using TCCCMS.CryptoUtility;
using System.Globalization;

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
            ImportMail();
            if (isMailReadSuccessful)
            {
                if (ZipDirectoryContainsFiles())
                {

                    StartImport();
                }

            }

            ////throw new NotImplementedException();
            ////for test
            //ImportData();
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

                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import - StartImport");
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
                        //if(fileCategory == "TICKET" || fileCategory == "FILLUPUPLOADEDFILE")
                        //{
                        //    zip1.ExtractAll(tmpPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                        //    TccLog.UpdateLog("Unzip Complete for Ticket And FILLUPUPLOADEDFILE", LogMessageType.Info, "Admin Import");
                        //}
                        //else
                        //{
                        //    zip1.ExtractAll(extractPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                        //    TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Admin Import");
                        //}

                        zip1.ExtractAll(extractPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                        TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Admin Import");

                    }
                    catch (Exception ex)
                    {
                        TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import-StartImport");
                        logger.Error("Could not unzip file {0}", extractPath);
                        logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                        //Environment.Exit(0);
                    }

                }

                logger.Info("UnZip Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Admin Import");

                UnzipUploadedFiles();
                TccLog.UpdateLog("Uploaded File Unzip Complete", LogMessageType.Info, "Admin Import");

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

        public static void UnzipUploadedFiles()
        {
            logger.Info("Uploaded File Uizip Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Uploaded File Uizip Started", LogMessageType.Info, "Admin Import");
            String TargetDirectory = extractPath + "\\";
            string tmpPath = Path.Combine(Path.GetDirectoryName(extractPath), "temp");
            string[] filePaths = null;

            try
            {
                filePaths = Directory.GetFiles(TargetDirectory,"*.zip");
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import - StartImport");
                logger.Error("Directory not found. - {0}", DateTime.Now.ToString(), ex.Message);
                logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);

                //------------------------------------------
                string[] fileNameParts = fileName.Split('_');
                string fileCategory = fileNameParts[1].ToString();
                //------------------------------------------


                //unzip the file
                using (ZipFile zip1 = ZipFile.Read(filePath))
                {
                    try
                    {
                        if (fileCategory == "TICKET" || fileCategory == "FILLUPUPLOADEDFILE")
                        {
                            zip1.ExtractAll(tmpPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                            TccLog.UpdateLog("Uploaded File Unzip Complete for Ticket And FILLUPUPLOADEDFILE", LogMessageType.Info, "Admin Import");
                        }
                        

                    }
                    catch (Exception ex)
                    {
                        TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import-StartImport");
                        logger.Error("Could not unzip file {0}", extractPath);
                        logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                        //Environment.Exit(0);
                    }
                }
            }

        }

        public static void ArchiveZipFiles(string f)
        {
            TccLog.UpdateLog("Start Archiving", LogMessageType.Info, "Admin Import -ExecuteSql");
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
                    TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import-ArchiveFiles");
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
            logger.Info("Copy uploaded file Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Copy uploaded file Started", LogMessageType.Info, "Admin Import");

            string sourceFilePath = Path.Combine(Path.GetDirectoryName(extractPath), "temp");

           

            try
            {
                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);
                    string[] sourceFiles = Directory.GetFiles(sourceFilePath);
                    //----------------------------------------------------------------
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        //string destinationFilePath = @"C:\\inetpub\\wwwroot\\TCCCMS";
                        string destinationFilePath = ConfigurationManager.AppSettings["iisPath"].ToString();
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

                        logger.Info(destinationFilePath + " - {0}", DateTime.Now.ToString());
                        TccLog.UpdateLog(destinationFilePath, LogMessageType.Info, "Admin Import- CopyUploadedFiles");
                        logger.Info(tempSourcePath + ". - {0}", DateTime.Now.ToString());
                        TccLog.UpdateLog(tempSourcePath, LogMessageType.Info, "Admin Import- CopyUploadedFiles");

                        if (File.Exists(tempSourcePath))
                        {
                            File.Copy(tempSourcePath, Path.Combine(destinationFilePath, uploadedFileName), true);
                            TccLog.UpdateLog("File copied from Temp to IIS", LogMessageType.Error, "Asmin Export-CopyUploadedFiles- foreach");
                        }
                        else
                        {
                            TccLog.UpdateLog("File not copied from Temp to IIS", LogMessageType.Error, "Admin Export-CopyUploadedFiles- foreach");
                        }

                    }

                    isMailReadSuccessful = false;
                    //System.IO.File.Move(sourceFilePath, destinationFilePath);
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-CreateUploadedZipFile");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-CreateUploadedZipFile");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-CreateUploadedZipFile");
                logger.Error(ex, "Ticket Import");
                //throw;
            }


        }

        public static void CopyUploadedFiles2(string f, string relativePath)
        {
            logger.Info("Copy uploaded file Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Copy uploaded file Started", LogMessageType.Info, "Admin Import");
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

                logger.Error("Directory not found. - {0}", ex.Message + " :" + ex.Message);
                logger.Info("Import process terminated unsuccessfully in ZipDirectoryContainsZipFiles.");
                TccLog.UpdateLog("Directory not found. - {0} ;"+ ex.Message + " :" + ex.Message, LogMessageType.Error, "Admin Import");
                TccLog.UpdateLog("Import process terminated unsuccessfully in ZipDirectoryContainsZipFiles.", LogMessageType.Info, "Admin Import");
                return false;
                //Environment.Exit(0);
            }
        }
        
        /// <summary>
        /// Modify 16th Jul 2021 @BK
        /// </summary>
        public static void ImportData()
        {
            try
            {

                ShipUser();//Added 16th Jul 2021 @BK
                logger.Info("ShipUser Import Complete. - {0}", DateTime.Now.ToString());
                Ticket();
                logger.Info("Ticket Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Ticket Import Complete", LogMessageType.Info, "Admin Import-ImportData");
                RevisionViewer();
                logger.Info("Revision Viewers Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Revision Viewers Import Complete", LogMessageType.Info, "Admin Import-ImportData");
                FillupFormsUploaded();
                logger.Info("Fillup Forms  Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Forms  Import Complete", LogMessageType.Info, "Admin Import-ImportData");
                FillupFormApproverMapper();
                logger.Info("Fillup Form Approver Mapper Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Form Approver Mapper Import Complete", LogMessageType.Info, "Admin Import-ImportData");

            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import-ImportData");
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
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("stpUpdateShipByLastSyncDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IMONumber", IMONumber);

                int recordsAffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import-UpdateLastSyncDate");
                logger.Error(ex.Message);
                logger.Info("Import process terminated unsuccessfully while updating last sysn date. - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }


        }

        /// <summary>
        /// Added 16th Jul 2021 @BK
        /// </summary>
        public static void ShipUser()
        {
            try
            {
                TccLog.UpdateLog("Importing ShipUser", LogMessageType.Info, "Admin Import-ShipUser");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlShipUser"].ToString();

                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportShipUserInAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        cmd.Parameters.Clear();
                        string CrewName = row["UserName"].ToString();
                        //int Rank = int.Parse(row["RankId"].ToString());
                        string ShipNumber = row["ShipId"].ToString();
                        string Email = row["Email"].ToString();
                        CultureInfo culture = new CultureInfo("en-US");
                        
                       // cdt = DateTime.ParseExact(row["CreatedOn"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        

                        cmd.Parameters.AddWithValue("@UserId", row["UserId"].ToString());
                        cmd.Parameters.AddWithValue("@UserName", CrewName);
                        cmd.Parameters.AddWithValue("@Password", row["Password"].ToString());
                        if (!string.IsNullOrEmpty(row["CreatedOn"].ToString()))
                        {
                            DateTime cdt = DateTime.Parse(row["CreatedOn"].ToString());
                            string sdt = cdt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            cmd.Parameters.AddWithValue("@CreatedOn", sdt);
                        }
                        else
                            cmd.Parameters.AddWithValue("@CreatedOn", DBNull.Value);

                        cmd.Parameters.AddWithValue("@IsActive", row["IsActive"].ToString());
                        if (!string.IsNullOrEmpty(Email))
                            cmd.Parameters.AddWithValue("@Email", Email);
                        else
                            cmd.Parameters.AddWithValue("@Email", DBNull.Value);

                        if (!string.IsNullOrEmpty(row["CreatedBy"].ToString()))
                            cmd.Parameters.AddWithValue("@CreatedBy", row["CreatedBy"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                        if (!string.IsNullOrEmpty(row["ModifiedBy"].ToString()))
                            cmd.Parameters.AddWithValue("@ModifiedBy", row["ModifiedBy"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@ModifiedBy", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", row["Gender"].ToString());
                        if (!string.IsNullOrEmpty(row["VesselIMO"].ToString()))
                            cmd.Parameters.AddWithValue("@VesselIMO", row["VesselIMO"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@VesselIMO", DBNull.Value);

                        if (!string.IsNullOrEmpty(row["RankID"].ToString()))
                            cmd.Parameters.AddWithValue("@RankID", row["RankID"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@RankID", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@RankId", Rank);

                        cmd.Parameters.AddWithValue("@ShipId", ShipNumber);
                        cmd.Parameters.AddWithValue("@UserCode", row["UserCode"].ToString());
                        cmd.Parameters.AddWithValue("@UserType", row["UserType"].ToString());

                        if (!string.IsNullOrEmpty(row["UploadPermission"].ToString()))
                            cmd.Parameters.AddWithValue("@UploadPermission", row["UploadPermission"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@UploadPermission", DBNull.Value);

                        if (!string.IsNullOrEmpty(row["JoinDate"].ToString()))
                        {
                            DateTime jdt = DateTime.Parse(row["JoinDate"].ToString());
                            string sjdt = jdt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            cmd.Parameters.AddWithValue("@JoinDate", sjdt);
                        }
                        else
                            cmd.Parameters.AddWithValue("@JoinDate", DBNull.Value);
                        if (!string.IsNullOrEmpty(row["ReleaseDate"].ToString()))
                        {
                            DateTime rdt = DateTime.Parse(row["ReleaseDate"].ToString());
                            string srdt = rdt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            cmd.Parameters.AddWithValue("@ReleaseDate", srdt);
                        }
                        else
                            cmd.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);

                        if (!string.IsNullOrEmpty(row["IsAdmin"].ToString()))
                            cmd.Parameters.AddWithValue("@IsAdmin", row["IsAdmin"].ToString());
                        else
                            cmd.Parameters.AddWithValue("@IsAdmin", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@IsAdmin", row["IsAdmin"].ToString());
                        cmd.Parameters.AddWithValue("@IsApprover", row["IsApprover"].ToString());


                        cmd.ExecuteNonQuery();
                        //cmd.Parameters.Clear();
                        TccLog.UpdateLog(CrewName + " is imported into DB Successfully", LogMessageType.Info, "Admin Import-ShipUser- Forceach");
                        
                    }
                    

                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-ShipUser");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-ShipUser");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-ShipUser");
                logger.Error(ex, "ShipUser Import");
                //throw;
            }
        }

        /// <summary>
        /// Modify on 16th Jul 2021 
        /// </summary>

        public static void Ticket()
        {
            try
            {
                TccLog.UpdateLog("Importing Ticket", LogMessageType.Info, "Admin Import-Ticket");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();

                if (File.Exists(xmlFile))
                {
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
                        string ticketNumber = row["TicketNumber"].ToString();
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
                            cmd.Parameters.AddWithValue("@IsSolved", int.Parse(row["IsSolved"].ToString()));
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
                        cmd.Parameters.AddWithValue("@ShipUserId", int.Parse(row["ShipUserId"].ToString())); //added on 16th Jul 2021 @bk


                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        TccLog.UpdateLog(ticketNumber + " is imported into DB Successfully", LogMessageType.Info, "Admin Import-Ticket- Forceach");
                        //if (!String.IsNullOrEmpty(filePath))
                        //{
                        //    uploadedFileName = Path.GetFileName(filePath);
                        //    relativePath = Path.GetDirectoryName(filePath);
                        //    relativePath = relativePath.Replace("\\", "/") + "/";
                        //    CopyUploadedFiles(uploadedFileName, relativePath);
                        //}
                        //CopyUploadedFiles("TICKET", xmlFile);
                        // CopyUploadedFiles(row["FilePath"].ToString());
                    }
                    CopyUploadedFiles("TICKET", xmlFile);

                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-Ticket");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-Ticket");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-Ticket");
                logger.Error(ex, "Ticket Import");
                //throw;
            }
        }

        /// <summary>
        /// Modify on 16th Jul 2021 
        /// </summary>
        public static void RevisionViewer()
        {
            try
            {
                TccLog.UpdateLog("Importing RevisionViewer", LogMessageType.Info, "Admin Import-RevisionViewer");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlRevisionViewer"].ToString();

                if (File.Exists(xmlFile))
                {
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
                        cmd.Parameters.AddWithValue("@ShipUserId", int.Parse(row["ShipUserId"].ToString())); //added on 16th Jul 2021 @bk



                        int x = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        TccLog.UpdateLog("Viewer User_"+ int.Parse(row["UserId"].ToString())+"-Ship_"+ int.Parse(row["ShipId"].ToString()) + " is imported into DB Successfully", LogMessageType.Info, "Admin Import-RevisionViewer- Forceach");
                    }
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-RevisionViewer");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-RevisionViewer");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-RevisionViewer");
                logger.Error(ex, "Crew Import");
                //throw;
            }
        }

        /// <summary>
        /// Modify on 16th Jul 2021 
        /// </summary>
        public static void FillupFormsUploaded()
        {
            try
            {
                TccLog.UpdateLog("Importing RevisionViewer", LogMessageType.Info, "Admin Import-FillupFormsUploaded");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();

                if (File.Exists(xmlFile))
                {
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
                        string formName = row["FormsName"].ToString();
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
                        cmd.Parameters.AddWithValue("@ShipUserId", int.Parse(row["ShipUserId"].ToString())); //added on 16th Jul 2021 @bk


                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        TccLog.UpdateLog("Form name " + formName + " is imported into DB Successfully", LogMessageType.Info, "Admin Import-FillupFormsUploaded- Forceach");
                        //if(!String.IsNullOrEmpty(uploadedFileName) && !String.IsNullOrEmpty(relativePath))
                        //{
                        //    CopyUploadedFiles(uploadedFileName, relativePath);
                        //}

                        //CopyUploadedFiles("FILLUPUPLOADEDFILE", xmlFile);
                    }
                    CopyUploadedFiles("FILLUPUPLOADEDFILE", xmlFile);
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-FillupFormsUploaded");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-FillupFormsUploaded");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-FillupFormsUploaded");
                logger.Error(ex, "FillupFormsUploaded Import");
                //throw;
            }
        }

        public static void FillupFormApproverMapper()
        {
            try
            {
                TccLog.UpdateLog("Importing FillupFormApproverMapper", LogMessageType.Info, "Ship Import-FillupFormApproverMapper");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlApprovedFillupForm"].ToString();

                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportFillupFormApproverInAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        string formName = row["UploadedFormName"].ToString();
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

                        TccLog.UpdateLog("Form name " + formName + " is imported into DB Successfully", LogMessageType.Info, "Admin Import-FillupFormApproverMapper- Forceach");
                    }
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Admin Import-FillupFormApproverMapper");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Admin Import-FillupFormApproverMapper");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Admin Import-FillupFormApproverMapper");
                logger.Error(ex, "FillupFormApproverMapper Import");
                //throw;
            }
        }

        #endregion

        #region Import Zip from Mail

        static void ImportMail()
        {
            MailOps mailops = null;
            try
            {
                TccLog.UpdateLog("Import Zip from Mailbox Started.", LogMessageType.Info, "Admin Import-ImportMail");
                logger.Info("Import Zip from Mailbox Started. - {0}", DateTime.Now.ToString());
                string mTyp = GetConfigData("protocol");
                //Creating Mail configuration 
                MailServiceConfiguration serviceconf = new MailServiceConfiguration
                {
                    MailId = GetConfigData("admincenteremail"),
                    MailPassword = GetConfigData("admincenteremailpwd"),

                    //MailPassword = EncodeDecode.DecryptString(GetConfigData("admincenteremailpwd")),

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
                TccLog.UpdateLog("Seivice Cofig created.", LogMessageType.Info, "Admin Import- ImportMail");
                mailops = new MailOps
                {
                    MailServerType = serviceconf.MailServerType
                };

                TccLog.UpdateLog("Signing in the Email.", LogMessageType.Info, "Admin Import- ImportMail");
                //mailops.Connect(serviceconf.MailId, Security.DecryptString(serviceconf.MailPassword), serviceconf.MailServerDomain, serviceconf.Port);
                mailops.Connect(serviceconf.MailId, serviceconf.MailPassword, serviceconf.MailServerDomain, serviceconf.Port);
                TccLog.UpdateLog("Connect Sucessfull.", LogMessageType.Info, "Admin Import- ImportMail");
                mailops.DownloadAllNewMails(serviceconf.SubjectLine, serviceconf.AttachmentPath);
                TccLog.UpdateLog("Download Sucessfull", LogMessageType.Info, "Admin Import - ImportMail");

                isMailReadSuccessful = true;
                logger.Info("Import Zip from Mailbox process Successfully Completed. - {0}", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Admin Import - ImportMail");
                logger.Error("Import Zip from Mailbox. - {0}", DateTime.Now.ToString(), ex.Message + " :" + ex.Message);
                logger.Info("Import Zip from Mailbox process terminated unsuccessfully in Importmail. - {0}", DateTime.Now.ToString());
            }
            finally
            {
                mailops.Dispose();
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
