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

        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId, string searchText,int shipId, string category)
        {
            List<Manual> manualsList = new List<Manual>();

            ManualDAL manualDAL = new ManualDAL();

            //string modifiedSearchTest = searchText.Replace(" ", " OR ");

            string modifiedSearchString = "";
            string searchString2 = "";

            string[] words = searchText.Split(' ');
            int wordsCount = 1;
            wordsCount = words.Count();
            int cnt = 0;
            foreach(string word in words)
            {
                cnt = cnt + 1;
                if(cnt == 1)
                {
                    modifiedSearchString = word;
                }
                else if(cnt > 1)
                {
                    modifiedSearchString = modifiedSearchString + " OR " + word;
                }

            }
            if(wordsCount ==1)
            {
                searchString2 = words[0].ToString();
            }
            else if (wordsCount > 1)
            {
                cnt = 0;
                searchString2 = "NEAR(";
                foreach (string word in words)
                {
                    cnt = cnt + 1;
                    if (cnt == 1)
                    {
                        searchString2 = searchString2 + word;
                    }
                    else if (cnt > 1)
                    {
                        searchString2 = searchString2 + "," + word;
                    }
                }
                searchString2 = searchString2 + ")";
            }

            modifiedSearchString = modifiedSearchString + ";" + searchString2;
            manualsList = manualDAL.SearchManuals(pageIndex, ref totalCount, pageSize, volumeId, modifiedSearchString, shipId, category);

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
                                string status = item.Attributes["status"].Value.ToString();//added on 11th Nov 2021
                                string version = item.Attributes["version"].Value.ToString();//added on 11th Nov 2021
                                // manual = manuBl.GetActionNameByFileName(filename + ".html");
                                if (type == "DOC" && actionName != "")
                                {
                                    sb.Append("\n");
                                    //sb.Append("<li><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                                    //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");

                                    #region -- Old Logic Commented on 15th Nov 2021
                                    //if (isDownload =="YES")
                                    //{
                                    //    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath="+ relaiveFilePath+ "' >");
                                    //}
                                    //else
                                    //{
                                    //    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >");//&fileName added on28th jun 2021
                                    //}

                                    ////sb.Append(filename + "</a>");
                                    ///*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                    //if (version == "")
                                    //{
                                    //    sb.Append(filename + "</a>");
                                    //}
                                    //else
                                    //{
                                    //    sb.Append(filename + " Rev " + version + "</a>");
                                    //}
                                    #endregion

                                    #region --- New Logic Added on 15th Nov 2021
                                    /* below if .. else if .. else condition with status added on 15th Nov 2021*/
                                    if (status == "DEL")
                                    {
                                        sb.Append("<a href='#' >");
                                        if (version == "")
                                        {
                                            sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                        }
                                        else
                                        {
                                            sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                        }
                                    }
                                    else if (status == "NEW")
                                    {
                                        if (isDownload == "YES")
                                        {
                                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relaiveFilePath + "' >");
                                        }
                                        else
                                        {
                                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >");//&fileName added on28th jun 2021
                                        }
                                        if (version == "")
                                        {
                                            sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                        }
                                        else
                                        {
                                            sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                        }
                                    }
                                    else
                                    {
                                        if (isDownload == "YES")
                                        {
                                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relaiveFilePath + "' >");
                                        }
                                        else
                                        {
                                            sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >");//&fileName added on28th jun 2021
                                        }

                                        //sb.Append(filename + "</a>");
                                        /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                        if (version == "")
                                        {
                                            sb.Append(filename + "</a>");
                                        }
                                        else
                                        {
                                            sb.Append(filename + " Rev " + version + "</a>");
                                        }
                                    }

                                    #endregion

                                    sb.Append("</br>");


                                }
                                else if (type == "PDF")
                                {
                                    sb.Append("\n");

                                    #region -- Old Logic Added on 15th Nov 2021
                                    //sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                    ////sb.Append(filename + "</a>");
                                    ///*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                    //if (version == "")
                                    //{
                                    //    sb.Append(filename + "</a>");
                                    //}
                                    //else
                                    //{
                                    //    sb.Append(filename + " Rev " + version + "</a>");
                                    //}
                                    #endregion

                                    #region --- New Logic Added on 15th Nov 2021
                                    /* below if .. else if .. else condition with status added on 15th Nov 2021*/
                                    if (status == "DEL")
                                    {
                                        sb.Append("<a href='#' >");
                                        if (version == "")
                                        {
                                            sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                        }
                                        else
                                        {
                                            sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                        }
                                    }
                                    else if (status == "NEW")
                                    {
                                        sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                        if (version == "")
                                        {
                                            sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                        }
                                        else
                                        {
                                            sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relaiveFilePath + "' >");
                                        if (version == "")
                                        {
                                            sb.Append(filename + "</a>");
                                        }
                                        else
                                        {
                                            sb.Append(filename + " Rev " + version + "</a>");
                                        }
                                    }
                                    #endregion


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
                                    string fDesc = item.Attributes["description"].Value.ToString();
                                    string relFilePath = "";
                                    relFilePath = relaiveFilePath + "/" + fName;

                                    sb.Append("\n");
                                    //sb.Append("<button class='accordion' style='margin-left:-25px;'>" + fName + "</button>");/**---Commented on 23rd OCT 2021 replace with below if.. elase condition**/
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
                    string status = item.Attributes["status"].Value.ToString();
                    string version = item.Attributes["version"].Value.ToString();//added on 11th Nov 2021
                    // manual = manuBl.GetActionNameByFileName(filename + ".html");
                    if (ctrlName == "NoticeBoard") // Added this condition on 10th Aug 2021
                    {
                       
                        if (type == "PDF")
                        {
                            if(status=="DEL")
                            {
                                sb.Append("\n");
                                //sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                                sb.Append("<a href='#' >");

                                //sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                if (version == "")
                                {
                                    sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                }
                                else
                                {
                                    sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                }
                                sb.Append("</br>");
                                

                            }
                            else if (status == "NEW")
                            {
                                sb.Append("\n");
                                sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                                //sb.Append("<ins style='color:white; background-color: #e90000;'>" + filename + "</ins></a>");
                                
                                //sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                if (version == "")
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                }
                                else
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                }
                                sb.Append("</br>");
                            }
                            else
                            {
                                sb.Append("\n");
                                sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                                
                                //sb.Append(filename + "</a>");
                                /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                if (version == "")
                                {
                                    sb.Append(filename + "</a>");
                                }
                                else
                                {
                                    sb.Append(filename + " Rev " + version + "</a>");
                                }
                                sb.Append("</br>");
                            }
                            

                        }
                    }
                    else
                    {
                        //content copied inside on 10th Aug 2021
                        if (type == "DOC" && actionName != "")
                        {
                            sb.Append("\n");

                            //sb.Append("<li ><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + " </span></a></li>");
                            //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                            //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                            ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                            //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                            //sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
                            #region -- Old Logic Commented on 15th Nov 2021  
                            /* -------Commented on 15th Nov 2021-----*/
                            //if (isDownload == "YES")
                            //{
                            //    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relativePath + "' >");
                            //}
                            //else
                            //{
                            //    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >"); //&fileName added on28th jun 2021
                            //}
                            ////sb.Append(filename + "</a>");
                            ///*  Below condition added on 11th Nov 2021 by commenting the above line*/
                            //if (version == "")
                            //{
                            //    sb.Append(filename + "</a>");
                            //}
                            //else
                            //{
                            //    sb.Append(filename + " Rev " + version + "</a>");
                            //}
                            /* ---End----Commented on 15th Nov 2021-----*/

                            #endregion

                            #region --- New Logic Added on 15th Nov 2021
                            /* below if .. else if .. else condition with status added on 15th Nov 2021*/
                            if (status == "DEL")
                            {
                                sb.Append("<a href='#' >");
                                if (version == "")
                                {
                                    sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                }
                                else
                                {
                                    sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                }
                            }
                            else if (status == "NEW")
                            {
                                if (isDownload == "YES")
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relativePath + "' >");
                                }
                                else
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >"); //&fileName added on28th jun 2021
                                }
                                if (version == "")
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                }
                                else
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                }
                            }
                            else
                            {
                                if (isDownload == "YES")
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&formName=" + filename + "&relformPath=" + relativePath + "' >");
                                }
                                else
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "&fileName=" + filename + "' >"); //&fileName added on28th jun 2021
                                }
                                //sb.Append(filename + "</a>");
                                /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                if (version == "")
                                {
                                    sb.Append(filename + "</a>");
                                }
                                else
                                {
                                    sb.Append(filename + " Rev " + version + "</a>");
                                }
                            }
                            
                            #endregion
                            
                            sb.Append("</br>");
                        }
                        else if (type == "PDF")
                        {
                            sb.Append("\n");

                            #region -- Old Logic Commented on15th Nov 2021
                            //sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                            ////sb.Append(filename + "</a>");
                            ///*  Below condition added on 11th Nov 2021 by commenting the above line*/
                            //if (version == "")
                            //{
                            //    sb.Append(filename + "</a>");
                            //}
                            //else
                            //{
                            //    sb.Append(filename + " Rev " + version + "</a>");
                            //}
                            #endregion

                            #region --- New Logic Added on 15th Nov 2021
                            /* below if .. else if .. else condition with status added on 15th Nov 2021*/
                            if (status == "DEL")
                            {
                                sb.Append("<a href='#' >");
                                if (version == "")
                                {
                                    sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                }
                                else
                                {
                                    sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                }
                            }
                            else if (status == "NEW")
                            {
                                sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                                if (version == "")
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                }
                                else
                                {
                                    sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                }
                            }
                            else
                            {
                                sb.Append("<a href='/" + ctrlName + "/PDFViewer?fileName=" + filename + "&relPDFPath=" + relativePath + "' >");
                                //sb.Append(filename + "</a>");
                                /*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                if (version == "")
                                {
                                    sb.Append(filename + "</a>");
                                }
                                else
                                {
                                    sb.Append(filename + " Rev " + version + "</a>");
                                }
                            }
                            #endregion
                            
                            sb.Append("</br>");

                        }
                        else if (type == "XLS")
                        {
                            if (isDownload == "YES")
                            {
                                sb.Append("\n");
                                #region --- Old logic Commented on 15th Nov 2021
                                //sb.Append("<a href='/" + ctrlName + "/Pages?formName=" + filename + "&relformPath=" + relativePath + "' >");
                                ////sb.Append(filename + "</a>");
                                ///*  Below condition added on 11th Nov 2021 by commenting the above line*/
                                //if (version == "")
                                //{
                                //    sb.Append(filename + "</a>");
                                //}
                                //else
                                //{
                                //    sb.Append(filename + " Rev " + version + "</a>");
                                //}
                                #endregion
                                #region --New Logic add on 15th Nov 2021
                                /* below if .. else if .. else condition with status added on 15th Nov 2021*/
                                if (status == "DEL")
                                {
                                    sb.Append("<a href='#' >");
                                    if (version == "")
                                    {
                                        sb.Append("<del style='color:black'>" + filename + "</del></a>");
                                    }
                                    else
                                    {
                                        sb.Append("<del style='color:black'>" + filename + " Rev " + version + "</del></a>");
                                    }
                                }
                                else if (status == "NEW")
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?formName=" + filename + "&relformPath=" + relativePath + "' >");
                                    if (version == "")
                                    {
                                        sb.Append("<ins style='color:#e90000; '>" + filename + "</ins></a>");
                                    }
                                    else
                                    {
                                        sb.Append("<ins style='color:#e90000; '>" + filename + " Rev " + version + "</ins></a>");
                                    }
                                }
                                else
                                {
                                    sb.Append("<a href='/" + ctrlName + "/Pages?formName=" + filename + "&relformPath=" + relativePath + "' >");
                                    if (version == "")
                                    {
                                        sb.Append(filename + "</a>");
                                    }
                                    else
                                    {
                                        sb.Append(filename + " Rev " + version + "</a>");
                                    }
                                }
                                #endregion
                                sb.Append("</br>");
                            }
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
