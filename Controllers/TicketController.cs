using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using System.Text;
using System.Net.Mail;
using System.Web.Routing;
using System.Configuration;

namespace TCCCMS.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewSupportTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendTicket( string error, string description, List<HttpPostedFileBase> fileData)
        {
            TicketBL ticketBl = new TicketBL();
            bool isSendSuccessfully = false;
            Ticket ticket = new Ticket();
            if (Request.Files.Count > 0)
            {
                try
                {
                    string relativePath = "~/TicketFiles";
                    string path = Server.MapPath(relativePath);
                    //string fileFath = Path.Combine(path, categoryName);
                    StringBuilder mailBody = new StringBuilder();
                    string senderEmail = string.Empty;
                    string fileFath = path;
                    if (!Directory.Exists(fileFath))
                    {
                        Directory.CreateDirectory(fileFath);
                    }
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
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

                        // Get the complete folder path and store the file inside it.  
                        //string fnameWithPath = Path.Combine(fileFath, fname);
                        fname = DateTime.Now.ToString("yyyyMMddhhmm")+"_"+fname;
                        string fnameWithPath = fileFath +"/"+ fname;
                        file.SaveAs(fnameWithPath);

                        ticket.Error        = error;
                        ticket.Description  = description;
                        ticket.FilePath     = relativePath+"/"+fname;
                        ticket.ShipId       = Convert.ToInt32(Session["ShipId"].ToString());


                    }

                    string ticketNumber = ticketBl.SaveTicket(ticket, Convert.ToInt32(Session["UserType"].ToString()));
                    if(ticketNumber != null)
                    {
                        if(Session["UserType"].ToString() == "1")
                        {
                            senderEmail= ConfigurationManager.AppSettings["shipEmail"];
                        }
                        else
                        {
                            senderEmail = Session["Email"].ToString();
                        }
                        MailMessage mail = new MailMessage();
                        
                            mailBody.Append("Ticket Number : ");
                            mailBody.Append(ticketNumber.ToString());
                            mailBody.Append("\n");
                            mailBody.Append("Error Message : ");
                            mailBody.Append(error);
                            mailBody.Append("\n");
                            mailBody.Append("Error Description : ");
                            mailBody.Append(description);
                            mailBody.Append("\n");

                        mail.Body = mailBody.ToString();
                            if (ticket.FilePath != null || ticket.FilePath != "")
                            {
                                mail.Attachments.Add(new Attachment(Server.MapPath(ticket.FilePath)));
                            }
                        


                        if (!String.IsNullOrEmpty(senderEmail))
                        {
                            //SendEmail.SendMail("Ticket", senderEmail, "tcccms2021@gmail.com", ticketNumber.ToString(), error, description, Server.MapPath(ticket.FilePath));

                            SendEmail.SendMail("Ticket", senderEmail, mail,ref isSendSuccessfully);
                        }
                        //SendEmail.SendMail("Ticket", "cableman24x7@Gmail.com", "tcccms2021@gmail.com", ticketNumber.ToString(), error, description, Server.MapPath(ticket.FilePath));
                        //SendEmail.SendMail("Ticket", "tcccms2021@Gmail.com", Session["Email"].ToString(), ticketNumber.ToString(), error, description, Server.MapPath(ticket.FilePath));
                    }
                    // Returns message that successfully uploaded  
                    return Json(ticketNumber, JsonRequestBehavior.AllowGet);
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


        public JsonResult LoadData()
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
                length = 500;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            TicketBL bL = new TicketBL(); 
            int totalrecords = 0;

            List<Ticket> pocoList = new List<Ticket>();
            pocoList = bL.GetAllTicketPageWise(pageIndex, ref totalrecords, length);
            List<Ticket> pList = new List<Ticket>();
            foreach (Ticket pC in pocoList)
            {
                Ticket pOCO = new Ticket();
                //pOCO.Id = pC.Id;
                pOCO.TicketNumber = pC.TicketNumber;
                pOCO.Error = pC.Error;
                pOCO.Description = pC.Description;
                pOCO.IsSolved = pC.IsSolved;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}