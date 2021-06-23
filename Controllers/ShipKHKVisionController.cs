using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class ShipKHKVisionController : Controller
    {
        //--------------------Ship1-------------------------

        private string controllerName = "ShipKHKVision";
        private int shipId = 1;
        private string xmlPath = "~/xmlMenu/" + "ALLSHIPS1.xml";
        ShipBL shipBL = new ShipBL();
        // GET: ShipKHKVision
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            //string xPath = Server.MapPath("~/xmlMenu/" + "ALLSHIPS.xml");
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateBodyContentHtml(xPath, 1);
            return View(file);
        }
        //public ActionResult Pages(string actionName)
        //{
        //    ShipManual file = new ShipManual();
        //    file = shipBL.GetManual(controllerName, actionName);
        //    TempData[actionName] = file.BodyHtml;
        //    return View(file);
        //}

        public ActionResult Pages(string actionName, string formName = "", string relformPath = "")
        {
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            ShipManual file = new ShipManual();
            if (formName == "")
            {
                file = shipBL.GetManual(controllerName, actionName);
            }
            else
            {
                string filePath = "../ShipManualsPDF/" + relformPath + "/";
                filePath = filePath + formName + ".pdf#toolbar=0";
                file.PdfName = formName;
                file.PdfPath = filePath;


                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.BodyHtml);

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
                sb2.Append("<label>" + formName + "</label>");
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
                file.BodyHtml = sb2.ToString();
            }

            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }

        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            ShipManual file = new ShipManual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../ShipManualsPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }
        public ActionResult FormPreview(string fileName, string relPDFPath)
        {
            Manual file = new Manual(); string filePath = "../ShipManualsPDF/" + relPDFPath + "/";
            filePath = filePath + fileName + ".pdf#toolbar=0";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return PartialView("_pvFormPreviewModal", file);
        }
        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/ShipManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                                            .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx") || s.Contains(fileName + ".xls")).First();
            var filePath = Directory.GetFiles(path, "*.*")
                                                       .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
                                                               || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(filePath, FileMode.Open))
            //{
            //    stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            //return File(memory, GetMimeTypes()[ext], Path.GetFileName(filePath));

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes,manualBl.GetMimeTypes()[ext], Path.GetFileName(filePath));
        }
       
        public ActionResult SOPEP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SOPEP");
            return View(file);
        }
        public ActionResult STS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "STS");
            return View(file);
        }
        public ActionResult BWMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BWMP");
            return View(file);
        }
        public ActionResult VOC()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "VOC");
            return View(file);
        }
        public ActionResult MSMPLMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "MSMPLMP");
            return View(file);
        }
        public ActionResult PRPW()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "PRPW");
            return View(file);
        }
        public ActionResult BMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "BMP");
            return View(file);
        }
        public ActionResult CWBMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "CWBMP");
            return View(file);
        }
        public ActionResult GMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "GMP");
            return View(file);
        }
        public ActionResult ETA()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "ETA");
            return View(file);
        }
        public ActionResult SEEMP1()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP1");
            return View(file);
        }
        public ActionResult SEEMP2()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateShipWiseFolderBodyContentHtml(xPath, shipId, "SEEMP2");
            return View(file);
        }

        #region All (22+24+33+17+39)=()

        #region 3. SOPEP (1+21)=(22)

        #region KHKV-SP (1)
        public ActionResult SOPEPSP()
        {
            //---------------Ship_s Particulars (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPSP");
            TempData["SOPEPSP"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region SOPEP-MB (Common to ALL) (21)

        public ActionResult SOPEPAppendices()
        {
            //---------------Appendices---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPAppendices");
            TempData["SOPEPAppendices"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA1CP()
        {
            //---------------Appendix 1 - Contact Point 31Oct20 (need to be updated)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA1CP");
            TempData["SOPEPA1CP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA1LNOCP()
        {
            //---------------Appendix 1 - List of National Operational Contact Point---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA1LNOCP");
            TempData["SOPEPA1LNOCP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA2LPC()
        {
            //---------------Appendix 2 - List of Port Contacts---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA2LPC");
            TempData["SOPEPA2LPC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA3LSIC()
        {
            //---------------Appendix 3 – List of Ship Interest Contacts (ABS)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA3LSIC");
            TempData["SOPEPA3LSIC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA4OPPT()
        {
            //---------------Appendix 4 - Oil Pollution Prevention Team---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA4OPPT");
            TempData["SOPEPA4OPPT"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA5RFC()
        {
            //---------------Appendix 5 – Response Flow Chart---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA5RFC");
            TempData["SOPEPA5RFC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA6NRD()
        {
            //---------------Appendix 6 - Normative Reference Document---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA6NRD");
            TempData["SOPEPA6NRD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA7SPD()
        {
            //---------------Appendix 7 Ship_s Plans and Drawing---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA7SPD");
            TempData["SOPEPA7SPD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA8OSRE()
        {
            //---------------Appendix 8 - Oill Spill Response Equipment---DOCX--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA8OSRE");
            TempData["SOPEPA8OSRE"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPA9ROSD()
        {
            //---------------Appendix 9 - Record of Oil Spill Drill---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPA9ROSD");
            TempData["SOPEPA9ROSD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPContent()
        {
            //---------------Content---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPContent");
            TempData["SOPEPContent"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPIntroduction()
        {
            //---------------Introduction---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPIntroduction");
            TempData["SOPEPIntroduction"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPMC()
        {
            //---------------Manual Control---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPMC");
            TempData["SOPEPMC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPMCover()
        {
            //---------------Manual Cover---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPMCover");
            TempData["SOPEPMCover"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPMR()
        {
            //---------------Manual Revision---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPMR");
            TempData["SOPEPMR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPS1Preamble()
        {
            //---------------Section 1 - Preamble---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPS1Preamble");
            TempData["SOPEPS1Preamble"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPS2RR()
        {
            //---------------Section 2 - Reporting requirements---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPS2RR");
            TempData["SOPEPS2RR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPS2SCD()
        {
            //---------------Section 3 - Steps to control discharge---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPS2SCD");
            TempData["SOPEPS2SCD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPS4NLC()
        {
            //---------------Section 4 - National _ Local Coordination---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPS4NLC");
            TempData["SOPEPS4NLC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult SOPEPS5AI()
        {
            //---------------Section 5 - Additional information---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "SOPEPS5AI");
            TempData["SOPEPS5AI"] = file.BodyHtml;
            return View(file);
        }
        #endregion

        #endregion

        #region 4. KHKV STS 15Oct20  (21+3)=(24)

        #region STS-MB-KHKV (21)
        public ActionResult STSA5GRA()
        {
            //---------------Appendix No.5 - Guidance on risk assessment---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSA5GRA");
            TempData["STSA5GRA"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP3OM()
        {
            //---------------PART A 3-STS OPERATION AND MANAGEMENT---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP3OM");
            TempData["STSP3OM"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP4CR()
        {
            //---------------PART A 4-CONDITIONS AND REQUIREMENTS---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP4CR");
            TempData["STSP4CR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP5Communications()
        {
            //---------------PART A 5-COMMUNICATIONS---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP5Communications");
            TempData["STSP5Communications"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP6Equipment()
        {
            //---------------PART A 6-EQUIPMENT---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP6Equipment");
            TempData["STSP6Equipment"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP7SE()
        {
            //---------------PART A 7-SAFETY AND EMERGENCIES---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP7SE");
            TempData["STSP7SE"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP8OPBM()
        {
            //---------------PART A 8-OPERATIONAL PREPARATIONS BEFORE MANEUVERING---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP8OPBM");
            TempData["STSP8OPBM"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP9MM()
        {
            //---------------PART A 9-MANEUVERING AND MOORING---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP9MM");
            TempData["STSP9MM"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP10CT()
        {
            //---------------PART A 10-CARGO TRANSFER (PROCEDURES ALONGSIDE)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP10CT");
            TempData["STSP10CT"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP11Unmooring()
        {
            //---------------PART A 11-UNMOORING---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP11Unmooring");
            TempData["STSP11Unmooring"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP12Reporting()
        {
            //---------------PART A 12-REPORTING ON STS COMPLETION---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP12Reporting");
            TempData["STSP12Reporting"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP13Glossary()
        {
            //---------------PART A 13-GLOSSARY---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP13Glossary");
            TempData["STSP13Glossary"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP14References()
        {
            //---------------PART A 14-REFERENCES---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP14References");
            TempData["STSP14References"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP15Appendix()
        {
            //---------------PART A 15-APPENDIX---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP15Appendix");
            TempData["STSP15Appendix"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSPSCLSF()
        {
            //---------------PART B - STS SAFTEY CHECK-LISTS AND SAMPLE FORM---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSPSCLSF");
            TempData["STSPSCLSF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSPB3LRD()
        {
            //---------------PART B.3 - List of responsibilty and duties---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSPB3LRD");
            TempData["STSPB3LRD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSPCTOLRB()
        {
            //---------------PART C - STS TRANSFER OPERATION LOG AND STS RECORD BOOK---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSPCTOLRB");
            TempData["STSPCTOLRB"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSPC4CSC()
        {
            //---------------PART C.4-Coastal States Contact-31Oct20---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSPC4CSC");
            TempData["STSPC4CSC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSRH()
        {
            //---------------Revision History---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSRH");
            TempData["STSRH"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSMBO()
        {
            //---------------STS Main Body - Responsible Persons for STS Operations---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSMBO");
            TempData["STSMBO"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSMBTC()
        {
            //---------------STS Main Body - Table of Contents---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSMBTC");
            TempData["STSMBTC"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region STS-SP-KHKV (3)
        public ActionResult STSMC()
        {
            //---------------Manual Cover (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSMC");
            TempData["STSMC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP1Introduction()
        {
            //---------------PART A 1-INTRODUCTION (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP1Introduction");
            TempData["STSP1Introduction"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult STSP2SP()
        {
            //---------------PART A 2-SHIP PARTICULAR (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "STSP2SP");
            TempData["STSP2SP"] = file.BodyHtml;
            return View(file);
        }


        #endregion


        #endregion

        #region 5. KHKV BWMP 15Oct20 (1+17+15) = (33)
        public ActionResult BWMPKHKV()
        {
            //---------------ONE PDF-BWMP-KHKV-15Oct20---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPKHKV");
            TempData["BWMPKHKV"] = file.BodyHtml;
            return View(file);
        }

        #region Appendix ((8-3)=>5+4+1+1+5+1)=(17)
        public ActionResult BWMPAContent()
        {
            //---------------Appendix Content---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPAContent");
            TempData["BWMPAContent"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA1NLR()
        {
            //---------------Appendix I - National and Local Requirement (R)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA1NLR");
            TempData["BWMPA1NLR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA3BEP()
        {
            //---------------Appendix III - Ballast Exchange Plan---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA3BEP");
            TempData["BWMPA3BEP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA9WMOD()
        {
            //---------------Appendix IX - WMO Definition---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA9WMOD");
            TempData["BWMPA9WMOD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA5FTBER()
        {
            //---------------Appendix V - Flow Through Ballast exchange record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA5FTBER");
            TempData["BWMPA5FTBER"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA5FTERS()
        {
            //---------------Appendix V - Flow Through Exchange Record-Sample---XLS--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA5FTERS");
            TempData["BWMPA5FTERS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA8BWMTR()
        {
            //---------------Appendix VIII - Ballast water management training record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA8BWMTR");
            TempData["BWMPA8BWMTR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA10WBTASP()
        {
            //---------------Appendix X - WBT arrangement _ Sampling Point---XLS--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA10WBTASP");
            TempData["BWMPA10WBTASP"] = file.BodyHtml;
            return View(file);
        }

        #region Appendix II - Normative Reference (4)
        public ActionResult BWMPA2A868()
        {
            //---------------Appendix II - A.868(20) (1)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA2A868");
            TempData["BWMPA2A868"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA2Contents()
        {
            //---------------Appendix II - Contents---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA2Contents");
            TempData["BWMPA2Contents"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA2MEPC124()
        {
            //---------------Appendix II - MEPC 124(53) (2)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA2MEPC124");
            TempData["BWMPA2MEPC124"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA2MEPC127()
        {
            //---------------Appendix II - MEPC 127(53) (3)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA2MEPC127");
            TempData["BWMPA2MEPC127"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region Appendix IV - Stability Information (2-1)=(1)
        public ActionResult BWMPA4SI()
        {
            //---------------Appendix IV - KHKV Stability Information (2)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA4SI");
            TempData["BWMPA4SI"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA4SInformation()
        {
            //---------------Appendix IV - Stability Information---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA4SInformation");
            TempData["BWMPA4SInformation"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region Appendix VI - Reporting Forms and Book - Office (2-1)=(1)
        public ActionResult BWMPA6BWRF()
        {
            //---------------Appendix VI - Ballast Water Reporting Form (5)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA6BWRF");
            TempData["BWMPA6BWRF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA6RFB()
        {
            //---------------Appendix VI - Reporting form and book---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA6RFB");
            TempData["BWMPA6RFB"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region Appendix VII - Reporting Form - Port States (12-7) =(5)
        public ActionResult BWMPA7BWRPI()
        {
            //---------------Appendix VII - 1a. Ballast-Water-Reporting-Form-Instructions (1a)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7BWRPI");
            TempData["BWMPA7BWRPI"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMP7BUSABWRF()
        {
            //--------------Appendix VII - 1b. USA-BWRF (up to 20 BWT) (1b)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMP7BUSABWRF");
            TempData["BWMP7BUSABWRF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMP7CUSABWRF()
        {
            //---------------Appendix VII - 1c. USA-BWRF (up to 36 BWT) (1c)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMP7CUSABWRF");
            TempData["BWMP7CUSABWRF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7PBWQRG()
        {
            //---------------Appendix VII - 2a. pilot-BW-QRG04---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7PBWQRG");
            TempData["BWMPA7PBWQRG"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7MOFBW()
        {
            //---------------Appendix VII - 2b. mars-offline-form-ballast-water---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7MOFBW");
            TempData["BWMPA7MOFBW"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7BBWRN()
        {
            //---------------Appendix VII - 3. Brazil - Ballast Water Reporting  NORMAM20 (3)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7BBWRN");
            TempData["BWMPA7BBWRN"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7ANZBBDP()
        {
            //---------------Appendix VII - 4a. NZ-Biofouling-and-Ballast-Declaration-Part-1-and-2-MAY2016--DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7ANZBBDP");
            TempData["BWMPA7ANZBBDP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7BNZBBDP()
        {
            //---------------Appendix VII - 4b. NZ-Biofouling-and-Ballast-Declaration-Part-3-MAY2016---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7BNZBBDP");
            TempData["BWMPA7BNZBBDP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7CNZBBDP()
        {
            //---------------Appendix VII - 4c. NZ-Biofouling-and-Ballast-Declaration-Part-3-extra-page-MAY2016---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7CNZBBDP");
            TempData["BWMPA7CNZBBDP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7RBWRF()
        {
            //---------------Appendix VII - 5. ROPME (Sea Area) Ballast Water Reporting Form---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7RBWRF");
            TempData["BWMPA7RBWRF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA7CadBWRF()
        {
            //---------------Appendix VII - 6. Canadian BW Reporting Form-20 tanks (85-0512AE)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA7CadBWRF");
            TempData["BWMPA7CadBWRF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPARF()
        {
            //---------------Appendix VII - Reporting form - Port States---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPARF");
            TempData["BWMPARF"] = file.BodyHtml;
            return View(file);
        }

        #endregion


        #region Appendix XI - Ship_s Plan (7-6)=(1)
        public ActionResult BWMPA11SSVCSPFI()
        {
            //---------------Appendix XI - 1. Standard Symbol, Valve, Cock, Stranier, Pipe Fitting and Instrument.---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11SSVCSPFI");
            TempData["BWMPA11SSVCSPFI"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11BWASS3()
        {
            //---------------Appendix XI - 4. Ballast Water Air _ Sounding System (3)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11BWASS3");
            TempData["BWMPA11BWASS3"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11SP()
        {
            //---------------Appendix XI - Ship_s Plan---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11SP");
            TempData["BWMPA11SP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11BTA()
        {
            //---------------Appendix XI -2. Ballast Tank Arrangement ---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11BTA");
            TempData["BWMPA11BTA"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11BWPS()
        {
            //---------------Appendix XI -3. Ballast Water Piping System ---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11BWPS");
            TempData["BWMPA11BWPS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11BWASS()
        {
            //---------------Appendix XI- 4. Ballast Water Air _ Sounding System (1)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11BWASS");
            TempData["BWMPA11BWASS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPA11BWASS2()
        {
            //---------------Appendix XI- 4. Ballast Water Air _ Sounding System (2) ---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPA11BWASS2");
            TempData["BWMPA11BWASS2"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #endregion


        #region  Main Body (15)
        public ActionResult BWMPContent()
        {
            //---------------Content---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPContent");
            TempData["BWMPContent"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS1Introduction()
        {
            //---------------Section 1 - Introduction---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS1Introduction");
            TempData["BWMPS1Introduction"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS2Definition()
        {
            //---------------Section 2 - Definition---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS2Definition");
            TempData["BWMPS2Definition"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS3SPS()
        {
            //---------------Section 3 - Ship_s Particular and System---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS3SPS");
            TempData["BWMPS3SPS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS4BWS()
        {
            //---------------Section 4 - Ballast Water Sampling---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS4BWS");
            TempData["BWMPS4BWS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS5BEP()
        {
            //---------------Section 5 - Ballast Exchange Procedures---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS5BEP");
            TempData["BWMPS5BEP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS6SP()
        {
            //---------------Section 6 - Safety Procedures--DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS6SP");
            TempData["BWMPA8BWMTR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS7OSR()
        {
            //--------------Section 7 - Operational and Safety Restrictions---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS7OSR");
            TempData["BWMPS7OSR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS8BWMM()
        {
            //---------------Section 8 - Ballast Water Management Method---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS8BWMM");
            TempData["BWMPS8BWMM"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS9SCD()
        {
            //---------------Section 9 - Sediment Control and Disposal---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS9SCD");
            TempData["BWMPS9SCD"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS10Communication()
        {
            //---------------Section 10 - Communication---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS10Communication");
            TempData["BWMPS10Communication"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS11EO()
        {
            //--------------Section 11 - Environment Officer---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS11EO");
            TempData["BWMPS11EO"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS12RR()
        {
            //---------------Section 12 - Reporting Requirement---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS12RR");
            TempData["BWMPS12RR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS13TF()
        {
            //---------------Section 13 - Training and Familiarisation---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS13TF");
            TempData["BWMPS13TF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult BWMPS14HWB()
        {
            //---------------Section 14 - Heavy Weather Ballasting---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "BWMPS14HWB");
            TempData["BWMPS14HWB"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region PDF-KHKV-BWMP 15Oct20 ()


        #endregion

        #endregion

        #region  6. KHKV VOC 15Oct20 () =(17)

        #region Main Body (27-10)=(17)

        public ActionResult VOCChap1()
        {
            //---------------Chapter 1---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap1");
            TempData["VOCChap1"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap2()
        {
            //---------------Chapter 2---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap2");
            TempData["VOCChap2"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap3()
        {
            //---------------Chapter 3---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap3");
            TempData["VOCChap3"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap4()
        {
            //---------------Chapter 4---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap4");
            TempData["VOCChap4"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap5()
        {
            //---------------Chapter 5---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap5");
            TempData["VOCChap5"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap6()
        {
            //---------------Chapter 6---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap6");
            TempData["VOCChap6"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap7()
        {
            //---------------Chapter 7---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap7");
            TempData["VOCChap7"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap8()
        {
            //---------------Chapter 8---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap8");
            TempData["VOCChap8"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap9()
        {
            //---------------Chapter 9---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap9");
            TempData["VOCChap9"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap10()
        {
            //---------------Chapter 10---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap10");
            TempData["VOCChap10"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap11()
        {
            //---------------Chapter 11---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap11");
            TempData["VOCChap11"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap12()
        {
            //---------------Chapter 12---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap12");
            TempData["VOCChap12"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13GAP()
        {
            //---------------Chapter 13.1 GA Plan---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13GAP");
            TempData["VOCChap13GAP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13CS()
        {
            //---------------Chapter 13.2 Cargo System---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13CS");
            TempData["VOCChap13CS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13BS()
        {
            //---------------Chapter 13.3 Ballast System---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13BS");
            TempData["VOCChap13BS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13CTVIGS()
        {
            //---------------Chapter 13.4 Cargo tank vent _ Inert Gas system---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13CTVIGS");
            TempData["VOCChap13CTVIGS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13CTCS()
        {
            //---------------Chapter 13.5 COW and Tank cleaning system---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13CTCS");
            TempData["VOCChap13CTCS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13SCM()
        {
            //--------------Chapter 13.6 Specification for COW machine---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13SCM");
            TempData["VOCChap13SCM"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13VEHHAS()
        {
            //---------------Chapter 13.7 Vapour Emission and High high alarm system---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13VEHHAS");
            TempData["VOCChap13VEHHAS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13EEBI()
        {
            //---------------Chapter 13.8 Electric earth bonding installation---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13EEBI");
            TempData["VOCChap13EEBI"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13VC()
        {
            //---------------Chapter 13.9.1 Vapour Connection ---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13VC");
            TempData["VOCChap13VC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13VC2()
        {
            //--------------Chapter 13.9.2 Vapour Connection ---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13VC2");
            TempData["VOCChap13VC2"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13MR()
        {
            //---------------Chapter 13.10 Marpol Annex VI Regulation 15---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13MR");
            TempData["VOCChap13MR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCChap13MEPC()
        {
            //---------------Chapter 13.11 - MEPC 185(59)---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCChap13MEPC");
            TempData["VOCChap13MEPC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOChap13()
        {
            //---------------Chapter 13---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOChap13");
            TempData["VOChap13"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOChap14()
        {
            //---------------Chapter 14---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOChap14");
            TempData["VOChap14"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult VOCCover()
        {
            //---------------Cover - KHK Vision ---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "VOCCover");
            TempData["VOCCover"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region PDF-KHKV-VOC 15Oct20 ()


        #endregion

        #endregion

        #region 13. MSMP_LMP (27+12)=(39)

        #region Common to ALL (6+7+14)=(27)
        public ActionResult LMPContent()
        {
            //---------------Content---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPContent");
            TempData["LMPContent"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPDL()
        {
            //---------------Distribution List---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPDL");
            TempData["LMPDL"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPMR()
        {
            //---------------Manual Revision---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPMR");
            TempData["LMPMR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPMSMPDP()
        {
            //---------------ConSection 1 - MSMP DIVIDER PAGEtent---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPMSMPDP");
            TempData["LMPMSMPDP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2DP()
        {
            //---------------Section 2 Part A - LMP DIVIDER PAGE---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2DP");
            TempData["LMPS2DP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2BDP()
        {
            //---------------Section 2 Part B - LMP DIVIDER PAGE---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2BDP");
            TempData["LMPS2BDP"] = file.BodyHtml;
            return View(file);
        }

        #region Relevant SMS Forms (7)
        public ActionResult LMPMRTIR()
        {
            //---------------MA041C- Mooring Rope Tail Inspection Record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPMRTIR");
            TempData["LMPMRTIR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPFMRIR()
        {
            //---------------MA041D - Fiber Mooring Rope  Inspection Record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPFMRIR");
            TempData["LMPFMRIR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPMWRIR()
        {
            //---------------MA041E - Mooring Wire Rope  Inspection Record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPMWRIR");
            TempData["LMPMWRIR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPPMORA()
        {
            //---------------MA075-Port Mooring Operation Record - Alframax---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPPMORA");
            TempData["LMPPMORA"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPPMOR()
        {
            //---------------MA075-Port Mooring Operation Record - KHKV---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPPMOR");
            TempData["LMPPMOR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPPMORVLCC()
        {
            //---------------MA075-Port Mooring Operation Record - VLCC---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPPMORVLCC");
            TempData["LMPPMORVLCC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPCFMS()
        {
            //---------------SP055-Crew Familiarization for Mooring Systems---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPCFMS");
            TempData["LMPCFMS"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region Section 2 Part B - LMP (12+1+1)=(14)
        public ActionResult LMPS2FI()
        {
            //---------------Section 2 Part B0 - Filing Instructions---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2FI");
            TempData["LMPS2FI"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2PMR()
        {
            //---------------Section 2 Part B1 - Port Mooring Records---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2PMR");
            TempData["LMPS2PMR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2LMR()
        {
            //---------------Section 2 Part B2 - Load Monitoring Records---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2LMR");
            TempData["LMPS2LMR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2RML()
        {
            //---------------Section 2 Part B3 - Record of Mooring Lines---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2RML");
            TempData["LMPS2RML"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2MLRHR()
        {
            //---------------Section 2 Part B4 - Mooring line running hours record---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2MLRHR");
            TempData["LMPS2MLRHR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2RIMLAT()
        {
            //---------------Section 2 Part B5 - Record of inspection of Mooring lines abd tails---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2RIMLAT");
            TempData["LMPS2RIMLAT"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2RIMFB()
        {
            //---------------Section 2 Part B6 - Record of inspections of Mooring Fittings on board---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2RIMFB");
            TempData["LMPS2RIMFB"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2DCMS()
        {
            //---------------Section 2 Part B7 - Details of Changes in Mooring System---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2DCMS");
            TempData["LMPS2DCMS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2RSCFMS()
        {
            //---------------Section 2 Part B8 - Record of ship’s crew familiarisation with mooring systems---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2RSCFMS");
            TempData["LMPS2RSCFMS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2LCS()
        {
            //---------------Section 2 Part B9 - List of critical spares---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2LCS");
            TempData["LMPS2LCS"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2MLC()
        {
            //---------------Section 2 Part B12 - Mooring line Certificates (Copies)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2MLC");
            TempData["LMPS2MLC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2RMWBRTCB()
        {
            //---------------Section 2 Part B13 - Records of Mooring Winch Brake Rendering Tests conducted on board---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2RMWBRTCB");
            TempData["LMPS2RMWBRTCB"] = file.BodyHtml;
            return View(file);
        }

        #region Section 2 of Part B11 (1-1)=(1)
        public ActionResult LMPCWR()
        {
            //---------------Care of wires and ropes---PDF--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPCWR");
            TempData["LMPCWR"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2CCC()
        {
            //---------------Section 2 Part B11 - Company Circulars and Communication---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2CCC");
            TempData["LMPS2CCC"] = file.BodyHtml;
            return View(file);
        }
        #endregion
        #region Section 2 Part B10 (1)
        public ActionResult LMPS2MCC()
        {
            //---------------Section 2 Part B10 - Manufacturers Circulars or Communications---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2MCC");
            TempData["LMPS2MCC"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #endregion

        #endregion
        #region MSMP_LMP-SP (KHKV) (4+7+1)=(12)
        public ActionResult LMPSPIntroduction()
        {
            //---------------Introduction (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPSPIntroduction");
            TempData["LMPSPIntroduction"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPSPCover()
        {
            //---------------KHKV Cover---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPSPCover");
            TempData["LMPSPCover"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPSPMC()
        {
            //---------------Manual Control - KHKV 01---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPSPMC");
            TempData["LMPSPMC"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS2()
        {
            //---------------Section 2 Part A - LMP (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS2");
            TempData["LMPS2"] = file.BodyHtml;
            return View(file);
        }

        #region Section 1 - MSMP (KHKV) (7)
        public ActionResult LMPS1GSP()
        {
            //---------------Section 1 Part A - General Ship Particulars (KHKV)---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1GSP");
            TempData["LMPS1GSP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS1MEDP()
        {
            //---------------Section 1 Part B - Mooring Equipment Design Philosophy---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1MEDP");
            TempData["LMPS1MEDP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS1DLME()
        {
            //---------------Section 1 Part C - Detailed List of Mooring Equiment---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1DLME");
            TempData["LMPS1DLME"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS1IMRSP()
        {
            //---------------Section 1 Part D - Inspection, Maintenance _ Retirement Strategies or Principles---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1IMRSP");
            TempData["LMPS1IMRSP"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS1RCMSPHF()
        {
            //---------------Section 1 Part E - Risk and Chagne Mgmt, Safety of Personnel and Human Factors---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1RCMSPHF");
            TempData["LMPS1RCMSPHF"] = file.BodyHtml;
            return View(file);
        }
        public ActionResult LMPS1RD()
        {
            //---------------Section 1 Part F - Records and Documentation---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1RD");
            TempData["LMPS1RD"] = file.BodyHtml;
            return View(file);

        }
        public ActionResult LMPS1MSMPR()
        {
            //---------------Section 1 Part G - Mooring System Management Plan Register---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPS1MSMPR");
            TempData["LMPS1MSMPR"] = file.BodyHtml;
            return View(file);
        }

        #endregion

        #region Section 2 Part B - Vessel Mooring Layout (1)
        public ActionResult LMPMOR()
        {
            //---------------MA075-Port Mooring Operation Record - KHKV---DOC--------------
            ShipManual file = new ShipManual();
            file = shipBL.GetManual(controllerName, "LMPMOR");
            TempData["LMPMOR"] = file.BodyHtml;
            return View(file);
        }
        #endregion

        #endregion

        #endregion

        #endregion

    }
}