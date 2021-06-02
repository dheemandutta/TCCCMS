using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

using System.Threading;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Search;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.Collections.Generic;

namespace TCCCMS.Admin.ImportData
{
    public static class MailType
    {
        public const string IMAP = "IMAP";
        public const string POP3 = "POP3";
    }

    public class MailOps : IDisposable
    {

        public string MailServerType { get; set; }
        public string AttachmentFolderPath { get; set; }

        string _msgindexset = "MsgIndexSet";
        ImapClient imap_Client;
        Pop3Client pop_Client;


        #region Using MailKit
        public void Connect(string userId, string password, string mailDomain, int port)
        {
            try
            {
                if (MailServerType == MailType.POP3)
                {
                    using (pop_Client = new Pop3Client(new ProtocolLogger("pop3.log")))
                    {
                        using (var cancel = new CancellationTokenSource())
                        {
                            pop_Client.Connect(mailDomain, port, true, cancel.Token);
                            pop_Client.AuthenticationMechanisms.Remove("XOAUTH2");
                            pop_Client.AuthenticationMechanisms.Remove("NTLM");
                            pop_Client.AuthenticationMechanisms.Remove("PLAIN");
                            pop_Client.Authenticate(userId, password, cancel.Token);
                        }
                    }
                }
                else if (MailServerType == MailType.IMAP)
                {
                    imap_Client = new ImapClient(new ProtocolLogger("imap.log"));
                    imap_Client.Connect(mailDomain, port, true);
                    imap_Client.AuthenticationMechanisms.Remove("XOAUTH2");
                    imap_Client.AuthenticationMechanisms.Remove("NTLM");
                    imap_Client.AuthenticationMechanisms.Remove("PLAIN");
                    imap_Client.Authenticate(userId, password);


                }
                else
                {
                    Console.WriteLine(MailServerType + " unsupported mail server type");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.EventLog, ex.Message);
            }

        }
        public void DownloadAllNewMails(string subjectLine, string attachmentFolderPath)
        {

            try
            {
                if (MailServerType == MailType.POP3)
                {

                    for (int i = 0; i < pop_Client.Count; i++)
                    {
                        MimeMessage message = pop_Client.GetMessage(i);
                        if (message.Subject.Equals(subjectLine))
                        {
                            foreach (var attachment in message.Attachments)
                            {
                                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                                string filePath = Path.Combine(attachmentFolderPath, fileName);
                                using (var stream = File.Create(filePath))
                                {
                                    if (attachment is MessagePart)
                                    {
                                        var part = (MessagePart)attachment;

                                        part.Message.WriteTo(stream);
                                    }
                                    else
                                    {
                                        var part = (MimePart)attachment;

                                        part.Content.DecodeTo(stream);
                                    }
                                }
                            }
                            pop_Client.DeleteMessage(i);
                        }

                    }



                }
                else if (MailServerType == MailType.IMAP)
                {
                    var inbox = imap_Client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    IList<UniqueId> uids = inbox.Search(SearchQuery.NotSeen);
                    foreach (UniqueId uid in uids)
                    {

                        MimeMessage message = inbox.GetMessage(uid);
                        if (message.Subject.Equals(subjectLine))
                        {
                            foreach (MimeEntity attach in message.Attachments)
                            {
                                var fileName = attach.ContentDisposition?.FileName ?? attach.ContentType.Name;
                                string filePath = Path.Combine(attachmentFolderPath, fileName);
                                using (var streem = File.Create(filePath))
                                {
                                    if (attach is MessagePart)
                                    {
                                        var part = (MessagePart)attach;
                                        part.Message.WriteTo(streem);
                                    }
                                    else
                                    {
                                        var part = (MimePart)attach;
                                        part.Content.DecodeTo(streem);

                                    }
                                }


                            }
                            inbox.AddFlags(uid, MessageFlags.Deleted, true);
                            inbox.Expunge();
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.EventLog, ex.Message);
            }
            finally
            {
                if (MailServerType == MailType.POP3)
                {
                    //pop.Disconnect();
                    pop_Client.Disconnect(true);

                }
                else
                {
                    //imap.Disconnect();
                    imap_Client.Disconnect(true);
                }
                // Dispose();
            }
            if (MailServerType == MailType.POP3)
            {
                //pop.Disconnect();
                pop_Client.Disconnect(true);

            }
            else
            {
                //imap.Disconnect();
                imap_Client.Disconnect(true);
            }


        }

        #endregion

        public void Dispose()
        {
            //pop.Dispose();
            //imap.Dispose();

            imap_Client.Dispose();
            pop_Client.Dispose();

        }

    }
}
