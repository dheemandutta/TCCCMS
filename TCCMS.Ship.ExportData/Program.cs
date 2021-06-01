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

namespace TCCMS.Ship.ExportData
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
                    // using (ZipFile zip = new ZipFile())
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
                FillupFormsUploaded();
                FillupFormApproverMapper();
                Ticket();
                RevisionViewer();

            }
            catch (Exception ex)
            {

                logger.Error("Error in ExportData. - {0}", ex.Message + " :" + ex.InnerException);
                logger.Info("Export process terminated unsuccessfully in ExportData.");
                //Environment.Exit(0);
            }
        }



        public static void FillupFormsUploaded()
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
        }

        public static void FillupFormApproverMapper()
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

        public static void Ticket()
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
        }

        public static void RevisionViewer()
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

    }
}

