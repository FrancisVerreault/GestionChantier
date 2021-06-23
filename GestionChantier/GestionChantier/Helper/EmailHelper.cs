using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace GestionChantier.Helper
{
    public static class EmailHelper
    {
        public static void SendEmailWithAttachmentForLoutec(string fromEmail, string[] ccs, string attachmentFilePath, string subject)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Port = 25;
            SmtpServer.Host = "exchange.bruneau.local";
            SmtpServer.EnableSsl = false;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential("pricer_user@bruneauelectrique.com", "Pr1c3r!");

            mail.From = new MailAddress(fromEmail);

            mail.To.Add("commande.loutec@bruneauelectrique.com");

            foreach (string cc in ccs)
                mail.CC.Add(cc);

            mail.Subject = subject;

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(attachmentFilePath);
            mail.Attachments.Add(attachment);

            SmtpServer.Send(mail);
        }
        public static void SendEmailWithAttachment(string[] fromEmail, string attachmentFilePath, string subject)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Port = 25;
            SmtpServer.Host = "exchange.bruneau.local";
            SmtpServer.EnableSsl = false;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential("pricer_user@bruneauelectrique.com", "Pr1c3r!");

            mail.From = new MailAddress(fromEmail[0]);

            foreach (string email in fromEmail)
                mail.CC.Add(email);
            mail.Subject = subject;

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(attachmentFilePath);
            mail.Attachments.Add(attachment);

            SmtpServer.Send(mail);
        }
    }
}