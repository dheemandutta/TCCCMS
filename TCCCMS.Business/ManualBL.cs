using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
namespace TCCCMS.Business
{
    public class ManualBL
    {

        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId, string searchText,int shipId)
        {
            List<Manual> manualsList = new List<Manual>();

            ManualDAL manualDAL = new ManualDAL();

            manualsList = manualDAL.SearchManuals(pageIndex, ref totalCount, pageSize, volumeId, searchText,shipId);

            return manualsList;
        }

        public Manual GetManual(string controllerName, string actionName)
        {
            Manual file = new Manual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetManual(controllerName, actionName);
            return file;
        }

        public Manual GetActionNameByFileName(string fileName)
        {
            Manual file = new Manual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetActionNameByFileName(fileName);
            return file;
        }
        public Volume GetVolumeById(string volumeId)
        {
            Volume vol = new Volume();
            ManualDAL manualDAL = new ManualDAL();

            vol = manualDAL.GetVolumeById(volumeId);
            return vol;
        }

        #region Generate Manual Body Html
        public string GenerateBodyContentHtml(string aXmlPath, int partId)
        {
            ManualBL manuBl = new ManualBL();


            //string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            string xPath = aXmlPath;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            //XmlNode node1 = xDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {

                sb.Append("<div class='container'>");
                foreach (XmlNode volume in node)
                {
                    Volume vol = new Volume(); 
                    string volName = volume.Attributes["name"].Value.ToString();
                    string ctrlName = volume.Attributes["controllername"].Value.ToString();
                    string partName = volName.Split(' ').Last();
                    string mainNodeId = "0";
                    if (volume.Name == "ship")
                    {
                        mainNodeId = volume.Attributes["shipnumber"].Value.ToString();
                    }
                    else if(volume.Name == "material")
                    {
                        mainNodeId = volume.Attributes["materialid"].Value.ToString();
                    }
                    else
                    {
                        mainNodeId = volume.Attributes["id"].Value.ToString();
                    }

                    string mainNodeName = volume.Attributes["name"].Value.ToString();
                    string relaiveFilePath = volName;//---this for pdf preview link path

                    if (Convert.ToInt32(mainNodeId) == partId)
                    {
                        //vol = manuBl.GetVolumeById(nodeId);
                        sb.Append("\n");
                        //sb.Append("<li class='mainmenu'><a href='#'><span class='vul'>Volume <b>I</b> </span><span class='pgnam'>" + volName + "</span></a>");
                        //string s= "'@Url.Action('"
                        //sb.Append("<li class='mainmenu'><a href='@Url.Action('Index', '"+vol.ControllerName+"')'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                        //sb.Append("<li class='mainmenu'><a href='/" + vol.ControllerName + "/Index'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                        sb.Append("<div>");
                        sb.Append("\n");

                        #region child accordian
                        /* ----------Lines Commented on 23rd Feb 2021 @BK  */

                        foreach (XmlNode item in volume)
                        {
                            Manual manual = new Manual();
                            int l = 1;

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
                                    //sb.Append("<li><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                                    //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                                    if(isDownload =="YES")
                                    {
                                        sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath="+ relaiveFilePath+ "' >");
                                    }
                                    else
                                    {
                                        sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
                                    }
                                    
                                    sb.Append(filename + "</a>");
                                    sb.Append("</br>");


                                }
                                else if (type == "PDF")
                                {
                                    sb.Append("\n");
                                    sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                    sb.Append(filename + "</a>");
                                    sb.Append("</br>");

                                }

                            }
                            else if (item.Name == "foldername")
                            {


                                if (Convert.ToInt32(mainNodeId) == 8 && mainNodeName == "Volume VIII")
                                {
                                    string fName = item.Attributes["name"].Value.ToString();
                                    string actionName = item.Attributes["actionname"].Value.ToString();
                                    if (actionName != "")
                                    {
                                        sb.Append("\n");
                                        sb.Append("<a href='/" + ctrlName + "/" + actionName + "' >");
                                        sb.Append(fName + "</a>");
                                        sb.Append("</br>");
                                    }
                                    else
                                    {
                                        string relFilePath = "";

                                        relFilePath = relaiveFilePath + "/" + fName;
                                        sb.Append("\n");
                                        sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fName + "</button>");
                                        sb.Append("\n");
                                        sb.Append("<div class='panel' style='margin-left:5px;'>");
                                        string sChild = GetChild(item, ref l, partName, ctrlName, ref relFilePath);
                                        //l = l;
                                        sb.Append(sChild);
                                        sb.Append("\n");
                                        sb.Append("</div>");
                                        sb.Append("\n");
                                    }

                                }
                                else
                                {
                                    string fName = item.Attributes["name"].Value.ToString();
                                    string relFilePath = "";
                                    relFilePath = relaiveFilePath + "/" + fName;

                                    sb.Append("\n");
                                    sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fName + "</button>");
                                    sb.Append("\n");
                                    sb.Append("<div class='panel' style='margin-left:5px;'>");
                                    string sChild = GetChild(item, ref l, partName, ctrlName, ref relFilePath);
                                    //l = l;
                                    sb.Append(sChild);
                                    sb.Append("\n");
                                    sb.Append("</div>");
                                    sb.Append("\n");
                                }
                                //string fName = item.Attributes["name"].Value.ToString();
                                //sb.Append("\n");
                                //sb.Append("<button class='accordion'>"+fName+"</button>");
                                //sb.Append("\n");
                                //sb.Append("<div class='panel'>");
                                //string sChild = GetChild(item, ref l, partName, ctrlName);
                                ////l = l;
                                //sb.Append(sChild);
                                //sb.Append("\n");
                                //sb.Append("</div>");
                                //sb.Append("\n");
                            }



                        }


                        /* ----End------Lines Commented on 23rd Feb 2021 @BK  */
                        #endregion
                        sb.Append("\n");
                        sb.Append("</div>");

                    }


                }
                sb.Append("\n");
                sb.Append("</div>");

                //WriteToText(sb);

            }
            return sb.ToString();
        }

        public string GetChild(XmlNode node, ref int l, string part, string ctrlName, ref string relativePath)
        {
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
                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
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
                    string fDesc = item.Attributes["description"].Value.ToString();
                    string relFilePath = "";
                    relFilePath = relativePath + "/" + fName;
                    sb.Append("\n");
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

            return sb.ToString();
        }

        public string GenerateShipWiseFolderBodyContentHtml(string aXmlPath, int partId,string folderAction)
        {
            ManualBL manuBl = new ManualBL();
            string xPath = aXmlPath;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xPath);
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {

                sb.Append("<div class='container'>");
                foreach (XmlNode ship in node)
                {
                    Volume vol = new Volume();
                    string volName = ship.Attributes["name"].Value.ToString();
                    string ctrlName = ship.Attributes["controllername"].Value.ToString();
                    string partName = volName.Split(' ').Last();
                    string sNo = "0";
                    if (ship.Name == "ship")
                    {
                        sNo = ship.Attributes["shipnumber"].Value.ToString();
                    }

                    string mainNodeName = ship.Attributes["name"].Value.ToString();
                    string relaiveFilePath = volName;//---this for pdf preview link path

                    if (Convert.ToInt32(sNo) == partId)
                    {
                        //vol = manuBl.GetVolumeById(nodeId);
                        sb.Append("\n");
                        //sb.Append("<li class='mainmenu'><a href='#'><span class='vul'>Volume <b>I</b> </span><span class='pgnam'>" + volName + "</span></a>");
                        //string s= "'@Url.Action('"
                        //sb.Append("<li class='mainmenu'><a href='@Url.Action('Index', '"+vol.ControllerName+"')'><span class='vul'>Volume <b>"+ partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                        //sb.Append("<li class='mainmenu'><a href='/" + vol.ControllerName + "/Index'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam'>" + vol.Description + "</span></a>");
                        sb.Append("<div>");
                        sb.Append("\n");

                        #region child accordian
                        /* ----------Lines Commented on 23rd Feb 2021 @BK  */

                        foreach (XmlNode item in ship)
                        {
                            Manual manual = new Manual();
                            int l = 1;

                           
                            if (item.Name == "foldername")
                            {
                                string actionName = item.Attributes["actionname"].Value.ToString();



                                if(actionName == folderAction)
                                {
                                    string fName = item.Attributes["name"].Value.ToString();
                                    string fDesc = item.Attributes["description"].Value.ToString();
                                    string relFilePath = "";
                                    relFilePath = relaiveFilePath + "/" + fName;

                                    sb.Append("\n");
                                   // sb.Append("<button class='accordion'>" + fName + "</button>");
                                    //sb.Append("\n");
                                    //sb.Append("<div class='panel'>");
                                    string sChild = GetChild(item, ref l, partName, ctrlName, ref relFilePath);
                                    //l = l;
                                    sb.Append(sChild);
                                    //sb.Append("\n");
                                    //sb.Append("</div>");
                                    sb.Append("\n");
                                }
                            }



                        }


                        /* ----End------Lines Commented on 23rd Feb 2021 @BK  */
                        #endregion
                        sb.Append("\n");
                        sb.Append("</div>");

                    }


                }
                sb.Append("\n");
                sb.Append("</div>");

                //WriteToText(sb);

            }
            return sb.ToString();
        }

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

                    
                        sb.Append("\n");sb.Append("<div>");
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

        public string GenerateNoticeBoardFolderBodyContentHtml(string aXmlPath, string folderAction)
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
        #endregion

        #region Common To All And Ref Matrial
        public ShipManual GetCommonToAllManual(string controllerName, string actionName)
        {
            ShipManual file = new ShipManual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetCommonToAllManual(controllerName, actionName);
            return file;
        }
        public ShipManual GetReferenceMaterialManual(string controllerName, string actionName)
        {
            ShipManual file = new ShipManual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetReferenceMaterialManual(controllerName, actionName);
            return file;
        }

        #endregion

        public Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".xls", "application/vnd.ms-excel"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
