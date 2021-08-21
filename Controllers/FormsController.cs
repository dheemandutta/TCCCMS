using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Net.Mail;


using GemBox.Document;
using GemBox.Document.Tables;
using GemBox.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Configuration;

namespace TCCCMS.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult ListOfAdministrationMmanagementForms()
        {
            return View();
        }

        public ActionResult ListOfMaintenanceFroms()
        {
            return View();
        }

        public ActionResult ListOfMmanagementOfShipPersonalForms()
        {
            return View();
        }

        public ActionResult NavigationMmanagementForms()
        {
            return View();
        }
        public ActionResult SafetyMmanagementForms()
        {
            return View();
        }


        public JsonResult LoadData(int CategoryId)
        {
            int draw, start, length;
            int pageIndex = 0;

            if (null != Request.Form.GetValues("draw"))
            {
                draw = int.Parse(Request.Form.GetValues("draw").FirstOrDefault().ToString());
                start = int.Parse(Request.Form.GetValues("start").FirstOrDefault().ToString());
                length = int.Parse(Request.Form.GetValues("length").FirstOrDefault().ToString());
            }
            else
            {
                draw = 1;
                start = 0;
                length = 50000;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            DownloadableFromsBL bL = new DownloadableFromsBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<DownloadableFroms> pocoList = new List<DownloadableFroms>();
            pocoList = bL.GetDownloadableFromsPageWise(pageIndex, ref totalrecords, length, CategoryId);
            List<DownloadableFroms> pList = new List<DownloadableFroms>();
            foreach (DownloadableFroms pC in pocoList)
            {
                DownloadableFroms pOCO = new DownloadableFroms();
                pOCO.ID         = pC.ID;
                pOCO.CategoryId = pC.CategoryId;// Added on 5th Aug 2021 @BK
                pOCO.FormName   = pC.FormName;
                pOCO.Path       = pC.Path;
                pOCO.Version    = pC.Version;
                pOCO.IsUpload   = pC.IsUpload;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Modified on 9th Aug 2021 @BK
        /// </summary>
        /// <param name="currentPage"> Added on 9th Aug 2021 @BK</param>
        /// <returns></returns>
        public ActionResult FormsApprovalList(int currentPage = 1)
        {
            ApprovedFilledupFormAndApproverViewModel affaVM = new ApprovedFilledupFormAndApproverViewModel();
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            formList = documentBL.GetFilledupFormRequiredApprovalList(Convert.ToInt32(Session["UserId"].ToString()));

            //*******************************Added on 9th Aug 2021 @BK****************************************//
            var pager = new Pager(formList.Count(), currentPage);
            affaVM.ApprovedFormList = formList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            affaVM.ApprovedFormList = affaVM.ApprovedFormList.OrderBy(a => a.IsApproved).ToList();
            affaVM.Pager = pager;
            //**********End*********************Added on 9th Aug 2021 @BK****************************************//

            //return View(formList);
            return View(affaVM);
        }

        public JsonResult ApproveFilledUpForm(Forms form)
        {
            string path             = Server.MapPath("~/UploadFilledUpFormForApproval");
            string uploadedForm     = form.FilledUpFormName;
            int filledUpFormId      = form.ID;
            int formsCategory       = form.CategoryId;//Added on 5th Aug 2021 @BK
            DocumentBL documentBl   = new DocumentBL();
            int x                   = documentBl.ApproveFilledUpForm(filledUpFormId, Convert.ToInt32(Session["UserId"].ToString()), uploadedForm);
            int y = 0;
            WriteErrorToText("Approved in DB", "ApproveFilledUpForm");

            if(x > 0)
            {
                //Added below condition on 5th Aug 2021 @BK
                if (formsCategory == 16 || formsCategory == 17)
                {
                    y = AddApproverOrReviewerSignatureInReviewedForm("A",path, uploadedForm, Convert.ToInt32(Session["UserId"].ToString()));
                }
                else
                {
                    y = AddSignatureInForm(path, uploadedForm, Convert.ToInt32(Session["UserId"].ToString()));
                }

                //y = AddSignatureInForm(path, uploadedForm, Convert.ToInt32(Session["UserId"].ToString())); //--commented on 5th Aug 2021 @BK
            }

            if (y == 1)
            {
                WriteErrorToText("Sign added in form", "ApproveFilledUpForm");
                if (System.IO.File.Exists(Path.Combine(path+"\\Temp\\", uploadedForm)))
                {
                    System.IO.File.Copy(Path.Combine(path + "\\Temp\\", uploadedForm), Path.Combine(path, uploadedForm),true);

                    System.IO.File.Delete(Path.Combine(path + "\\Temp\\", uploadedForm));
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Modified on 3rd Jul 2021 @BK 
        /// add argument 'qr'
        /// </summary>
        /// <param name="qr"></param>
        /// <param name="currentPage">Added on 9th Aug 2021 @bk</param>
        /// <returns></returns>
        public ActionResult FillupFormList(int currentPage = 1, string qr = "2")
        {
            ApprovedFilledupFormAndApproverViewModel affaVM = new ApprovedFilledupFormAndApproverViewModel();
            DocumentBL documentBL = new DocumentBL();
            affaVM = documentBL.GetApprovedFilledUpForms(Convert.ToInt32(Session["UserId"].ToString()), currentPage); // UserId not in use from 3rd Jul 2021
            
            if(Convert.ToInt32(qr) != 2)// added on 03/07/2021 @BK
            {
                var res = affaVM.ApprovedFormList.Where(af => af.IsApproved == Convert.ToInt32(qr)).ToList();
                affaVM.ApprovedFormList = res;
            }
            //else if(Convert.ToInt32(qr) == 1)
            //{
            //    var res = affaVM.ApprovedFormList.Where(af => af.IsApproved = 1);
            //}
            
            //****************Added on 9th Aug 2021 @bk**********************//
            var pager = new Pager(affaVM.ApprovedFormList.Count(), currentPage);
            affaVM.ApprovedFormList= affaVM.ApprovedFormList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            affaVM.Pager=pager;
            affaVM.IsApprove= Convert.ToInt32(qr);
            //********End********Added on 9th Aug 2021 @bk**********************//

            return View(affaVM);
        }

        public JsonResult SendMailForapproval(string approvalId,string approverUserId,string formName, string task)
        {
            bool isSendSuccessfully = false;
            //UserMasterBL umBL       = new UserMasterBL();

            isSendSuccessfully = SendMailToApprover(approverUserId, formName,task);

            if (isSendSuccessfully)
            {
                return Json("Mail Sent Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Send Mail Failed", JsonRequestBehavior.AllowGet);
            }
            //try
            //{
            //    UserMasterPOCO um       = new UserMasterPOCO();
            //    StringBuilder mailBody  = new StringBuilder();
            //    string senderEmail      = string.Empty;
            //    string receiverEmail    = string.Empty;
            //    string lTask            = string.Empty;
            //    string lSubject         = string.Empty;
            //    if (task == "R")
            //    {
            //        lTask = "Review";
            //        lSubject = "FormsReview";
            //    }
            //    else if (task == "A")
            //    {
            //        lTask = "Approve";
            //        lSubject = "FormsApproval";
            //    }

            //    if (Session["UserType"].ToString() == "1")
            //    {
            //        senderEmail = ConfigurationManager.AppSettings["shipEmail"];
            //    }
            //    else
            //    {
            //        senderEmail = Session["Email"].ToString();
            //    }

            //    um = umBL.GetUserByUserId(Convert.ToInt32( approverUserId));
            //    receiverEmail = um.Email.ToString();

            //    MailMessage mail = new MailMessage();

            //    mailBody.Append("Form Name : ");
            //    mailBody.Append(formName.ToString());
            //    mailBody.Append("\n");
            //    mailBody.Append("\n");
            //    mailBody.Append("Messege : ");
            //    mailBody.Append("You are requested to "+ lTask + " the above Form. Please Login and " + lTask + " the Form.");
            //    mailBody.Append("\n");

            //    if (!String.IsNullOrEmpty(senderEmail) && !String.IsNullOrEmpty(receiverEmail))
            //    {
                    
            //        SendEmail.SendMail(lSubject, senderEmail, receiverEmail, mail, ref isSendSuccessfully);
            //    }

            //    return Json("Mail Sent Successfully", JsonRequestBehavior.AllowGet);
            //}
            //catch(Exception ex)
            //{
            //    return Json("Send Mail Failed", JsonRequestBehavior.AllowGet);
            //}

            //return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFillupFormsListForNotification()
        {
            DocumentBL bL = new DocumentBL(); 
            int totalrecords = 0;
            string approverUserId = Session["UserId"].ToString();
            List<Forms> pocoList = new List<Forms>();
            List<Forms> frmList = new List<Forms>();
            pocoList = bL.GetFillupFormsListForNotification(Convert.ToInt32(approverUserId));
            totalrecords = pocoList.Count();

            foreach(Forms frm in pocoList)
            {
                Forms f = new Forms();
                f.FilledUpFormName = frm.FilledUpFormName.CheckStringLenghtAndGetFirstFewCharecters(25);
                frmList.Add(f);
            }
            var data = frmList;
            return Json(new {recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public static int AddSignatureInForm(string relPath,string uploadedFormName,int approverUserId)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";

            int x = 0;

            try
            {
                //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                ComponentInfo.SetLicense("DN-2021Jan04-gSb72AQqrg9T4PQnvYNDgVtyd4tD3W3oBds51kfYp7zSsuFpxRw1a5Cxr49JiCLbMf2JCIKuinkUhgiQmuOz5yMoWdA==A");

                //string tempPath         = Path.Combine(relPath, "\\Temp\\");
                string tempPath = relPath+ "\\Temp\\";

                //string signPath = @"E:\WFH\TCC\WordModify\logo.png";
                //string docPath          = Path.Combine(tempPath, uploadedFormName);
                string docPath = tempPath + uploadedFormName;

                //string sdocPath         = Path.Combine(relPath, "\\"+uploadedFormName);
                string sdocPath = relPath + "\\" + uploadedFormName;

                string root = Path.GetDirectoryName(relPath);

                ApproverMaster approver = new ApproverMaster();
                ApproverSignBL aSignBL  = new ApproverSignBL();
                approver                = aSignBL.GetAllApproverSign(approverUserId, uploadedFormName);

                string decryptSignPath = AesOperation.DecryptString(key, approver.SignImagePath.ToString());

                string signPath         = Path.Combine(root+"\\", decryptSignPath.Replace("/","\\"));
                string approverName     = approver.Name;
                string designation      = approver.Position;
                int approverPossition   = approver.ApprovedCount;

                //int numberOfItems       = 4;// Number of rows of the signature tablee
                int numberOfItems       = approver.ApproversCount;// Added on 24th Jul 2021@BK

                DocumentModel document  = DocumentModel.Load(sdocPath);

                // Template document contains 4 tables, each contains some set of information.
                Table[] tables          = document.GetChildElements(true, ElementType.Table).Cast<Table>().ToArray();

                int tableCount          = tables.Count();

                Table signatureTable    = tables[tableCount - 1];

                for (int rowIndex = 0; rowIndex <= numberOfItems; rowIndex++)
                {
                    //DateTime date = DateTime.Today.AddDays(rowIndex - numberOfItems);
                    //int hours = rowIndex % 3 + 6;
                    //int unit = 35;
                    //int price = hours * unit;
                    if (rowIndex == approverPossition)
                    {
                        var paragraph = new Paragraph(document);

                        Picture picture1 = new Picture(document, signPath, 100, 35, GemBox.Document.LengthUnit.Pixel);
                        paragraph.Inlines.Add(picture1);

                        signatureTable.Rows[rowIndex].Cells[1].Blocks.Add(new Paragraph(document,
                                                                              new Run(document, "Name :" + approverName),
                                                                              new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                                                                              new Run(document, "Designation : " + designation)
                                                                          ));
                        signatureTable.Rows[rowIndex].Cells[2].Blocks.Add(paragraph);
                    }
                }
                ////------Need append new row at end of the Signature Table to add final Approved Date
                //code here
                if(approver.IsFinalApproved > 0)
                {
                    DateTime approvedDate = approver.FinalApprovedOn;
                    string d = approvedDate.Day.ToString(); /*DateTime.Now.Day.ToString();*/
                    string y = approvedDate.Year.ToString();
                    string m = approvedDate.Month.ToString();
                    string H = approvedDate.TimeOfDay.ToString();
                    string date = d + "th" + " " + m + " " + y + " " + H;



                    var cel = new TableCell(document) { ColumnSpan = 3 };
                    var para = new Paragraph(document, "Approved: "+ date);

                    cel.CellFormat.VerticalAlignment = (VerticalAlignment)0;
                    cel.Blocks.Add(para);
                    para.ParagraphFormat.Alignment = (GemBox.Document.HorizontalAlignment)3;

                    signatureTable.Rows.Add(new TableRow(document, cel));
                }


                document.Save(docPath);
                x = 1;
            }
            catch (Exception ex)
            {
                x = 0;
            }


            return x;
        }


        public static int AppendSignatureTable(string relPath, string uploadedFormName,int approverCount)
        {
            int x = 0;
            try
            {
                //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                ComponentInfo.SetLicense("DN-2021Jan04-gSb72AQqrg9T4PQnvYNDgVtyd4tD3W3oBds51kfYp7zSsuFpxRw1a5Cxr49JiCLbMf2JCIKuinkUhgiQmuOz5yMoWdA==A");

                //string tempPath         = Path.Combine(relPath, "\\Temp\\");
                string tempPath = relPath + "Temp\\";
                string sigTableName = "Approvers Digital Signature";
                string extn = ".docx";
                if(approverCount > 0)
                {
                    sigTableName = sigTableName + approverCount + extn;

                    string tblPath = tempPath + sigTableName;
                    string sdocPath = relPath + uploadedFormName;

                    string appendedDocPath = tempPath + uploadedFormName;

                    // Word files that will be combined into one file.
                    string[] files ={
                        sdocPath,
                        tblPath
                    };

                    // Create destination document.
                    var destination = new DocumentModel();

                    // Merge multiple source documents by importing their content at the end.
                    foreach (var file in files)
                    {
                        var source = DocumentModel.Load(file);
                        
                        destination.Content.End.InsertRange(source.Content);
                    }

                    // Save joined documents into one file.
                    destination.Save(appendedDocPath);

                    x = 1;
                }
                else
                {
                    x = 0;
                }
                

            }
            catch(Exception ex)
            {
                x = 0;
            }

            return x;
        }

        public static int ConvertToPDFForApproval(string relPath, string uploadedFormName,string task)
        {
            int x = 0;
            try
            {
                //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                ComponentInfo.SetLicense("DN-2021Jan04-gSb72AQqrg9T4PQnvYNDgVtyd4tD3W3oBds51kfYp7zSsuFpxRw1a5Cxr49JiCLbMf2JCIKuinkUhgiQmuOz5yMoWdA==A");

                //string tempPath         = Path.Combine(relPath, "\\Temp\\");
                //string tempPath = relPath + "Temp\\";

                string fNameWithoutExtn = Path.GetFileNameWithoutExtension(uploadedFormName);
                string ext = Path.GetExtension(uploadedFormName).ToLowerInvariant();
                string destinationFile = relPath + fNameWithoutExtn +".pdf";

                
                if (task == "A" && (ext == ".doc" || ext == ".docx"))
                {
                    var source = DocumentModel.Load(relPath + uploadedFormName);
                    source.Save(destinationFile);
                    x = 1;
                }
                else if (task == "A" && (ext == ".xls" || ext == ".xlsx"))
                {
                    // If using Professional version, put your serial key below.
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                    // In order to convert Excel to PDF, we just need to:
                    //   1. Load XLS or XLSX file into ExcelFile object.
                    //   2. Save ExcelFile object to PDF file.
                    ExcelFile workbook = ExcelFile.Load(relPath + uploadedFormName);
                    workbook.Save(destinationFile, new GemBox.Spreadsheet.PdfSaveOptions() { SelectionType = SelectionType.EntireFile });
                    x = 1;
                }
               


            }
            catch (Exception ex)
            {
                x = 0;
            }

            return x;
        }


        public void WriteErrorToText(string subName, string fileName)
        {
            string contentRootPath = Server.MapPath("~/Log/");
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                string filepath = contentRootPath;  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                //filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                filepath = filepath + subName + "_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!System.IO.File.Exists(filepath))
                {


                    System.IO.File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    //sw.WriteLine(line);
                    sw.WriteLine("File Name : " + fileName);
                    //sw.WriteLine("Path : " + filePath);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }

        /// <summary>
        /// UploadFilledUpForm is old because form approval logics
        /// has changed.
        /// changed after 7th APR 2021
        /// </summary>
        /// <param name="approvers"></param>
        /// <returns></returns>
        //[HttpPost]

        //public ActionResult UploadFilledUpForm(string approvers = null)
        //{
        //    string catchMessage = "";
        //    List<Forms> formList = new List<Forms>();
        //    DocumentBL documentBL = new DocumentBL();
        //    List<ApproverMaster> approvers1 = new List<ApproverMaster>();
        //    //approvers1 = (List<ApproverMaster>)JsonConvert.DeserializeObject(approvers);
        //    string s1 = JsonConvert.SerializeObject(approvers);
        //    approvers1 = JsonConvert.DeserializeObject<List<ApproverMaster>>(approvers);
        //    //string s = (string)JsonConvert.DeserializeObject(approvers);

        //    if (Request.Files.Count == 1)
        //    {
        //        try
        //        {
        //            string relativePath = "~/UploadFilledUpFormForApproval/";
        //            string path = Server.MapPath(relativePath);
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            //  Get all files from Request object  
        //            HttpFileCollectionBase files = Request.Files;

        //            //---For Single form
        //            Forms form = new Forms();

        //            HttpPostedFileBase file = files[0];
        //            string fname;

        //            // Checking for Internet Explorer  
        //            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //            {
        //                string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                fname = testfiles[testfiles.Length - 1];
        //            }
        //            else
        //            {
        //                fname = file.FileName;
        //            }
        //            string uniqueFormName   = GetUniqueFileNameWithUserId(fname);
        //            // Get the complete folder path and store the file inside it.  
        //            string fnameWithPath    = Path.Combine(path, uniqueFormName);
        //            file.SaveAs(fnameWithPath);

        //            form.FormName           = fname;
        //            form.FilledUpFormName   = uniqueFormName;
        //            form.FilePath           = relativePath;
        //            //form.ShipId             = Convert.ToInt32(shipId);
        //            //form.Approvers          = approvers;
        //            form.CreateedBy         = 1;//--- userId
        //            //---End---For Single form
        //            int count               = documentBL.SaveFilledUpForm(form, approvers1,ref catchMessage);
        //            // Returns message that successfully uploaded  
        //            return Json("File Uploaded Successfully!");
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("Error occurred. Error details: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}

        /// <summary>
        /// UploadFilledUpForm is New because form approval logics
        /// in this method forms approved by Company user only who has to right to approve
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult UploadFilledUpForm()
        //{
        //    string catchMessage = "";
        //    List<Forms> formList = new List<Forms>();
        //    DocumentBL documentBL = new DocumentBL();
            
        //    if (Request.Files.Count == 1)
        //    {
        //        try
        //        {
        //            string relativePath = "~/UploadFilledUpFormForApproval/";
        //            string path = Server.MapPath(relativePath);
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            //  Get all files from Request object  
        //            HttpFileCollectionBase files = Request.Files;

        //            //---For Single form
        //            Forms form = new Forms();

        //            HttpPostedFileBase file = files[0];
        //            string fname;

        //            // Checking for Internet Explorer  
        //            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //            {
        //                string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                fname = testfiles[testfiles.Length - 1];
        //            }
        //            else
        //            {
        //                fname = file.FileName;
        //            }
        //            string uniqueFormName = GetUniqueFileNameWithUserId(fname);
        //            // Get the complete folder path and store the file inside it. 

        //            fname = Path.GetFileNameWithoutExtension(fname);
        //            string fnameWithPath = Path.Combine(path, uniqueFormName);
        //            //file.SaveAs(fnameWithPath);

        //            form.FormName = fname;
        //            form.FilledUpFormName = uniqueFormName;
        //            form.FilePath = relativePath;
        //            //form.ShipId             = Convert.ToInt32(shipId);
        //            form.ShipId = Convert.ToInt32(Session["ShipId"].ToString());
        //            //form.Approvers          = approvers;
        //            //form.CreateedBy = 1;//--- userId
        //            form.CreateedBy = Convert.ToInt32(Session["UserId"].ToString());//--- userId
        //            //---End---For Single form
        //            int count = documentBL.SaveFilledUpForm(form, ref catchMessage);
                    

        //            // Returns message that successfully uploaded  
        //            return Json("File Uploaded Successfully!");
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("Error occurred. Error details: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Json("No files selected.");
        //    }
        //}

        /// <summary>
        /// UploadFilledUpForm is New because form approval logics has changed again
        /// in this method user select approvers and ther task 
        /// (for Company user only who has to right to approve)
        /// created on 23rd Jul 2021
        /// </summary>
        /// <param name="task"></param>
        /// <param name="approvers"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFilledUpForm(string task, object approvers)
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            string[] result = Array.ConvertAll<object, string>((object[])approvers, x => x.ToString());

            List<ApproverMaster> approverList = new List<ApproverMaster>();
            //JObject json = JObject.Parse(result[0].Replace("[","").Replace("]","").ToString());
            string orderedApprovers = "";
            int approversCount = 0;
            approverList = JsonConvert.DeserializeObject<List<ApproverMaster>>(result[0]);
            approversCount = approverList.Count();
            int cnt = 0;
            foreach (ApproverMaster a in approverList.OrderBy(a => a.SL))
            {
                cnt = cnt + 1;
                if (cnt == 1)
                    orderedApprovers = a.ID.ToString();
                else if(cnt > 1)
                    orderedApprovers = orderedApprovers+"," + a.ID.ToString();

                
            }
            orderedApprovers = orderedApprovers + ",";

            //-----------------------------------------------
            bool isSendSuccessfully = false;
            //-----------------------------------------------

            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval/";
                    string path = Server.MapPath(relativePath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        //fname = file.FileName;
                        fname = file.FileName;
                    }
                    string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 
                    var ext = Path.GetExtension(fname).ToLowerInvariant();
                    fname = Path.GetFileNameWithoutExtension(fname);
                    
                    string fnameWithPath = Path.Combine(path, uniqueFormName);
                    file.SaveAs(fnameWithPath);// file save into the root directory

                    
                    form.FormName           = fname;
                    form.FilledUpFormName   = uniqueFormName;
                    form.FilePath           = relativePath;
                    //form.ShipId             = Convert.ToInt32(shipId);
                    form.ShipId             = Convert.ToInt32(Session["ShipId"].ToString());
                    form.Approvers          = orderedApprovers;//added on 24th Jul 2021 @BK
                    form.Task               = task;
                    //form.CreateedBy = 1;//--- userId
                    form.CreateedBy         = Convert.ToInt32(Session["UserId"].ToString());//--- userId
                    //---End---For Single form
                    //int count = documentBL.SaveFilledUpForm(form, ref catchMessage);
                    int count = documentBL.SaveFilledUpFormsForCompanyApproval(form, ref catchMessage);
                    int y = 0;
                    if (count > 0 && task == "A" && (ext == ".doc" || ext == ".docx"))
                    {
                        y = AppendSignatureTable(path, uniqueFormName, approversCount);
                    }
                    if (y == 1)
                    {
                        WriteErrorToText("Sign added in form", "ApproveFilledUpForm");
                        if (System.IO.File.Exists(Path.Combine(path + "Temp\\", uniqueFormName)))
                        {
                            System.IO.File.Copy(Path.Combine(path + "Temp\\", uniqueFormName), Path.Combine(path, uniqueFormName), true);

                            System.IO.File.Delete(Path.Combine(path + "Temp\\", uniqueFormName));
                        }

                        string[] approverUsers = orderedApprovers.Split(',');
                        foreach(string s in approverUsers)
                        {
                            if(s !="")
                                isSendSuccessfully = SendMailToApprover(s, uniqueFormName, task);
                        }

                        
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        /// <summary>
        /// UploadFilledUpForm is New because form approval logics has changed again
        /// created on 19th  Aug 2021
        /// Forms will be uploaded as PDF when form is for Approval
        /// </summary>
        /// <param name="task"></param>
        /// <param name="approvers"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFilledUpFormNew(string task, object approvers)
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            string[] result = Array.ConvertAll<object, string>((object[])approvers, x => x.ToString());

            List<ApproverMaster> approverList = new List<ApproverMaster>();
            //JObject json = JObject.Parse(result[0].Replace("[","").Replace("]","").ToString());
            string orderedApprovers = "";
            int approversCount = 0;
            approverList = JsonConvert.DeserializeObject<List<ApproverMaster>>(result[0]);
            approversCount = approverList.Count();
            int cnt = 0;
            foreach (ApproverMaster a in approverList.OrderBy(a => a.SL))
            {
                cnt = cnt + 1;
                if (cnt == 1)
                    orderedApprovers = a.ID.ToString();
                else if (cnt > 1)
                    orderedApprovers = orderedApprovers + "," + a.ID.ToString();


            }
            orderedApprovers = orderedApprovers + ",";

            //-----------------------------------------------
            bool isSendSuccessfully = false;
            //-----------------------------------------------

            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval/";
                    string path = Server.MapPath(relativePath);
                    

                    //string tmpPath = path + "Temp\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        //fname = file.FileName;
                        fname = file.FileName;
                    }
                    string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 
                    var ext = Path.GetExtension(fname).ToLowerInvariant();
                    fname = Path.GetFileNameWithoutExtension(fname);
                   // var uniquePdfname = Path.GetFileNameWithoutExtension(fname) + ".pdf";
                    //string fnameWithPath = Path.Combine(path, uniqueFormName);
                    string fnameWithPath = Path.Combine(path, uniqueFormName);
                    file.SaveAs(fnameWithPath);// file save into the temp  directory

                    int y = 0;
                    if(task == "A")
                    {
                        
                        y = ConvertToPDFForApproval(path, uniqueFormName, task);
                        //uniqueFormName = Path.GetFileNameWithoutExtension(uniqueFormName) + ".pdf";
                    }
                   
                    

                    form.FormName = fname;
                    form.FilledUpFormName = Path.GetFileNameWithoutExtension(uniqueFormName) + ".pdf"; ;
                    //form.FilledUpFormName = uniquePdfname;
                    form.FilePath = relativePath;
                    //form.ShipId             = Convert.ToInt32(shipId);
                    form.ShipId = Convert.ToInt32(Session["ShipId"].ToString());
                    form.Approvers = orderedApprovers;//added on 24th Jul 2021 @BK
                    form.Task = task;
                    //form.CreateedBy = 1;//--- userId
                    form.CreateedBy = Convert.ToInt32(Session["UserId"].ToString());//--- userId
                    //---End---For Single form
                    //int count = documentBL.SaveFilledUpForm(form, ref catchMessage);
                    int count = documentBL.SaveFilledUpFormsForCompanyApproval(form, ref catchMessage);
                    
                    if (y == 1)
                    {
                        WriteErrorToText("Sign added in form", "ApproveFilledUpForm");
                        if (System.IO.File.Exists(Path.Combine(path , uniqueFormName)))
                        {
                            //System.IO.File.Copy(Path.Combine(path + "Temp\\", uniqueFormName), Path.Combine(path, uniqueFormName), true);

                            System.IO.File.Delete(Path.Combine(path , uniqueFormName));
                        }

                    }
                    string[] approverUsers = orderedApprovers.Split(',');
                    foreach (string s in approverUsers)
                    {

                        isSendSuccessfully = SendMailToApprover(s, uniqueFormName, task);
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// Added on 19th aug 2021
        /// Due to approvallogic has changed.
        /// Approver add their sign in PDF form and the upload the same form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadApprovedForm()
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();

            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval/";
                    string path = Server.MapPath(relativePath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    //string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 

                    //fname = Path.GetFileNameWithoutExtension(fname);
                    string fnameWithPath = Path.Combine(path+"Temp\\", fname);
                    file.SaveAs(fnameWithPath);
                    int y = 0;
                    if (System.IO.File.Exists(Path.Combine(path + "Temp\\", fname)))
                    {
                        System.IO.File.Copy(Path.Combine(path + "Temp\\", fname), Path.Combine(path, fname), true);

                        System.IO.File.Delete(Path.Combine(path + "Temp\\", fname));
                        y = 1;
                    }
                    string originalFrmName;
                    originalFrmName         = fname.Split('_').First();

                    if(y == 1)
                    {
                        form.FormName = originalFrmName;
                        form.FilledUpFormName = fname;
                        form.FilePath = relativePath;
                        //form.ShipId             = Convert.ToInt32(shipId);
                        form.ShipId = Convert.ToInt32(Session["ShipId"].ToString());
                        //form.Approvers          = approvers;
                        //form.CreateedBy = 1;//--- userId
                        form.CreateedBy = Convert.ToInt32(Session["UserId"].ToString());//--- userId
                                                                                        //---End---For Single form
                        int count = documentBL.SaveFilledUpForm(form, ref catchMessage);

                        // Returns message that successfully uploaded  
                        return Json("File Uploaded Successfully!");

                    }
                    else
                    {
                        // Returns message that successfully uploaded  
                        return Json("File could not Uploaded ..!");
                    }


                    
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }


        [HttpPost]
        public JsonResult UploadFilledUpReviewedForm(string task, object approvers)
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            

            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval";
                    string path = Server.MapPath(relativePath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    //string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 

                    string fnameWithPath = Path.Combine(path + "\\Temp\\", fname);
                    
                    //fname = Path.GetFileNameWithoutExtension(fname);
                    file.SaveAs(fnameWithPath);

                    //---End---For Single form
                    int x = documentBL.ReviewedFilledUpForm(Convert.ToInt32(Session["UserId"].ToString()), fname);
                    int y = 0;
                    if (x > 0 && task == "R")
                    {
                        y = AddApproverOrReviewerSignatureInReviewedForm(task, path, fname, Convert.ToInt32(Session["UserId"].ToString()));
                    }
                    if (x > 0 && task == "R")
                    {
                        WriteErrorToText("Reviewed form copied from Temp", "ReviewedFilledUpForm");
                        if (System.IO.File.Exists(Path.Combine(path + "\\Temp\\", fname)))
                        {
                            System.IO.File.Copy(Path.Combine(path + "\\Temp\\", fname), Path.Combine(path+"\\", fname), true);

                            System.IO.File.Delete(Path.Combine(path + "\\Temp\\", fname));
                        }
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        public JsonResult UploadApprovedFilledUpForm(string formId, string approverId, string formName, object cat)
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();


            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval";
                    string path = Server.MapPath(relativePath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    //string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 

                    string fnameWithPath = Path.Combine(path + "\\Temp\\", fname);

                    //fname = Path.GetFileNameWithoutExtension(fname);
                    file.SaveAs(fnameWithPath);

                    //---End---For Single form
                   // int x = documentBL.ReviewedFilledUpForm(Convert.ToInt32(Session["UserId"].ToString()), fname);
                    int x = documentBL.ApproveFilledUpForm(Convert.ToInt32( formId), Convert.ToInt32(Session["UserId"].ToString()), formName);
                    int y = 0;
                    
                    if (x > 0 )
                    {
                        WriteErrorToText("Reviewed form copied from Temp", "ReviewedFilledUpForm");
                        if (System.IO.File.Exists(Path.Combine(path + "\\Temp\\", fname)))
                        {
                            System.IO.File.Copy(Path.Combine(path + "\\Temp\\", fname), Path.Combine(path + "\\", fname), true);

                            System.IO.File.Delete(Path.Combine(path + "\\Temp\\", fname));
                        }
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// Added on 19th aug 2021
        /// Due to approvallogic has changed.
        /// Approver add their sign in PDF form and the upload the same form
        /// </summary>
        /// <param name="task"></param>
        /// <param name="approvers"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFilledUpReviewedFormNew(string task, object approvers)
        {
            string catchMessage = "";
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();


            if (Request.Files.Count == 1)
            {
                try
                {
                    string relativePath = "~/UploadFilledUpFormForApproval";
                    string path = Server.MapPath(relativePath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //---For Single form
                    Forms form = new Forms();

                    HttpPostedFileBase file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    //string uniqueFormName = GetUniqueFileNameWithUserId(fname);
                    // Get the complete folder path and store the file inside it. 

                    string fnameWithPath = Path.Combine(path + "\\Temp\\", fname);

                    //fname = Path.GetFileNameWithoutExtension(fname);
                    file.SaveAs(fnameWithPath);

                    //---End---For Single form
                    int x = documentBL.ReviewedFilledUpForm(Convert.ToInt32(Session["UserId"].ToString()), fname);
                    int y = 0;
                    
                    if (x > 0 && task == "R")
                    {
                        WriteErrorToText("Reviewed form copied from Temp", "ReviewedFilledUpForm");
                        if (System.IO.File.Exists(Path.Combine(path + "\\Temp\\", fname)))
                        {
                            System.IO.File.Copy(Path.Combine(path + "\\Temp\\", fname), Path.Combine(path + "\\", fname), true);

                            System.IO.File.Delete(Path.Combine(path + "\\Temp\\", fname));
                        }
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }


        /// <summary>
        /// not used. may use in later
        /// </summary>
        /// <param name="relPath"></param>
        /// <param name="uploadedFormName"></param>
        /// <param name="approverUserId"></param>
        /// <returns></returns>
        public static int AddReviewerSignatureInReviewedForm(string relPath, string uploadedFormName, int approverUserId)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";

            int x = 0;

            try
            {
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                //ComponentInfo.SetLicense("DN-2021Jan04-gSb72AQqrg9T4PQnvYNDgVtyd4tD3W3oBds51kfYp7zSsuFpxRw1a5Cxr49JiCLbMf2JCIKuinkUhgiQmuOz5yMoWdA==A");

                string tempPath = relPath + "\\Temp\\";
                string docPath = tempPath + uploadedFormName;
                string sdocPath = relPath + "\\" + uploadedFormName;

                string root = Path.GetDirectoryName(relPath);

                ApproverMaster approver = new ApproverMaster();
                ApproverSignBL aSignBL  = new ApproverSignBL();
                approver                = aSignBL.GetAllApproverSign(approverUserId, uploadedFormName);

                string decryptSignPath = AesOperation.DecryptString(key, approver.SignImagePath.ToString());

                string signPath         = Path.Combine(root + "\\", decryptSignPath.Replace("/", "\\"));
                string approverName     = approver.Name;
                string designation      = approver.Position;
                int approverPossition   = approver.ApprovedCount;

                //int numberOfItems       = 4;// Number of rows of the signature tablee
                int numberOfItems       = approver.ApproversCount;

                // Load an Excel template.
                var workbook = ExcelFile.Load(sdocPath);

                // Get template sheet.
                var worksheet = workbook.Worksheets[0];

                // Find cells with placeholder text and set their values.
                int row, column;
                
                if (approverPossition == 1 && worksheet.Cells.FindText("SM/FM", out row, out column))
                {
                    
                    worksheet.Cells[row, column + 3].Value = approverName;
                    worksheet.Pictures.Add(signPath,
                                             new AnchorCell(worksheet.Columns[column + 8], worksheet.Rows[row], 100000, 100000),
                                             new AnchorCell(worksheet.Columns[column + 12], worksheet.Rows[row + 1], 50000, 50000)
                                          ).Position.Mode = PositioningMode.Move;
                }
                else if (approverPossition == 2 && worksheet.Cells.FindText("By D/GM", out row, out column))
                {

                    worksheet.Cells[row, column + 3].Value = approverName;
                    worksheet.Pictures.Add(signPath,
                                             new AnchorCell(worksheet.Columns[column + 8], worksheet.Rows[row], 100000, 100000),
                                             new AnchorCell(worksheet.Columns[column + 12], worksheet.Rows[row + 1], 50000, 50000)
                                          ).Position.Mode = PositioningMode.Move;
                }
                workbook.Save(docPath);
                x = 1;
            }
            catch (Exception ex)
            {
                x = 0;
            }


            return x;
        }

        public static int AddApproverOrReviewerSignatureInReviewedForm(string task,string relPath, string uploadedFormName, int approverUserId)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";

            int x = 0;

            try
            {
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                //ComponentInfo.SetLicense("DN-2021Jan04-gSb72AQqrg9T4PQnvYNDgVtyd4tD3W3oBds51kfYp7zSsuFpxRw1a5Cxr49JiCLbMf2JCIKuinkUhgiQmuOz5yMoWdA==A");

                string tempPath = relPath + "\\Temp\\";
                string docPath = tempPath + uploadedFormName;
                string sdocPath = relPath + "\\" + uploadedFormName;

                string root = Path.GetDirectoryName(relPath);

                ApproverMaster approver = new ApproverMaster();
                ApproverSignBL aSignBL = new ApproverSignBL();
                approver = aSignBL.GetAllApproverSign(approverUserId, uploadedFormName);

                string decryptSignPath = AesOperation.DecryptString(key, approver.SignImagePath.ToString());

                string signPath = Path.Combine(root + "\\", decryptSignPath.Replace("/", "\\"));
                string approverName = approver.Name;
                string designation = approver.Position;
                int approverPossition = approver.ApprovedCount;

                //int numberOfItems       = 4;// Number of rows of the signature tablee
                int numberOfItems = approver.ApproversCount;

                // Load an Excel template.
                var workbook = ExcelFile.Load(sdocPath);

                // Get template sheet.
                var worksheet = workbook.Worksheets[0];

                // Find cells with placeholder text and set their values.
                int row, column;
                if(task == "R")
                {
                    if (approverPossition == 1 && worksheet.Cells.FindText("SM/FM", out row, out column))
                    {

                        worksheet.Cells[row, column + 3].Value = approverName;
                        worksheet.Pictures.Add(signPath,
                                                 new AnchorCell(worksheet.Columns[column + 8], worksheet.Rows[row], 100000, 100000),
                                                 new AnchorCell(worksheet.Columns[column + 12], worksheet.Rows[row + 1], 50000, 50000)
                                              ).Position.Mode = PositioningMode.Move;
                    }
                    else if (approverPossition == 2 && worksheet.Cells.FindText("By D/GM", out row, out column))
                    {

                        worksheet.Cells[row, column + 3].Value = approverName;
                        worksheet.Pictures.Add(signPath,
                                                 new AnchorCell(worksheet.Columns[column + 8], worksheet.Rows[row], 100000, 100000),
                                                 new AnchorCell(worksheet.Columns[column + 12], worksheet.Rows[row + 1], 50000, 50000)
                                              ).Position.Mode = PositioningMode.Move;
                    }
                }
                else if (task == "A")
                {
                    if (worksheet.Cells.FindText("By GM", out row, out column))
                    {
                        DateTime dt = DateTime.Now;
                        worksheet.Cells[row, column + 4].Value = approverName + "/ " +dt.ToString("dd/MM/yyyy");
                        worksheet.Pictures.Add(signPath,
                                                 new AnchorCell(worksheet.Columns[column + 8], worksheet.Rows[row], 100000, 100000),
                                                 new AnchorCell(worksheet.Columns[column + 12], worksheet.Rows[row + 1], 50000, 50000)
                                              ).Position.Mode = PositioningMode.Move;
                    }
                }

                workbook.Save(docPath);
                x = 1;
            }
            catch (Exception ex)
            {
                x = 0;
            }


            return x;
        }

        [HttpPost]
        public JsonResult UploadFilledUpReviewedFormForApproval(string formName, object approvers)
        {
            try
            {
                string catchMessage = "";
                string relativePath = "~/UploadFilledUpFormForApproval/";
                string fname;
                fname = formName.Split('_').First();
                Forms form = new Forms();
                DocumentBL documentBL = new DocumentBL();
               // string[] result = Array.ConvertAll<object, string>((object[])approvers, x => x.ToString());
                //string[] result = ((IEnumerable)approvers).Cast<object>()
                //                 .Select(x => x.ToString())
                //                 .ToArray();
                List<ApproverMaster> approverList = new List<ApproverMaster>();
                //JObject json = JObject.Parse(result[0].Replace("[","").Replace("]","").ToString());
                string orderedApprovers = "";
                int approversCount = 0;
                //approverList = JsonConvert.DeserializeObject<List<ApproverMaster>>(result[0]);
                approverList = JsonConvert.DeserializeObject<List<ApproverMaster>>(approvers.ToString());
                approversCount = approverList.Count();
                int cnt = 0;
                foreach (ApproverMaster a in approverList.OrderBy(a => a.SL))
                {
                    cnt = cnt + 1;
                    if (cnt == 1)
                        orderedApprovers = a.ID.ToString();
                    else if (cnt > 1)
                        orderedApprovers = orderedApprovers + "," + a.ID.ToString();


                }
                orderedApprovers = orderedApprovers + ",";

                form.FormName           = fname;
                form.FilledUpFormName   = formName;
                form.FilePath           = relativePath;
                //form.ShipId             = Convert.ToInt32(shipId);
                form.ShipId             = Convert.ToInt32(Session["ShipId"].ToString());
                form.Approvers          = orderedApprovers;//added on 24th Jul 2021 @BK
                form.Task               = "A";
                //form.CreateedBy           = 1;//--- userId
                form.CreateedBy         = Convert.ToInt32(Session["UserId"].ToString());//--- userId

                int count = documentBL.SaveFilledUpFormsForCompanyApproval(form, ref catchMessage);

                // Returns message that successfully uploaded  
                return Json("File Uploaded Successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        /// <summary>
        /// UploadFilledUpReviewedFormForApprovalNew is New because form approval logics has changed again
        /// created on 19th  Aug 2021
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="approvers"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFilledUpReviewedFormForApprovalNew(string formName, object approvers)
        {
            try
            {
                string catchMessage         = "";
                string relativePath         = "~/UploadFilledUpFormForApproval/";
                string fname;
                fname                       = formName.Split('_').First();
                Forms form                  = new Forms();
                DocumentBL documentBL       = new DocumentBL();
                List<ApproverMaster> approverList = new List<ApproverMaster>();
                string orderedApprovers     = "";
                int approversCount          = 0;
                approverList                = JsonConvert.DeserializeObject<List<ApproverMaster>>(approvers.ToString());
                approversCount              = approverList.Count();
                int cnt                     = 0;
                foreach (ApproverMaster a in approverList.OrderBy(a => a.SL))
                {
                    cnt = cnt + 1;
                    if (cnt == 1)
                        orderedApprovers = a.ID.ToString();
                    else if (cnt > 1)
                        orderedApprovers = orderedApprovers + "," + a.ID.ToString();


                }
                orderedApprovers        = orderedApprovers + ",";
                int y                   = ConvertToPDFForApproval(relativePath, formName,"A");
                form.FormName           = fname;
                form.FilledUpFormName   = formName;
                form.FilePath           = relativePath;
                //form.ShipId             = Convert.ToInt32(shipId);
                form.ShipId             = Convert.ToInt32(Session["ShipId"].ToString());
                form.Approvers          = orderedApprovers;//added on 24th Jul 2021 @BK
                form.Task               = "A";
                //form.CreateedBy           = 1;//--- userId
                form.CreateedBy         = Convert.ToInt32(Session["UserId"].ToString());//--- userId

                int count               = documentBL.SaveFilledUpFormsForCompanyApproval(form, ref catchMessage);

                // Returns message that successfully uploaded  
                return Json("File Uploaded Successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }


        public JsonResult UploadFilledUpReviewedFormFor(string formName)
        {
            return Json("File Uploaded Successfully!");
        }
        #region Utility Methods
        private string GetUniqueFileName(string fileName)
        {//Added on 8th jan 2021 @bk
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        private string GetUniqueFileNameWithUserId(string fileName)
        {
            //string userId = "1";

            string userId = Session["UserId"].ToString();
            string shipId = Session["ShipId"].ToString();
            var n = DateTime.Now;
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + shipId
                      + "-"
                      + userId
                      + "_"
                      + string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", n.Year - 2000, n.Month, n.Day, n.Hour, n.Minute, n.Second)
                      + Path.GetExtension(fileName);
            //----test
            //fileName = Path.GetFileNameWithoutExtension(fileName);
            //string s = string.Format(fileName + "_" + userId + "_{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", n.Year - 2000, n.Month, n.Day, n.Hour, n.Minute, n.Second);
            //return s+ Path.GetExtension(fileName);
        }
        #endregion


        public bool SendMailToApprover(string approverUserId, string formName, string task)
        {
            bool isSendSuccessfully = false;
            UserMasterBL umBL = new UserMasterBL();

            try
            {
                UserMasterPOCO um = new UserMasterPOCO();
                StringBuilder mailBody = new StringBuilder();
                string senderEmail = string.Empty;
                string receiverEmail = string.Empty;
                string lTask = string.Empty;
                string lSubject = string.Empty;
                if (task == "R")
                {
                    lTask = "Review";
                    lSubject = "FormsReview";
                }
                else if (task == "A")
                {
                    lTask = "Approve";
                    lSubject = "FormsApproval";
                }

                if (Session["UserType"].ToString() == "1")
                {
                    senderEmail = ConfigurationManager.AppSettings["shipEmail"];
                }
                else
                {
                    senderEmail = Session["Email"].ToString();
                }

                um = umBL.GetUserByUserId(Convert.ToInt32(approverUserId));
                receiverEmail = um.Email.ToString();

                MailMessage mail = new MailMessage();

                mailBody.Append("Form Name : ");
                mailBody.Append(formName.ToString());
                mailBody.Append("\n");
                mailBody.Append("\n");
                mailBody.Append("Messege : ");
                mailBody.Append("You are requested to " + lTask + " the above Form. Please Login and " + lTask + " the Form.");
                mailBody.Append("\n");
                mail.Body = mailBody.ToString();

                if (!String.IsNullOrEmpty(senderEmail) && !String.IsNullOrEmpty(receiverEmail))
                {

                    SendEmail.SendMail(lSubject, senderEmail, receiverEmail, mail, ref isSendSuccessfully);
                }

                
            }
            catch (Exception ex)
            {
                
            }

            return isSendSuccessfully;
            
        }

    }

    
}