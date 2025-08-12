using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            string traceId = Guid.NewGuid().ToString("N");

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

                using var smtpClient = new SmtpClient
                {
                    Port = smtpPort,
                    Host = smtpHost,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    Timeout = 100000
                };

                Console.WriteLine($"[{traceId}] Iniciando envío de email: Host={smtpHost}, Puerto={smtpPort}, SSL={smtpClient.EnableSsl}, " +
                                  $"From={senderEmail}, To={message.To.Count}, CC={message.CC.Count}, BCC={message.Bcc.Count}, Attach={message.Attachments.Count}");

                smtpClient.Send(message);

                return $"OK | TraceId={traceId}";
            }
            catch (SmtpFailedRecipientsException ex)
            {
                var failedList = string.Join(", ", ex.InnerExceptions.Select(e => $"{e.FailedRecipient}({e.StatusCode})"));
                return $"ERROR (Varios destinatarios fallidos) | TraceId={traceId} | Failed={failedList} | Message={ex.Message}";
            }
            catch (SmtpFailedRecipientException ex)
            {
                
                return $"ERROR (Destinatario fallido) | TraceId={traceId} | Recipient={ex.FailedRecipient} | StatusCode={ex.StatusCode} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex)
            {
                return $"ERROR SMTP | TraceId={traceId} | StatusCode={ex.StatusCode} | HResult={ex.HResult} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                return $"ERROR General | TraceId={traceId} | Type={ex.GetType().Name} | HResult={ex.HResult} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message} | StackTrace={ex.StackTrace}";
            }
        }


        public static string SendEmailWithMicrosoftToken(
            string senderEmail,
            string appPassword,
            List<string> toRecipients,
            string subject,
            string body,
            List<string>? ccRecipients = null,
            List<string>? bccRecipients = null,
            List<string>? attachmentPaths = null)
        {
            string traceId = Guid.NewGuid().ToString("N");
            const string smtpHost = "smtp.office365.com";
            const int smtpPort = 587;

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
                    Credentials = new NetworkCredential(senderEmail, appPassword)
                };

                smtpClient.Send(message);
                return "OK";
            }
            catch (SmtpFailedRecipientsException ex)
            {
                var failedList = string.Join(", ", ex.InnerExceptions.Select(e => $"{e.FailedRecipient}({e.StatusCode})"));
                return $"ERROR (Varios destinatarios fallidos) | TraceId={traceId} | Failed={failedList} | Message={ex.Message}";
            }
            catch (SmtpFailedRecipientException ex)
            {

                return $"ERROR (Destinatario fallido) | TraceId={traceId} | Recipient={ex.FailedRecipient} | StatusCode={ex.StatusCode} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex)
            {
                return $"ERROR SMTP | TraceId={traceId} | StatusCode={ex.StatusCode} | HResult={ex.HResult} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                return $"ERROR General | TraceId={traceId} | Type={ex.GetType().Name} | HResult={ex.HResult} | " +
                       $"Message={ex.Message} | Inner={ex.InnerException?.Message} | StackTrace={ex.StackTrace}";
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
