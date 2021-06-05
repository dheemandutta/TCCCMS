using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Configuration;

namespace TCCCMS.Controllers
{
	public class SendEmail
	{
		public static bool isMailSendSuccessful = false;
		public static void SendMail(string subject, string senderEmail, string receiverEmail,string ticketNo, string errorMsg, string errorDesc, string errorImg)
		{

			try
			{
				string smtpEmail = ConfigurationManager.AppSettings["smtpEmail"];
				string smtpEmailPwd = ConfigurationManager.AppSettings["smtpEmailPwd"];
				string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
				int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
				string supportEmail = ConfigurationManager.AppSettings["supportEmail"];

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
					mail.To.Add(supportEmail);
					//mail.CC.Add(receiverEmail);
					//mail.CC.Add(supportEmail);

					mail.Subject = subject;
					mail.Body = mailBody.ToString();

					if (errorImg != null || errorImg != "")
					{
						mail.Attachments.Add(new Attachment(errorImg));
					}

					//SmtpClient smtp = new SmtpClient("smtp.gmail.com");
					//smtp.EnableSsl = true;
					//smtp.Port = 587;
					////smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "tcccms202112345");
					//smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "cableman24x712345");

					SmtpClient smtp = new SmtpClient(smtpServer);
					smtp.EnableSsl = true;
					smtp.Port = smtpPort;
					//smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "tcccms202112345");
					smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpEmailPwd);

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

		public static void SendMail(string subject, string senderEmail, MailMessage mailBodyMsg,ref bool isSendSuccessfully)
		{

			try
			{
				string smtpEmail		= ConfigurationManager.AppSettings["smtpEmail"];
				string smtpEmailPwd		= ConfigurationManager.AppSettings["smtpEmailPwd"];
				string smtpServer		= ConfigurationManager.AppSettings["smtpServer"];
				int smtpPort			= Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
				string supportEmail		= ConfigurationManager.AppSettings["supportEmail"];
				StringBuilder mailBody	= new StringBuilder();
				MailMessage mail		= new MailMessage();
				mail.From = new MailAddress(smtpEmail);
				mail.To.Add(supportEmail);


				//mail.From = new MailAddress(senderEmail);
                //mail.To.Add(supportEmail);

                //mail.From = new MailAddress(supportEmail);
                //mail.To.Add(senderEmail);
                //mail.From = new MailAddress("b_bingshu@yahoo.in");
                //mail.To.Add("tcccms2021@gmail.com");
                mail.Subject	= subject;
				mail			= mailBodyMsg;
				//SmtpClient smtp = new SmtpClient("smtp.gmail.com");
				//smtp.EnableSsl = true;
				//smtp.Port = 587;
				//smtp.UseDefaultCredentials = false;
				////smtp.Credentials = new System.Net.NetworkCredential(senderEmail, "tcccms202112345");
				//smtp.Credentials = new System.Net.NetworkCredential("cableman24x7@gmail.com", "cableman24x712345");
				SmtpClient smtp				= new SmtpClient(smtpServer);
                smtp.EnableSsl				= true;
                smtp.Port					= smtpPort;
                smtp.UseDefaultCredentials	= false;
                smtp.DeliveryMethod			= SmtpDeliveryMethod.Network;
                smtp.Credentials			= new System.Net.NetworkCredential(smtpEmail, smtpEmailPwd);

                smtp.Send(mail);
				isMailSendSuccessful = true;
				isSendSuccessfully = true;

			}
			catch (Exception ex)
			{
				//EventLog.WriteEntry("DataExport-SendMail", ex.Message + " :" + ex.InnerException, EventLogEntryType.Error);
				isMailSendSuccessful = false;
				isSendSuccessfully = false;

			}

		}
	}
}