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
using System.Xml;

namespace TCCCMS.Ship.ImportData
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

            //throw new NotImplementedException();
        }


        #region Import Data To Database
        static void StartImport()
        {
            logger.Info("Import Process Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Import Process Started", LogMessageType.Info, "Ship Import");
            String TargetDirectory = zipPath + "\\";
            string[] filePaths = null;

            string tmpPath = Path.Combine(Path.GetDirectoryName(zipPath), "temp");

            try
            {
                filePaths = Directory.GetFiles(TargetDirectory);
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-StartImport");
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


                //unzip the file
                using (ZipFile zip1 = ZipFile.Read(filePath))
                {
                    //Unzip a file
                    try
                    {
                        zip1.ExtractAll(extractPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                        TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Ship  Import - StartImport");
                    }
                    catch (Exception ex)
                    {
                        TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-StartImport-ZipExtract");
                        logger.Error("Could not unzip file {0}", extractPath);
                        logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                        //Environment.Exit(0);
                    }

                }

                logger.Info("UnZip Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Unzip Complete", LogMessageType.Info, "Ship Import");
                //start Uploaded files UnZip
                UnzipUploadedFiles();
                TccLog.UpdateLog("Uploaded File Unzip Complete", LogMessageType.Info, "Ship Import");

                // start DB sync process
                ImportData();
                logger.Info("Data Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Data Import Complete", LogMessageType.Info, "Ship Import");
                //Update last stnc date for IMO
                //UpdateLastSyncDate(int.Parse(vesselIMONumber[0].ToString()));
                //logger.Info("Update Sync Date Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Read Command  Started", LogMessageType.Info, "Ship Import- StartImport");
                ReadCommand();

                //Delete extracted files
                string[] extractedFiles = Directory.GetFiles(extractPath + "\\");
                foreach (string files in extractedFiles)
                {
                    File.Delete(files);
                }
                logger.Info("Files Deleted . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Files Deleted", LogMessageType.Info, "Ship Import");
                //Delete temp files
                string[] tempFiles = Directory.GetFiles(tmpPath + "\\");
                foreach (string files in tempFiles)
                {
                    File.Delete(files);
                }
                logger.Info("Temp Files Deleted . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Temporary File Deletion Complete", LogMessageType.Info, "Ship Import");
                //Archive zip file
                ArchiveZipFiles(fileName);
                logger.Info("Archive Complete . - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Archive Complete", LogMessageType.Info, "Ship Import");


            }
        }

        public static void UnzipUploadedFiles()
        {
            logger.Info("Uploaded File Uizip Started. - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Uploaded File Uizip Started", LogMessageType.Info, "Ship Import");
            String TargetDirectory      = extractPath + "\\";
            string tmpPath              = Path.Combine(Path.GetDirectoryName(extractPath), "temp");
            string[] filePaths          = null;
            string revisionSuffixName   = ConfigurationManager.AppSettings["revisionfilesuffixname"].ToString();
            string temRevPath           = Path.Combine(Path.GetDirectoryName(extractPath), "TempRevision");
            try
            {
                filePaths = Directory.GetFiles(TargetDirectory, "*.zip");
            }
            catch (Exception ex)
            {

                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Ship Import - StartImport");
                logger.Error("Directory not found. - {0}", DateTime.Now.ToString(), ex.Message);
                logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);

                //------------------------------------------
                string[] fileNameParts  = fileName.Split('_');
                string fileCategory     = fileNameParts[1].ToString();
                string revised          = fileNameParts[0].ToString();
                
                //------------------------------------------


                //unzip the file
                using (ZipFile zip1 = ZipFile.Read(filePath))
                {
                    try
                    {
                        if (fileCategory == "TICKET" || fileCategory == "FILLUPUPLOADEDFILE")
                        {
                            zip1.ExtractAll(tmpPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                            TccLog.UpdateLog("Uploaded File Unzip Complete for Ticket And FILLUPUPLOADEDFILE", LogMessageType.Info, "Ship Import- UnzipUploadedFiles");
                        }
                        else if(revised == revisionSuffixName)
                        {
                            zip1.ExtractAll(temRevPath + "\\", ExtractExistingFileAction.DoNotOverwrite);
                            TccLog.UpdateLog("Revision Files Unzip Completed", LogMessageType.Info, "Ship Import- UnzipUploadedFiles");
                        }


                    }
                    catch (Exception ex)
                    {
                        TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Ship Import-StartImport");
                        logger.Error("Could not unzip file {0}", extractPath);
                        logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
                        //Environment.Exit(0);
                    }
                }
            }

        }

        public static void ReadCommand()
        {
            string temRevPath = Path.Combine(Path.GetDirectoryName(extractPath), "TempRevision");
            string destinationRootPath = ConfigurationManager.AppSettings["rootPath"].ToString();
            string commandFile = string.Empty;
            string xPath = "";
            XmlDocument xDoc = new XmlDocument();
            string shipId = ConfigurationManager.AppSettings["SHIPID"].ToString();


            try
            {
                commandFile = Directory.GetFiles(temRevPath, "command.xml").FirstOrDefault();

            }
            catch(Exception ex)
            {
                TccLog.UpdateLog(ex.Message, LogMessageType.Error, "Ship Import-ReadCommand");
                logger.Error("Could not read file {0}", temRevPath);
                logger.Info("Import process terminated unsuccessfully.  - {0}", DateTime.Now.ToString());
            }
            if(!String.IsNullOrEmpty(commandFile))
            {
                xDoc.Load(commandFile);

                foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
                {
                    if(node.Name == "ships")
                    {
                        foreach (XmlNode snode in node)
                        {
                            string nodeId = snode.Attributes["id"].Value.ToString();
                            if (nodeId == shipId)
                            {
                                foreach (XmlNode item in snode)
                                {
                                    if (item.Name == "filename")
                                    {
                                        string filename = item.InnerText.ToString();
                                        string type = item.Attributes["type"].Value.ToString();
                                        string operation = item.Attributes["operation"].Value.ToString();
                                        string path = item.Attributes["path"].Value.ToString();
                                        string sourceFile = "";
                                        string destinationFile = "";

                                        sourceFile = Path.Combine(temRevPath, filename);
                                        destinationFile = Path.Combine(destinationRootPath + path, filename);
                                        switch (type)
                                        {
                                            case "SQL":
                                                DirectoryInfo d = new DirectoryInfo(temRevPath);
                                                FileInfo file = d.GetFiles(filename).FirstOrDefault();
                                                ExecuteSql(file);
                                                break;
                                            case "PDF":
                                                if (operation == "")
                                                {
                                                    if (File.Exists(sourceFile))
                                                    {
                                                        File.Copy(sourceFile, destinationFile, true);
                                                        TccLog.UpdateLog("Source: "+ sourceFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("Destination: "+ destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("PDF Copied", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }
                                                        
                                                }
                                                else if (operation == "remove")
                                                {
                                                    if (File.Exists(destinationFile))
                                                    {
                                                        File.Delete(destinationFile);
                                                        TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("PDF Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }
                                                       
                                                }
                                                break;
                                            case "XLS":
                                                if (operation == "")
                                                {
                                                    if (File.Exists(sourceFile))
                                                    {
                                                        File.Copy(sourceFile, destinationFile, true);
                                                        TccLog.UpdateLog("Source: " + sourceFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("XLS Copied", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }
                                                        
                                                }
                                                else if (operation == "remove")
                                                {
                                                    if (File.Exists(destinationFile))
                                                    {

                                                        File.Delete(destinationFile);
                                                        TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("XLS Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }

                                                }
                                                break;
                                            case "DOC":
                                                if (operation == "")
                                                {
                                                    if (File.Exists(sourceFile))
                                                    {
                                                        File.Copy(sourceFile, destinationFile, true);
                                                        TccLog.UpdateLog("Source: " + sourceFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("DOC Copied", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }
                                                        
                                                }
                                                else if (operation == "remove")
                                                {
                                                    if (File.Exists(destinationFile))
                                                    {

                                                        File.Delete(destinationFile);
                                                        TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand");
                                                        TccLog.UpdateLog("XLS Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                                    }
                                                }
                                                break;

                                        }
                                    }
                                }
                            }

                        }

                    }
                    else if(node.Name == "others")
                    {
                        foreach (XmlNode onode in node)
                        {
                            string nodeName = onode.Name;
                            switch(nodeName)
                            {
                                case "volume":
                                    string nodeId = node.Attributes["id"].Value.ToString();
                                    ReadChildNode(onode);
                                    break;
                                case "common2all":
                                    ReadChildNode(onode);
                                    break;
                                case "referencematerials":
                                    ReadChildNode(onode);
                                    break;
                            }
                        }

                    }
                }
            }
            string[] tempRevFiles = Directory.GetFiles(temRevPath + "\\");
            foreach (string files in tempRevFiles)
            {
                File.Delete(files);
            }
            logger.Info("Temp Revision Files Deleted . - {0}", DateTime.Now.ToString());
            TccLog.UpdateLog("Temporary Revision File Deletion Complete", LogMessageType.Info, "Ship Import -ReadCommand");
        }

        public static void ReadChildNode(XmlNode cNode)
        {
            string temRevPath = Path.Combine(Path.GetDirectoryName(extractPath), "TempRevision");
            string destinationRootPath = ConfigurationManager.AppSettings["rootPath"].ToString();

            foreach (XmlNode item in cNode)
            {
                if (item.Name == "filename")
                {
                    string filename = item.InnerText.ToString();
                    string type = item.Attributes["type"].Value.ToString();
                    string operation = item.Attributes["operation"].Value.ToString();
                    string path = item.Attributes["path"].Value.ToString();
                    string sourceFile = "";
                    string destinationFile = "";

                    sourceFile = Path.Combine(temRevPath, filename);
                    destinationFile = Path.Combine(destinationRootPath + path, filename);
                    switch (type)
                    {
                        case "SQL":
                            DirectoryInfo d = new DirectoryInfo(temRevPath);
                            FileInfo file = d.GetFiles(filename).FirstOrDefault();
                            ExecuteSql(file);
                            break;
                        case "PDF":
                            if (operation == "")
                            {
                                if (File.Exists(sourceFile))
                                {
                                    File.Copy(sourceFile, destinationFile, true);
                                    TccLog.UpdateLog("Source: " + sourceFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("PDF Copied", LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                }

                            }
                            else if (operation == "remove")
                            {
                                if (File.Exists(destinationFile))
                                {
                                    File.Delete(destinationFile);
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("PDF Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                }

                            }
                            break;
                        case "XLS":
                            if (operation == "")
                            {
                                if (File.Exists(sourceFile))
                                {
                                    File.Copy(sourceFile, destinationFile, true);
                                    TccLog.UpdateLog("Source: " + sourceFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("XLS Copied", LogMessageType.Info, "Ship Import-ReadCommand");
                                }

                            }
                            else if (operation == "remove")
                            {
                                if (File.Exists(destinationFile))
                                {
                                    
                                    File.Delete(destinationFile);
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("XLS Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                }
                                    

                            }
                            break;
                        case "DOC":
                            if (operation == "")
                            {
                                if (File.Exists(sourceFile))
                                {
                                    File.Copy(sourceFile, destinationFile, true);
                                    TccLog.UpdateLog("Source: " + sourceFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("DOC Copied", LogMessageType.Info, "Ship Import-ReadCommand");
                                }

                            }
                            else if (operation == "remove")
                            {
                                if (File.Exists(destinationFile))
                                {

                                    File.Delete(destinationFile);
                                    TccLog.UpdateLog("Destination: " + destinationFile, LogMessageType.Info, "Ship Import-ReadCommand-Child");
                                    TccLog.UpdateLog("DOC Deleted", LogMessageType.Info, "Ship Import-ReadCommand");
                                }
                            }
                            break;

                    }
                }
            }
        }
        public static void ExecuteSql(FileInfo file)
        {
            TccLog.UpdateLog("Start Executing SQL", LogMessageType.Info, "Ship Import -ExecuteSql");
            string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;
            string sql              = String.Empty;
            string fileName         = String.Empty;

            fileName                = file.Name;
            FileStream fs           = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            //create byte array of same size as FileStream length
            byte[] fileBytes        = new byte[fs.Length];
            //define counter to check how much bytes to read. Decrease the counter as you read each byte
            int numBytesToRead      = (int)fileBytes.Length;
            //Counter to indicate number of bytes already read
            int numBytesRead        = 0;

            //iterate till all the bytes read from FileStream
            while (numBytesToRead > 0)
            {
                int n = fs.Read(fileBytes, numBytesRead, numBytesToRead);

                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            //Once you read all the bytes from FileStream, you can convert it into string using UTF8 encoding
            sql = Encoding.UTF8.GetString(fileBytes);
            SqlConnection con = new SqlConnection(connectionString);
            int recordsAffected = 0;
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                recordsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (recordsAffected == 1)
                {
                    //richTextBox1.AppendText("******" + fileName + " execute successfully" + "***********" + Environment.NewLine);
                    TccLog.UpdateLog("******" + fileName + " execute successfully" + "***********", LogMessageType.Error, "Ship Import-ExecuteSql");
                }
                else
                {
                    string failureText = "#######" + fileName + " execution un-successfull" + "######################";
                    // richTextBox1.AppendText(failureText, Color.Red);
                    TccLog.UpdateLog(failureText, LogMessageType.Error, "Ship Import-ExecuteSql");
                }

                con.Close();
            }
        }
        public static void ArchiveZipFiles(string f)
        {
            TccLog.UpdateLog("Start Archiving", LogMessageType.Info, "Ship Import -ExecuteSql");
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
                    TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Import-ArchiveZipFiles");
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
            TccLog.UpdateLog("Copy uploaded file Started", LogMessageType.Info, "Ship Import");

            string sourceFilePath = Path.Combine(Path.GetDirectoryName(extractPath), "temp");

            DataSet dataSet = new DataSet();
            dataSet.ReadXmlSchema(xmlFile);
            dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);
            string[] sourceFiles = Directory.GetFiles(sourceFilePath);


            //----------------------------------------------------------------
            try
            {
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
                    TccLog.UpdateLog(destinationFilePath, LogMessageType.Info, "Ship Import- CopyUploadedFiles");
                    logger.Info(tempSourcePath + ". - {0}", DateTime.Now.ToString());
                    TccLog.UpdateLog(tempSourcePath, LogMessageType.Info, "Ship Import - CopyUploadedFiles");

                    if (File.Exists(tempSourcePath))
                    {
                        File.Copy(tempSourcePath, Path.Combine(destinationFilePath, uploadedFileName), true);
                        TccLog.UpdateLog("File copied from Temp to IIS", LogMessageType.Error, "Ship Export-CreateUploadedZipFile- foreach");
                    }
                    else
                    {
                        TccLog.UpdateLog("File not copied from Temp to IIS", LogMessageType.Error, "Ship Export-CreateUploadedZipFile- foreach");
                    }

                }

                isMailReadSuccessful = false;
                //System.IO.File.Move(sourceFilePath, destinationFilePath);
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import- CopyUploadedFiles");
                logger.Error(ex.Message);
                logger.Info("Import process terminated unsuccessfully in CopyUploadedFiles. - {0}", DateTime.Now.ToString());
            }
            
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
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Import-ZipDirectoryContainsFiles");
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
                TccLog.UpdateLog("Ticket Import Complete", LogMessageType.Info, "Ship Import");
                RevisionHeader();
                logger.Info("Revision Header Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Revision Header Import Complete", LogMessageType.Info, "Ship Import");
                RevisionDetails();
                logger.Info("Revision History Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Revision History Import Complete", LogMessageType.Info, "Ship Import");
                FillupFormsUploaded();
                logger.Info("Fillup Forms  Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Forms  Import Complete", LogMessageType.Info, "Ship Import");
                FillupFormApproverMapper();
                logger.Info("Fillup Form Approver Mapper Import Complete. - {0}", DateTime.Now.ToString());
                TccLog.UpdateLog("Fillup Form Approver Mapper Import Complete", LogMessageType.Info, "Ship Import");

            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-ImportData");
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
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Import-UpdateLastSyncDate");
                logger.Error(ex.Message);
                logger.Info("Import process terminated unsuccessfully while updating last sysn date. - {0}", DateTime.Now.ToString());
                //Environment.Exit(0);
            }


        }

        public static void Ticket()
        {
            try
            {
                TccLog.UpdateLog("Importing Ticket", LogMessageType.Info, "Ship Import-Ticket");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlTicket"].ToString();
                int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());
                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportTicketInShip", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        //cmd.Parameters.AddWithValue("@ID", int.Parse(row["ID"].ToString()));
                        string ticketNumber = row["TicketNumber"].ToString();
                        if (ShipId == int.Parse(row["ShipId"].ToString()))
                        {
                            cmd.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                            if (row["IsSolved"] != DBNull.Value)
                            {
                                //string s = row["IsSolved"].ToString();

                                cmd.Parameters.AddWithValue("@IsSolved", int.Parse(row["IsSolved"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@IsSolved", DBNull.Value);
                            }

                            cmd.Parameters.AddWithValue("@ShipId", int.Parse(row["ShipId"].ToString()));
                            if (row["UpdatedBy"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@UpdatedBy", int.Parse(row["UpdatedBy"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@UpdatedBy", DBNull.Value);
                            }
                            if (row["UpdatedAt"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Parse(row["UpdatedAt"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@UpdatedAt", DBNull.Value);
                            }
                            int x = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            TccLog.UpdateLog(ticketNumber+ " is imported into DB Successfully", LogMessageType.Info, "Ship Import-Ticket- Forceach");
                        }


                    }
                }
                else
                {
                    TccLog.UpdateLog("Xml path: "+ xmlFile, LogMessageType.Info, "Ship Import-Ticket");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Ship Import-Ticket");
                }
                
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-Ticket");
                logger.Error(ex, "Ticket Import");
                //throw;
            }
        }

        public static void RevisionHeader()
        {
            try
            {
                TccLog.UpdateLog("Importing RevisionHeader", LogMessageType.Info, "Ship Import-RevisionHeader");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlRevisionHeader"].ToString();
                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportRevisionHeaderInShip", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        string revisionNo = row["RevisionNo"].ToString();
                        cmd.Parameters.AddWithValue("@Id", int.Parse(row["Id"].ToString()));
                        cmd.Parameters.AddWithValue("@RevisionNo", revisionNo);
                        cmd.Parameters.AddWithValue("@RevisionDate", row["RevisionDate"].ToString());
                        //cmd.Parameters.AddWithValue("@CreatedAt", row["CreatedAt"].ToString());
                        if (row["CreatedAt"] != DBNull.Value)
                        {
                            cmd.Parameters.AddWithValue("@CreatedAt", row["CreatedAt"].ToString());
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

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        TccLog.UpdateLog("Revision No " + revisionNo + " is imported into DB Successfully", LogMessageType.Info, "Ship Import-RevisionHeader- Forceach");
                    }
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Ship Import-RevisionHeader");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Ship Import-RevisionHeader");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Import-RevisonHeader");
                logger.Error(ex, "Crew Import");
                //throw;
            }
        }

        public static void RevisionDetails()
        {
            try
            {
                TccLog.UpdateLog("Importing RevisionDetails", LogMessageType.Info, "Ship Import-RevisionDetails");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlRevisionHistory"].ToString();

                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportRevisionHistoryInShip", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        string chapter = row["Chapter"].ToString();
                        cmd.Parameters.AddWithValue("@RevisionHistoryId", int.Parse(row["RevisionHistoryId"].ToString()));
                        cmd.Parameters.AddWithValue("@Chapter", chapter);
                        cmd.Parameters.AddWithValue("@Section", row["Section"].ToString());
                        cmd.Parameters.AddWithValue("@ChangeComment", row["ChangeComment"].ToString());
                        cmd.Parameters.AddWithValue("@ModificationDate", row["ModificationDate"].ToString());
                        cmd.Parameters.AddWithValue("@HeaderId", int.Parse(row["HeaderId"].ToString()));

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        TccLog.UpdateLog("Chapter name " + chapter + " is imported into DB Successfully", LogMessageType.Info, "Ship Import-RevisionDetails- Forceach");
                    }
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Ship Import-RevisionDetails");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Ship Import-RevisionDetails");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-RevisionDetails");
                logger.Error(ex, "Revision  Import");
                //throw;
            }
        }

        public static void FillupFormsUploaded()
        {
            try
            {
                TccLog.UpdateLog("Importing FillupFormsUploaded", LogMessageType.Info, "Ship Import-FillupFormsUploaded");
                // Here your xml file
                string xmlFile = extractPath + "\\" + ConfigurationManager.AppSettings["xmlFillupFormUpload"].ToString();
                int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());

                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportFillupUoloadedFormsInShip", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        string uploadedFileName = string.Empty;
                        string relativePath = string.Empty;

                        if (ShipId == int.Parse(row["ShipId"].ToString()))
                        {
                            string formName = row["FormsName"].ToString();
                            cmd.Parameters.AddWithValue("@FormsName", formName);
                            //cmd.Parameters.AddWithValue("@IsApprove", int.Parse(row["IsApprove"].ToString()));
                            if (row["IsApprove"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@IsApprove", int.Parse(row["IsApprove"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@IsApprove", DBNull.Value);
                            }
                            uploadedFileName = row["FormsName"].ToString();
                            relativePath = row["FormsPath"].ToString();

                            if (row["ApprovedOn"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@ApprovedOn", DateTime.Parse(row["ApprovedOn"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ApprovedOn", DBNull.Value);
                            }



                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            TccLog.UpdateLog("Form name " + formName + " is imported into DB Successfully", LogMessageType.Info, "Ship Import-FillupFormsUploaded- Forceach");

                        }

                    }

                    CopyUploadedFiles("FILLUPUPLOADEDFILE", xmlFile);
                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Ship Import-FillupFormsUploaded");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Ship Import-FillupFormsUploaded");
                }

                //if (File.Exists(xmlFile))
                //    CopyUploadedFiles("FILLUPUPLOADEDFILE", xmlFile);
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-FilupFormsUploaded");
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
                int ShipId = int.Parse(ConfigurationManager.AppSettings["SHIPID"].ToString());

                if (File.Exists(xmlFile))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXmlSchema(xmlFile);
                    dataSet.ReadXml(xmlFile, XmlReadMode.ReadSchema);

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("stpImportApprovedFillupFormApproverInShip", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (ShipId == int.Parse(row["ShipId"].ToString()))
                        {
                            string formName = row["UploadedFormName"].ToString();
                            cmd.Parameters.AddWithValue("@UploadedFormName", formName);
                            cmd.Parameters.AddWithValue("@ApproverUserId", int.Parse(row["ApproverUserId"].ToString()));
                            //cmd.Parameters.AddWithValue("@IsApprove", int.Parse(row["IsApprove"].ToString()));
                            if (row["IsApprove"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@IsApprove", int.Parse(row["IsApprove"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@IsApprove", DBNull.Value);
                            }
                            if (row["ApprovedOn"] != DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@ApprovedOn", DateTime.Parse(row["ApprovedOn"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ApprovedOn", DBNull.Value);
                            }

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            TccLog.UpdateLog("Form name " + formName + " is imported into DB Successfully", LogMessageType.Info, "Ship Import-FillupFormApproverMapper- Forceach");

                        }

                    }

                }
                else
                {
                    TccLog.UpdateLog("Xml path: " + xmlFile, LogMessageType.Info, "Ship Import-FillupFormApproverMapper");
                    TccLog.UpdateLog("XML not Found", LogMessageType.Info, "Ship Import-FillupFormApproverMapper");
                }
            }
            catch (Exception ex)
            {
                TccLog.UpdateLog(ex.InnerException.Message, LogMessageType.Error, "Ship Import-FillupFormApproverMapper");
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
                TccLog.UpdateLog("Import Zip from Mailbox Started.", LogMessageType.Info, "Ship Import- ImportMail");
                string mTyp = GetConfigData("protocol");
                //Creating Mail configuration 
                MailServiceConfiguration serviceconf = new MailServiceConfiguration
                {
                    MailId              = GetConfigData("shipemail"),
                    //MailPassword = GetConfigData("shipemailpwd"),
                    MailPassword        = EncodeDecode.DecryptString(GetConfigData("shipemailpwd")),
                    SubjectLine         = GetConfigData("tccAsubject"),
                    MailServerDomain    = GetConfigData("imappopServer"),
                    Port                = int.Parse(GetConfigData("imappopport")),
                    MailServerType      = mTyp,

                    AttachmentPath      = zipPath,

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
                TccLog.UpdateLog("Seivice Cofig created.", LogMessageType.Info, "Ship Import- ImportMail");
                MailOps mailops = new MailOps
                {
                    MailServerType = serviceconf.MailServerType
                };
                TccLog.UpdateLog("Signing in the Email.", LogMessageType.Info, "Ship Import- ImportMail");
                //mailops.Connect(serviceconf.MailId, Security.DecryptString(serviceconf.MailPassword), serviceconf.MailServerDomain, serviceconf.Port);
                mailops.Connect(serviceconf.MailId, serviceconf.MailPassword, serviceconf.MailServerDomain, serviceconf.Port);
                TccLog.UpdateLog("Connect Sucessfull.", LogMessageType.Info, "Ship Import- ImportMail");
                mailops.DownloadAllNewMails(serviceconf.SubjectLine, serviceconf.AttachmentPath);
                TccLog.UpdateLog("Download Sucessfull", LogMessageType.Info, "Ship Import - ImportMail");

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
