﻿using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Text;
namespace TCCCMS.Business
{
    public class ManualBL
    {
        
        public List<Manual> SearchManuals(int pageIndex, ref int totalCount, int pageSize, int volumeId,string searchText)
        {
            List<Manual> manualsList = new List<Manual>();

            ManualDAL manualDAL = new ManualDAL();

            manualsList = manualDAL.SearchManuals(pageIndex, ref totalCount, pageSize, volumeId,searchText);

            return manualsList;
        }

        public Manual GetManual(string controllerName, string actionName)
        {
            Manual file = new Manual();
            ManualDAL manualDAL = new ManualDAL();

            file = manualDAL.GetManual(controllerName,  actionName);
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
        public string GenerateBodyContentHtml(string aXmlPath,int partId)
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
                    string nodeId = volume.Attributes["id"].Value.ToString();
                    if(Convert.ToInt32(nodeId) == partId)
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
                               // manual = manuBl.GetActionNameByFileName(filename + ".html");
                                if (actionName != null)
                                {
                                    sb.Append("\n");
                                    //sb.Append("<li><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + partName + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + "</span></a></li>");
                                    ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                                    //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                                    sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
                                    sb.Append( filename + "</a>");
                                    sb.Append("</br>");


                                }

                            }
                            else if (item.Name == "foldername")
                            {

                                string fName = item.Attributes["name"].Value.ToString();
                                sb.Append("\n");
                                sb.Append("<button class='accordion'>"+fName+"</button>");
                                sb.Append("\n");
                                sb.Append("<div class='panel'>");
                                string sChild = GetChild(item, ref l, partName, ctrlName);
                                //l = l;
                                sb.Append(sChild);
                                sb.Append("\n");
                                sb.Append("</div>");
                                sb.Append("\n");
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

        public string GetChild(XmlNode node, ref int l, string part,string ctrlName)
        {
            ManualBL manuBl = new ManualBL();
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode item in node)
            {
                Manual manual = new Manual();
                if (item.Name == "filename")
                {

                    string filename = item.InnerText.ToString();
                    string actionName= item.Attributes["actionname"].Value.ToString();
                   // manual = manuBl.GetActionNameByFileName(filename + ".html");
                    if (actionName != null)
                    {
                        sb.Append("\n");
                        //sb.Append("<li ><a href='@Url.Action('" + manual.ActionName + "', '" + manual.ControllerName + "'><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background - color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        //sb.Append("<li ><a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' ><span class='vul'>Volume <b>" + part + "</b> </span><span class='pgnam' style='background-color:salmon; '>" + filename + " </span></a></li>");
                        ///--------below 2 lines chenged with next uper line on 20th Feb 2021-------
                        //sb.Append("<a href='/" + manual.ControllerName + "/Pages?actionName=" + manual.ActionName + "' >");
                        sb.Append("<a href='/" + ctrlName + "/Pages?actionName=" + actionName + "' >");
                        sb.Append( filename + "</a>");
                        sb.Append("</br>");
                    }

                }
                else if (item.Name == "foldername")
                {
                    int x = 0;
                    string fName = item.Attributes["name"].Value.ToString();
                    sb.Append("\n");
                    sb.Append("<button class='accordion'>" + fName + "</button>");
                    sb.Append("\n");
                    sb.Append("<div class='panel'>");
                    x = l + 1;
                    string sChild = GetChild(item, ref x, part, ctrlName);
                    sb.Append(sChild);
                    sb.Append("\n");
                    sb.Append("</div>");
                    sb.Append("\n");
                }

            }

            return sb.ToString();
        }
        #endregion
    }
}
