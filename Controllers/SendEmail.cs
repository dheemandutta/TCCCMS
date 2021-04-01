﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace TCCCMS.Controllers
{
	public class SendEmail
	{
		public static bool isMailSendSuccessful = false;
		public static void SendMail(string subject, string senderEmail, string receiverEmail,string ticketNo, string errorMsg, string errorDesc, string errorImg)
		{

			try
			{
				StringBuilder mailBody = new StringBuilder();
				using (MailMessage mail = new MailMessage())
				{
					mailBody.Append("Ticket Number : ");
					mailBody.Append(ticketNo);
					mailBody.Append("\n");
					mailBody.Append("Error Message : ");
					mailBody.Append(errorMsg);
					mailBody.Append("\n");
					mailBody.Append("Error Description : ");
					mailBody.Append(errorDesc);
					mailBody.Append("\n");


					mail.From = new MailAddress(senderEmail);
					mail.To.Add(senderEmail);
					mail.CC.Add(receiverEmail);

					mail.Subject = subject;
					mail.Body = mailBody.ToString();

					if (errorImg != null || errorImg != "")
					{
						mail.Attachments.Add(new Attachment(errorImg));
					}

					SmtpClient smtp = new SmtpClient("smtp.gmail.com");
					smtp.EnableSsl = true;
					smtp.Port = 587;
					//smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "tcccms202112345");
					smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "cableman24x712345");

					smtp.Send(mail);

					isMailSendSuccessful = true;
				}
			}
			catch (Exception ex)
			{
				//EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
				isMailSendSuccessful = false;

			}

		}
	}
}