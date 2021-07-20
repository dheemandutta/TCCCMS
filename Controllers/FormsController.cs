using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


using GemBox.Document;
using GemBox.Document.Tables;

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
                pOCO.FormName   = pC.FormName;
                pOCO.Path       = pC.Path;
                pOCO.Version    = pC.Version;
                pOCO.IsUpload   = pC.IsUpload;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FormsApprovalList()
        {
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            formList = documentBL.GetFilledupFormRequiredApprovalList(Convert.ToInt32(Session["UserId"].ToString()));
            return View(formList);
        }

        public JsonResult ApproveFilledUpForm(Forms form)
        {
            string path = Server.MapPath("~/UploadFilledUpFormForApproval");
            string uploadedForm = form.FilledUpFormName;
            DocumentBL documentBl = new DocumentBL();
            int filledUpFormId = form.ID;
            int x = documentBl.ApproveFilledUpForm(filledUpFormId, Convert.ToInt32(Session["UserId"].ToString()), uploadedForm);
            int y = 0;

            if(x == 1)
            {
                y= AddSignatureInForm(path, uploadedForm, Convert.ToInt32(Session["UserId"].ToString()));
            }

            if (y == 1)
            {
                if(System.IO.File.Exists(Path.Combine(path+"\\Temp\\", uploadedForm)))
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
        /// <returns></returns>
        public ActionResult FillupFormList(string qr = "2")
        {
            ApprovedFilledupFormAndApproverViewModel affaVM = new ApprovedFilledupFormAndApproverViewModel();
            DocumentBL documentBL = new DocumentBL();
            affaVM = documentBL.GetApprovedFilledUpForms(Convert.ToInt32(Session["UserId"].ToString())); // UserId not in use from 3rd Jul 2021
            
            if(Convert.ToInt32(qr) != 2)// added on 03/07/2021 @BK
            {
                var res = affaVM.ApprovedFormList.Where(af => af.IsApproved == Convert.ToInt32(qr)).ToList();
                affaVM.ApprovedFormList = res;
            }
            //else if(Convert.ToInt32(qr) == 1)
            //{
            //    var res = affaVM.ApprovedFormList.Where(af => af.IsApproved = 1);
            //}
            
            return View(affaVM);
        }

        public JsonResult SendMailForapproval(string approvalId,string approverUserId)
        {

            return Json("", JsonRequestBehavior.AllowGet);
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

                string signPath         = Path.Combine(root+"\\", approver.SignImagePath.Replace("/","\\"));
                string approverName     = approver.Name;
                string designation      = approver.Position;
                int approverPossition   = approver.ApprovedCount;

                int numberOfItems       = 4;// 

                DocumentModel document  = DocumentModel.Load(sdocPath);

                // Template document contains 4 tables, each contains some set of information.
                Table[] tables          = document.GetChildElements(true, ElementType.Table).Cast<Table>().ToArray();

                int tableCount          = tables.Count();

                Table signatureTable    = tables[tableCount - 2];

                for (int rowIndex = 0; rowIndex <= numberOfItems; rowIndex++)
                {
                    //DateTime date = DateTime.Today.AddDays(rowIndex - numberOfItems);
                    //int hours = rowIndex % 3 + 6;
                    //int unit = 35;
                    //int price = hours * unit;
                    if (rowIndex == approverPossition)
                    {
                        var paragraph = new Paragraph(document);

                        Picture picture1 = new Picture(document, signPath, 100, 35, LengthUnit.Pixel);
                        paragraph.Inlines.Add(picture1);

                        signatureTable.Rows[rowIndex].Cells[1].Blocks.Add(new Paragraph(document,
                                                                              new Run(document, "Name :" + approverName),
                                                                              new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                                                                              new Run(document, "Designation : " + designation)
                                                                          ));
                        signatureTable.Rows[rowIndex].Cells[2].Blocks.Add(paragraph);
                    }
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
    }
}