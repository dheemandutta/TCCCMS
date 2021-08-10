using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class CommonToAllManualController : Controller
    {
        private string controllerName = "CommonToAllManual";
        private string xmlPath = "~/xmlMenu/" + "COMMONTOALL.xml";
        ManualBL manualBL = new ManualBL();
        // GET: CommonToAllManual
        public ActionResult Index()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "COMMONTOALL.xml");
            //file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 0);
            file.ManualBodyHtml = GenerateC2AMenu();
            return View(file);
        }
        //public ActionResult Pages(string actionName)
        //{
        //    ShipManual file = new ShipManual();
        //    file = manualBL.GetCommonToAllManual(controllerName, actionName);
        //    TempData[actionName] = file.BodyHtml;
        //    return View(file);
        //}
        public ActionResult Pages(string actionName, string formName = "", string relformPath = "")
        {
            
            System.Web.HttpContext.Current.Session["ManualFileActionName"] = actionName;// this session used in Breadcrumb Navigation
            ShipManual file = new ShipManual();
            //file = manualBL.GetCommonToAllManual(controllerName, actionName);
            //if (formName != "")
            //{
            //    StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
            //    sb.Append(file.BodyHtml);

            //    sb.Append("</div>");
            //    sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
            //    sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
            //    sb.Append("</div>");

            //    file.BodyHtml = sb.ToString();
            //}

            //--------------------------------------------------------------------------------
            if (formName == "")
            {
                file = manualBL.GetCommonToAllManual(controllerName, actionName);
            }
            else
            {
                string filePath = "../CommonToAllManualsPDF/" + relformPath + "/";
                filePath = filePath + formName + ".pdf#toolbar=0";
                file.PdfName = formName;
                file.PdfPath = filePath;
                //-----------Not used old-------------
                StringBuilder sb = new StringBuilder("<div><div style = 'height: 800px; overflow: scroll;' >");
                sb.Append(file.BodyHtml);

                sb.Append("</div>");
                sb.Append("<div class='col-sm-12.><div class='row'><div class='col-sm-6'><a href='/" + controllerName + "/Download?fileName=");
                sb.Append(formName + "&relformPath=" + relformPath + "' class='btn btn-info btn-sm' style='background-color: #e90000;' >Download</a></div></div></div>");
                sb.Append("</div>");
                //------End-----Not used old-------------

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
            //--------------------------------------------------------------------------------

            //TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PDFViewer(string fileName, string relPDFPath)
        {
            //-------------
            Manual file = new Manual();
            //string filePath = "../ManualsPDF/Volume I/";
            string filePath = "../CommonToAllManualsPDF/" + relPDFPath + "/";
            // filePath = filePath + fileName + ".pdf#toolbar=0&zoom=137";//----#zoom=85&scrollbar=0&toolbar=0&navpanes=0
            filePath = filePath + fileName + ".pdf#zoom=137";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        public FileResult Download(string fileName, string relformPath)
        {
            ManualBL manualBl = new ManualBL();
            string path = Server.MapPath("~/CommonToAllManualsPDF/" + relformPath + "/");
            //var folderPath = Path.Combine(path, relformPath);
            //var filePath = Path.Combine(path, fileName);
            //var filePath = Directory.GetFiles(path, "*.doc?")
            //                        .Where(s => s.Contains(fileName + ".doc") || s.Contains(fileName + ".DOC") || s.Contains(fileName + ".docx")
            //                                || s.Contains(fileName + ".xls") || s.Contains(fileName + ".xlsx")).First();

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
        
        public ActionResult PPM()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "PPM");
            return View(file);
        }
        public ActionResult CGS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "CGS");
            return View(file);
        }
        public ActionResult GCGS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "GCGS");
            return View(file);
        }
        public ActionResult OMPCOV()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "OMPCOV");
            return View(file);
        }
        public ActionResult TOM()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "TOM");
            return View(file);
        }
        public ActionResult PCMP()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "PCMP");
            return View(file);
        }
        public ActionResult EMS()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "EMS");
            return View(file);
        }
        public ActionResult Manning()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "Manning");
            return View(file);
        }
        public ActionResult CEM()
        {
            Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "CEM");
            return View(file);
        }

        public string GenerateC2AMenu()
        {
            string xPath = Server.MapPath("~/xmlMenu/" + "COMMONTOALL.xml");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {

                sb.Append("<ul>");
                foreach (XmlNode c2a in node)
                {

                    string c2aName = c2a.Attributes["name"].Value.ToString();
                    string ctrlName = c2a.Attributes["controllername"].Value.ToString();
                    string relaiveFilePath = c2aName;//---this for pdf preview link path

                    foreach (XmlNode item in c2a)
                    {
                        if (item.Name == "filename")
                        {
                            string filename = item.InnerText.ToString();
                            string actionName = item.Attributes["actionname"].Value.ToString();
                            string type = item.Attributes["doctype"].Value.ToString();
                            string isDownload = item.Attributes["isdownloadable"].Value.ToString();
                            // manual = manuBl.GetActionNameByFileName(filename + ".html");
                            if (type == "DOC" && actionName != "")
                            {
                                sb.Append("\n");

                                sb.Append("<li ><span>></span><a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >" + filename + "</a>");
                            }
                            else if (type == "PDF")
                            {
                                sb.Append("\n");
                                sb.Append("<li ><span style='margin-right:5px;'>></span><a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                sb.Append(filename + "</a>");
                                sb.Append("</br>");

                            }

                        }
                        else if (item.Name == "foldername")
                        {


                            string fName = item.Attributes["name"].Value.ToString();
                            string fDesc = item.Attributes["description"].Value.ToString();
                            string actionName = item.Attributes["actionname"].Value.ToString();

                            if (actionName != "")
                            {
                                if (fDesc != "")
                                {
                                    sb.Append("<li ><span>></span><a href='/" + ctrlName + "/" + actionName + "'>" + fDesc + "</a>");
                                }
                                else
                                {
                                    sb.Append("<li><a href='/" + ctrlName + "/" + actionName + "'>" + fName + "</a>");

                                }

                            }
                        }
                    }

                    sb.Append("\n");
                    sb.Append("\n");
                    sb.Append("</li>");


                }
                sb.Append("\n");
                sb.Append("</ul>");

                //WriteToText(sb);

            }
            return sb.ToString();
        }






        public ActionResult VRP()
        {
            //Session["IsSearched"] = "0";
            ManualBL manualBL = new ManualBL();
            ShipManual file = new ShipManual();
            string xPath = Server.MapPath(xmlPath);
            //file.BodyHtml = manualBL.GenerateC2AFolderBodyContentHtml(xPath, "VRP");
            file.BodyHtml = GenerateC2AFolderBodyContentHtml(xPath, "VRP");
            return View(file);
        }

        #region Generare Part menu copy here 7th Aug 2021
        public string GenerateC2AFolderBodyContentHtml(string aXmlPath, string folderAction)
        {
            ManualBL manuBl = new ManualBL();
            string xPath = aXmlPath;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {

                sb.Append("<div class='container'>");
                foreach (XmlNode c2a in node)
                {
                    Volume vol = new Volume();
                    string c2aName = c2a.Attributes["name"].Value.ToString();
                    string ctrlName = c2a.Attributes["controllername"].Value.ToString();
                    string partName = c2aName.Split(' ').Last();

                    string mainNodeName = c2a.Attributes["name"].Value.ToString();
                    string relaiveFilePath = c2aName;//---this for pdf preview link path


                    sb.Append("\n"); sb.Append("<div>");
                    sb.Append("\n");

                    #region child accordian
                    foreach (XmlNode item in c2a)
                    {
                        Manual manual = new Manual();
                        int l = 1;


                        if (item.Name == "foldername")
                        {
                            string actionName = item.Attributes["actionname"].Value.ToString();
                            if (actionName == folderAction)
                            {
                                string fName = item.Attributes["name"].Value.ToString();
                                string fDesc = item.Attributes["description"].Value.ToString();
                                string relFilePath = "";
                                relFilePath = relaiveFilePath + "/" + fName;

                                sb.Append("\n");
                                string sChild = GetChild(item, ref l, partName, ctrlName, ref relFilePath);
                                sb.Append(sChild);
                                sb.Append("\n");
                            }
                        }



                    }

                    #endregion
                    sb.Append("\n");
                    sb.Append("</div>");




                }
                sb.Append("\n");
                sb.Append("</div>");

                //WriteToText(sb);

            }
            return sb.ToString();
        }

        public string GetChild(XmlNode node, ref int l, string part, string ctrlName, ref string relativePath)
        {
            //var sessionValue = HttpContext.Current.Session["ActiveDepartment"];
            ManualBL manuBl = new ManualBL();
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode item in node)
            {
                Manual manual = new Manual();
                if (item.Name == "filename")
                {

                    string filename = item.InnerText.ToString();
                    string actionName = item.Attributes["actionname"].Value.ToString();
                    string type = item.Attributes["doctype"].Value.ToString();
                    string isDownload = item.Attributes["isdownloadable"].Value.ToString();
                    // manual = manuBl.GetActionNameByFileName(filename + ".html");
                    if (type == "DOC" && actionName != "")
                    {
                        sb.Append("\n");
                        //sb.Append("<li ><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                        //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                        //sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
                        if (isDownload == "YES")
                        {
                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relativePath + "' >");
                        }
                        else
                        {
                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >"); //&fileName added on28th jun 2021
                        }
                        sb.Append(filename + "</a>");
                        sb.Append("</br>");
                    }
                    else if (type == "PDF")
                    {
                        sb.Append("\n");
                        sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                        sb.Append(filename + "</a>");
                        sb.Append("</br>");

                    }
                    else if (type == "XLS")
                    {
                        if (isDownload == "YES")
                        {
                            sb.Append("\n");
                            sb.Append("<a href='/" + ctrlName + "/Pages?formName=" + filename + "&relformPath=" + relativePath + "' >");
                            sb.Append(filename + "</a>");
                            sb.Append("</br>");
                        }
                    }


                }
                else if (item.Name == "foldername")
                {
                    int x = 0;
                    string fName = item.Attributes["name"].Value.ToString();
                    string fActionName = item.Attributes["actionname"].Value.ToString(); // Added on 7th Aug 2021 (for C2A) 
                    string fDesc = item.Attributes["description"].Value.ToString();
                    string relFilePath = "";
                    relFilePath = relativePath + "/" + fName;
                    sb.Append("\n");
                    if(fActionName != "" && fActionName== "OFFICE")
                    {
                        if(Session["UserType"].ToString() == "2")
                        {
                            if (fDesc != "")
                            {
                                sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fDesc + "</button>");
                            }
                            else
                            {
                                sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fName + "</button>");
                            }

                            sb.Append("\n");
                            sb.Append("<div class='panel' style='margin-left:5px;'>");
                            x = l + 1;
                            string sChild = GetChild(item, ref x, part, ctrlName, ref relFilePath);
                            sb.Append(sChild);
                            sb.Append("\n");
                            sb.Append("</div>");
                            sb.Append("\n");
                        }
                        
                    }
                    else
                    {
                        if (fDesc != "")
                        {
                            sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fDesc + "</button>");
                        }
                        else
                        {
                            sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fName + "</button>");
                        }

                        sb.Append("\n");
                        sb.Append("<div class='panel' style='margin-left:5px;'>");
                        x = l + 1;
                        string sChild = GetChild(item, ref x, part, ctrlName, ref relFilePath);
                        sb.Append(sChild);
                        sb.Append("\n");
                        sb.Append("</div>");
                        sb.Append("\n");
                    }

                    
                }

            }

            return sb.ToString();
        }

        #endregion
    }
}