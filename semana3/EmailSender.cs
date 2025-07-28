using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Cedia.Common.Helpers
{
    public class EmailSender
    {
        public static string SendEmail(
            string senderEmail,
            string senderPassword,
            List<string> toRecipients,
            string subject,
            string body,
            string smtpHost,
            int smtpPort = 25,
            List<string>? ccRecipients = null,
            List<string>? bccRecipients = null,
            List<string>? attachmentPaths = null)
        {
            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    SubjectEncoding = Encoding.UTF8
                };

                AddRecipients(message.To, toRecipients);
                AddRecipients(message.CC, ccRecipients);
                AddRecipients(message.Bcc, bccRecipients);
                AddAttachments(message, attachmentPaths);

#if NET48
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#elif NET8_0_OR_GREATER
                
#endif

                using var smtpClient = new SmtpClient
                {
                    Port = smtpPort,
                    Host = smtpHost,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                smtpClient.Send(message);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        private static void AddRecipients(MailAddressCollection collection, List<string>? recipients)
        {
            if (recipients == null) return;

            foreach (var email in recipients)
            {
                if (!string.IsNullOrWhiteSpace(email))
                    collection.Add(email);
            }
        }

        private static void AddAttachments(MailMessage message, List<string>? attachmentPaths)
        {
            if (attachmentPaths == null) return;

            foreach (var path in attachmentPaths)
            {
                if (!string.IsNullOrWhiteSpace(path))
                    message.Attachments.Add(new Attachment(path));
            }
        }
    }
}
