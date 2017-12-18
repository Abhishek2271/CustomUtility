using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace CustomUtility.EmailSender
{
    public class EmailSender
    {
        private ISmtpServer server = null;
        public EmailAddress From = null;
        public IList<EmailAddress> To { get; set; }
        public IList<EmailAddress> Cc { get; set; }
        public IList<EmailAddress> Bcc { get; set; }
        public string EmailSubject { get; set; }
        public string Body { get; set; }

        public EmailSender(ISmtpServer server, EmailAddress _from, IList<EmailAddress> to)
        {
            this.server = server;
            From = _from;
            To = to;
        }

        public void SendEmail()
        {
            if (From == null || To == null || server == null)
                throw new Exception("Please make sure that all required fields are set properly.");

            SmtpClient smtp_client = GetConfiguredSmtpClient();

            List<MailMessage> msgList = GetMailMessageList();
            foreach (MailMessage msg in msgList)
            {
                try
                {
                    //MailMessage msg = GetMailMessage();
                    smtp_client.Send(msg);
                }
                catch (Exception e)
                {
                    //TODO:- need to handle exception 

                }
            }
        }

        public void SendTestEmail()
        {
            if (From == null || To == null || server == null)
                throw new Exception("Some parameters are not set properly");

            SmtpClient smtp_client = GetConfiguredSmtpClient();

            List<MailMessage> msgList = GetMailMessageList();

            foreach (MailMessage msg in msgList)
            {
                try
                {
                    smtp_client.Send(msg);
                }
                catch (ArgumentNullException argNullException)
                {
                    throw new Exception("Body of the Email is null.");
                }
                catch (InvalidOperationException invalidOpException)
                {
                    throw new Exception("Either Sender and/or Recipient for this Email is not specified.");
                }
                catch (SmtpFailedRecipientsException failedRecipientException)
                {
                    throw new Exception("The Test Email couldnot be delivered to specified recipients. The recipients email address maynot be valid. Exception thrown is:" + failedRecipientException.Message);
                }
                catch (SmtpException smtpException)
                {
                    throw new Exception(smtpException.Message);

                }
                catch (Exception e)
                {
                    throw e;
                }                
            }
        }

        private SmtpClient GetConfiguredSmtpClient()
        {
            SmtpClient smtp_client = new SmtpClient(server.Server, server.Port);

            if (server.RequireAuthentication)
                smtp_client.Credentials = new System.Net.NetworkCredential(server.Username, server.Password);
            else
                smtp_client.UseDefaultCredentials = true;

            smtp_client.EnableSsl = server.RequireSSL;

            return smtp_client;
        }

        /// <summary>
        /// Create a list so that each mail can be sent individually
        /// </summary>
        /// <returns>Mailing list</returns>
        private List<MailMessage> GetMailMessageList()
        {
            //MailMessage msg = new MailMessage();
            //msg.From = From.GetMailAddress();

            //foreach (EmailAddress eml in To)
            //{
            //    msg.To.Add(eml.GetMailAddress());
            //}

            //if (Cc != null)
            //{
            //    foreach (EmailAddress eml in Cc)
            //    {
            //        msg.CC.Add(eml.GetMailAddress());
            //    }
            //}

            //if (Bcc != null)
            //{
            //    foreach (EmailAddress eml in Bcc)
            //    {
            //        msg.Bcc.Add(eml.GetMailAddress());
            //    }
            //}

            List<MailMessage> msgList = new List<MailMessage>();
            foreach (EmailAddress eml in To)
            {
                MailMessage msg = new MailMessage();
                msg.From = From.GetMailAddress();
                msg.To.Add(eml.GetMailAddress());
                msg.Subject = EmailSubject;
                msg.SubjectEncoding = Encoding.UTF8;

                msg.Body = Body;
                msg.BodyEncoding = Encoding.UTF8;
                msgList.Add(msg);
            }

            //if (Cc != null)
            //{
            //    foreach (EmailAddress eml in Cc)
            //    {
            //        msg.CC.Add(eml.GetMailAddress());
            //    }
            //}

            //if (Bcc != null)
            //{
            //    foreach (EmailAddress eml in Bcc)
            //    {
            //        msg.Bcc.Add(eml.GetMailAddress());
            //    }
            //}



            //msg.Subject = EmailSubject;
            //msg.SubjectEncoding = Encoding.UTF8;

            //msg.Body = Body;
            //msg.BodyEncoding = Encoding.UTF8;
            //msg.IsBodyHtml = IsBodyHTML;

            return msgList;
        }


    }
}

