using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class CompanyResponsibilityAndAuthorityManualController : Controller
    {
        //--------------------Vol3-------------------------

        private string controllerName = "CompanyResponsibilityAndAuthorityManual";
        ManualBL manualBL = new ManualBL();
        // GET: CompanyResponsibilityAndAuthorityManual
        
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 3);

            return View(file);
        }

        //public ActionResult Pages(string actionName)
        //{
        //    System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, actionName);
        //    TempData[actionName] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult Pages(string actionName, string formName = "", string relformPath = "")
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            Manual file = new Manual();
            if(formName == "")
            {
                file = manualBL.GetManual(controllerName, actionName);
            } 
            else
            {
                string filePath = "../ManualsPDF/" + relformPath + "/";
                filePath = filePath + formName + ".pdf#toolbar=0";
                file.PdfName = formName;
                file.PdfPath = filePath;


                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.ManualBodyHtml);

                sb.Append("</div>");
                sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
                sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
                sb.Append("</div>");
                //-------------------------------------------------------------------------
                StringBuilder sb2 = new StringBuilder("<div class='row'>");
                sb2.Append("<div class='col-sm-12 col-xs-12 marginTP10'>");
                sb2.Append("<div class='card'>");
                sb2.Append("<div class='col-sm-12 col-xs-12'>");
                sb2.Append("<div class='row'>");
                //------
                sb2.Append("<p class='marginTP10'>");
                sb2.Append("<div class='col-sm-10 col-xs-12'>");
                sb2.Append("<label>"+ formName + "</label>");
                sb2.Append("</div>");

                sb2.Append("<div class='col-sm-2 col-xs-12'>");
                sb2.Append("<button type='button' class='btn btn-info btn-sm' style='background-color: #e90000;' data-toggle='modal' data-target='#formPreviewModal' >Preview</button>");
                //sb2.Append("</div>");

                //sb2.Append("<div class='col-sm-2 col-xs-12'>");
                sb2.Append("<a href='/" + controllerName + "/Download?fileName=");
                sb2.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a>");
                
                sb2.Append("</div>");
                sb2.Append("</p>");
                //-------
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");
                sb2.Append("\n");
                sb2.Append("</div>");


                //file.ManualBodyHtml = sb.ToString();
                file.ManualBodyHtml = sb2.ToString();
            }

            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume III/";
            string filePath = "../ManualsPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        public ActionResult FormPreview(string fileName, string relPDFPath)
        {
            Manual file = new Manual(); string filePath = "../ManualsPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf#toolbar=0";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return PartialView("_pvFormPreviewModal", file);
        }

        public FileResult Download(string fileName, string relformPath)
        {
            string path = Server.MapPath("~/ManualsPDF/"+ relformPath+"/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            var filePath = Directory.GetFiles(path, "*.doc?")
                                                        .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")).First();
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(filePath));
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                //{".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #region Other Action Not Used
        [HttpGet]
        public ActionResult Manual()
        {
            //---------------Vol. III Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SOOJD()
        {
            //---------------Second Officer Job Description - TO BE DELETED-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SOOJD");
            TempData["SOOJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ShoreOrganisation()
        {
            //---------------Section 1 - Shore Organisation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ShoreOrganisation");
            TempData["ShoreOrganisation"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SOC()
        {
            //---------------Section 1 Annex 1 - Shore Organisation Chart-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SOC");
            TempData["SOC"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SPR()
        {
            //---------------Section 2 - Shore Personnel Recruitment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SPR");
            TempData["SPR"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult DesignatedPerson()
        {
            //---------------Section 3 - Designated Person-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DesignatedPerson");
            TempData["DesignatedPerson"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ShipboardOrganisation()
        {
            //---------------Section 4 - Shipboard Organisation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ShipboardOrganisation");
            TempData["ShipboardOrganisation"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult Appraisal()
        {
            //---------------Section 5 - Appraisal-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Appraisal");
            TempData["Appraisal"] = file.ManualBodyHtml;
            return View(file);
        }
        
        [HttpGet]
        public ActionResult Promotion()
        {
            //---------------Section 6 - Promotion-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Promotion");
            TempData["Promotion"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SOOC()
        {
            //---------------Section 7 - Signing On _ Off crew-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SOOC");
            TempData["SOOC"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult HandOver()
        {
            //---------------Section 8 - Hand Over-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HandOver");
            TempData["HandOver"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SMC()
        {
            //---------------Section 9 - Safe Manning _ Certification-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SMC");
            TempData["SMC"] = file.ManualBodyHtml;
            return View(file);
            

        }
        [HttpGet]
        public ActionResult WRH()
        {
            //---------------Section 10 - Work and Rest hour-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WRH");
            TempData["WRH"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult WSR()
        {
            //---------------Section 11 - Working Schedule and Roster-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WSR");
            TempData["WSR"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ODG()
        {
            //---------------Section 12 - Onboard Discipline and Grievance-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ODG");
            TempData["ODG"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult CDMP()
        {
            //---------------Section 13 - Crew Desertion _ Missing Person-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CDMP");
            TempData["CDMP"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult DrugAndAlcohol()
        {
            //---------------Section 14 - Drug and Alcohol-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DrugAndAlcohol");
            TempData["DrugAndAlcohol"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ShipboardMedical()
        {
            //---------------Section 15 - Shipboard Medical-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ShipboardMedical");
            TempData["ShipboardMedical"] = file.ManualBodyHtml;
            return View(file);
        }

        [HttpGet]
        public ActionResult DeathOnboard()
        {
            //---------------Section 16 - Death Onboard-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DeathOnboard");
            TempData["DeathOnboard"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult PBS()
        {
            //---------------Section 17 - Provision and Bond Store-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PBS");
            TempData["PBS"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult DressCode()
        {
            //---------------Section 18  - Dress Code-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DressCode");
            TempData["DressCode"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult CrewWelfare()
        {
            //---------------Section 19 - Crew Welfare-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CrewWelfare");
            TempData["CrewWelfare"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult FamilyOnboard()
        {
            //---------------Section 21 - Family Onboard----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FamilyOnboard");
            TempData["FamilyOnboard"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult CGM()
        {
            //---------------Section 22 - Crew General Matters-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CGM");
            TempData["CGM"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult VesselAccount()
        {
            //---------------Section 23 - Vessel Account-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "VesselAccount");
            TempData["VesselAccount"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult POM()
        {
            //---------------Section 24 - Pornography and Obscene Material-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "POM");
            TempData["POM"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult MLC()
        {
            //---------------Section 25 - MLC-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MLC");
            TempData["MLC"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult MLCCT()
        {
            //---------------Section 26 - MLC Compliance _ Training-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MLCCT");
            TempData["MLCCT"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult MedicalExamination()
        {
            //---------------Section 27 - Medical Examination-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MedicalExamination");
            TempData["MedicalExamination"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult RecruitmentOfSeafarer()
        {
            //---------------Section 28 - Recruitment of Seafarer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RecruitmentOfSeafarer");
            TempData["RecruitmentOfSeafarer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult TRM()
        {
            //---------------Section 29 - Travel Risk Management-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TRM");
            TempData["TRM"] = file.ManualBodyHtml;
            return View(file);
        }
        

        //-----31 AC

        #region Appendix 1 - Promotion Guidelines
        ////------------------------Appendix 1 - Promotion Guidelines-----------------------
        [HttpGet]
        public ActionResult P2ETO()
        {
            //---------------Promotion to  Electro-Technical Officer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ETO");
            TempData["P2ETO"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2SecondEngineer()
        {
            //---------------Promotion to 2nd Engineer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2SecondEngineer");
            TempData["P2SecondEngineer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2SecondOfficer()
        {
            //---------------Promotion to 2nd Officer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2SecondOfficer");
            TempData["P2SecondOfficer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2ThirdEngineer()
        {
            //---------------Promotion to 3rd Engineer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ThirdEngineer");
            TempData["P2ThirdEngineer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2ThirdOfficer()
        {
            //---------------Promotion to 3rd Officer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ThirdOfficer");
            TempData["P2ThirdOfficer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2FourthEngineer()
        {
            //---------------Promotion to 4th Engineer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2FourthEngineer");
            TempData["P2FourthEngineer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2AB()
        {
            //---------------Promotion to AB-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2AB");
            TempData["P2AB"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2Bosun()
        {
            //---------------Promotion to Bosun-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2Bosun");
            TempData["P2Bosun"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2ChiefCook()
        {
            //---------------Promotion to Chief Cook-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ChiefCook");
            TempData["P2ChiefCook"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2ChiefEngineer()
        {
            //---------------Promotion to Chief Engineer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ChiefEngineer");
            TempData["P2ChiefEngineer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2ChiefOfficer()
        {
            //---------------Promotion to Chief Officer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2ChiefOfficer");
            TempData["P2ChiefOfficer"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2Fitter()
        {
            //---------------Promotion to Fitter-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2Fitter");
            TempData["P2Fitter"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2Master()
        {
            //---------------Promotion to Master-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2Master");
            TempData["P2Master"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2Oiler()
        {
            //---------------Promotion to Oiler-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2Oiler");
            TempData["P2Oiler"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult P2Pumpman()
        {
            //---------------Promotion to Pumpman-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "P2Pumpman");
            TempData["P2Pumpman"] = file.ManualBodyHtml;
            return View(file);
        }


        ////------------End------------Appendix 1 - Promotion Guidelines-----------------------
        #endregion

        #region  Appendix 2 - Shipboard Job Description-
        ////------------------------Appendix 2 - Shipboard Job Description-----------------------
        [HttpGet]
        public ActionResult ABJD()
        {
            //---------------AB Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ABJD");
            TempData["ABJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult BJD()
        {
            //---------------Bosun Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BJD");
            TempData["BJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult CCJD()
        {
            //---------------Chief Cook Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CCJD");
            TempData["CCJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult CEJD()
        {
            //---------------Chief Engineer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CEJD");
            TempData["CEJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult COJD()
        {
            //---------------Chief Officer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "COJD");
            TempData["COJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult ETOJD()
        {
            //---------------Electro-Technical Officer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ETOJD");
            TempData["ETOJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult FJD()
        {
            //---------------Fitter Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FJD");
            TempData["FJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult FEJD()
        {
            //---------------Fourth Engineer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FEJD");
            TempData["FEJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult MJD()
        {
            //---------------Master Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MJD");
            TempData["MJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult MessmanJD()
        {
            //---------------Messman Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MessmanJD");
            TempData["MessmanJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult OJD()
        {
            //---------------Oiler Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "OJD");
            TempData["OJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult OSJD()
        {
            //---------------OS Job Description----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "OSJD");
            TempData["OSJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult PJD()
        {
            //---------------Pump man Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PJD");
            TempData["PJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SEJD()
        {
            //---------------Second Engineer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SEJD");
            TempData["SEJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SOJD()
        {
            //---------------Second Officer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SOJD");
            TempData["SOJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult SSOJD()
        {
            //---------------Senior Second Officer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SSOJD");
            TempData["SSOJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult TEJD()
        {
            //---------------Third Engineer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TEJD");
            TempData["TEJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult TOJD()
        {
            //---------------Third Officer Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TOJD");
            TempData["TOJD"] = file.ManualBodyHtml;
            return View(file);
        }
        [HttpGet]
        public ActionResult WJD()
        {
            //---------------Wiper Job Description-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WJD");
            TempData["WJD"] = file.ManualBodyHtml;
            return View(file);
        }

        ////------------End------------Appendix 2 - Shipboard Job Description-----------------------
        #endregion

        #endregion
    }
}