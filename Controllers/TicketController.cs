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
using System.Web.Routing;

namespace TCCCMS.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendTicket( string error, string description, List<HttpPostedFileBase> fileData)
        {
            TicketBL ticketBl = new TicketBL();
            Ticket ticket = new Ticket();
            if (Request.Files.Count > 0)
            {
                try
                {
                    string relativePath = "~/TicketFiles";
                    string path = Server.MapPath(relativePath);
                    //string fileFath = Path.Combine(path, categoryName);
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

                        ticket.Error = error;
                        ticket.Description = description;
                        ticket.FilePath = relativePath+"/"+fname;


                    }

                    int ticketNumber = ticketBl.SaveTicket(ticket);
                    if(ticketNumber > 0)
                    {
                        SendEmail.SendMail("Ticket","cableman24x7@Gmail.com", "tcccms2021@gmail.com", ticketNumber.ToString(), error, description,Server.MapPath(ticket.FilePath));
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

    }
}