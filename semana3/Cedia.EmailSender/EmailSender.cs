using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Cedia.Common.Helpers
{
    public class EmailSender
    {

        private const long MaxAttachmentsBytes = 25L * 1024 * 1024;
        private static double BytesToMB(long bytes) => bytes / 1024d / 1024d;
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
            List<string>? attachmentPaths = null,
            int timeoutMs = 100000)      
        {
            string traceId = Guid.NewGuid().ToString("N");
            var sw = Stopwatch.StartNew();

            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                };

                message.Headers.Add("X-Client-TraceId", traceId);
                message.Headers.Add("X-Client-TimeoutMs", timeoutMs.ToString());

                AddRecipients(message.To, toRecipients);
                AddRecipients(message.CC, ccRecipients);
                AddRecipients(message.Bcc, bccRecipients);
                var attCheck = ValidateAttachments(attachmentPaths, MaxAttachmentsBytes);
                if (!attCheck.ok)
                {
                    sw.Stop();
                    return $"{attCheck.error} | TraceId={traceId}";
                }
                AddAttachments(message, attachmentPaths);

#if NET48
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#elif NET8_0_OR_GREATER
           
#endif

                using var smtpClient = new SmtpClient
                {
                    Port = smtpPort,
                    Host = smtpHost,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    Timeout = timeoutMs 
                };

                smtpClient.Send(message);

                sw.Stop();
                return $"OK | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds}";
            }
            catch (SmtpFailedRecipientsException ex)
            {
                sw.Stop();
                var failedList = string.Join(", ", ex.InnerExceptions.Select(e => $"{e.FailedRecipient}({e.StatusCode})"));
                return $"ERROR (Varios destinatarios fallidos) | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Failed={failedList} | Message={ex.Message}";
            }
            catch (SmtpFailedRecipientException ex)
            {
                sw.Stop();
                return $"ERROR (Destinatario fallido) | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Recipient={ex.FailedRecipient} | StatusCode={ex.StatusCode} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex) when (IsTimeout(ex, sw.ElapsedMilliseconds, timeoutMs))
            {
                sw.Stop();
                return $"ERROR TIMEOUT | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | " +
                       $"StatusCode={ex.StatusCode} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex)
            {
                sw.Stop();
                return $"ERROR SMTP | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | " +
                       $"StatusCode={ex.StatusCode} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                sw.Stop();
                return $"ERROR General | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Type={ex.GetType().Name} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
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
            List<string>? attachmentPaths = null,
            int timeoutMs = 100000,
            string smtpHost = "smtp.office365.com", 
            int smtpPort = 587)
        {
            string traceId = Guid.NewGuid().ToString("N");
            var sw = Stopwatch.StartNew();

            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                };

                message.Headers.Add("X-Client-TraceId", traceId);
                message.Headers.Add("X-Client-TimeoutMs", timeoutMs.ToString());

                AddRecipients(message.To, toRecipients);
                AddRecipients(message.CC, ccRecipients);
                AddRecipients(message.Bcc, bccRecipients);
                var attCheck = ValidateAttachments(attachmentPaths, MaxAttachmentsBytes);
                if (!attCheck.ok)
                {
                    sw.Stop();
                    return $"{attCheck.error} | TraceId={traceId}";
                }
                AddAttachments(message, attachmentPaths);

#if NET48
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#elif NET8_0_OR_GREATER
               
#endif

                using var smtpClient = new SmtpClient
                {
                    Port = smtpPort,
                    Host = smtpHost,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, appPassword),
                    Timeout = timeoutMs
                };

                smtpClient.Send(message);

                sw.Stop();
                return $"OK | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds}";
            }
            catch (SmtpFailedRecipientsException ex)
            {
                sw.Stop();
                var failedList = string.Join(", ", ex.InnerExceptions.Select(e => $"{e.FailedRecipient}({e.StatusCode})"));
                return $"ERROR (Varios destinatarios fallidos) | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Failed={failedList} | Message={ex.Message}";
            }
            catch (SmtpFailedRecipientException ex)
            {
                sw.Stop();
                return $"ERROR (Destinatario fallido) | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Recipient={ex.FailedRecipient} | StatusCode={ex.StatusCode} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex) when (IsTimeout(ex, sw.ElapsedMilliseconds, timeoutMs))
            {
                sw.Stop();
                return $"ERROR TIMEOUT | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | " +
                       $"StatusCode={ex.StatusCode} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (SmtpException ex)
            {
                sw.Stop();
                return $"ERROR SMTP | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | " +
                       $"StatusCode={ex.StatusCode} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
            catch (Exception ex)
            {
                sw.Stop();
                return $"ERROR General | TraceId={traceId} | TimeoutMs={timeoutMs} | ElapsedMs={sw.ElapsedMilliseconds} | Type={ex.GetType().Name} | HResult={ex.HResult} | Message={ex.Message} | Inner={ex.InnerException?.Message}";
            }
        }

        private static void AddRecipients(MailAddressCollection collection, List<string>? recipients)
        {
            if (recipients == null) return;
            foreach (var email in recipients)
                if (!string.IsNullOrWhiteSpace(email))
                    collection.Add(email);
        }

        private static void AddAttachments(MailMessage message, List<string>? attachmentPaths)
        {
            if (attachmentPaths == null) return;
            foreach (var path in attachmentPaths)
                if (!string.IsNullOrWhiteSpace(path))
                    message.Attachments.Add(new Attachment(path));
        }

        private static bool IsTimeout(SmtpException ex, long elapsedMs, int timeoutMs)
        {
            if (elapsedMs >= Math.Max(0, timeoutMs - 5)) return true;

            if (ex.InnerException is WebException wex && wex.Status == WebExceptionStatus.Timeout) return true;

            var text = (ex.Message ?? "") + " " + (ex.InnerException?.Message ?? "");
            return text.IndexOf("timed out", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("tiempo de espera", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static long EstimateBase64Size(long bytes)
        {
            if (bytes <= 0) return 0;
            return ((bytes + 2) / 3) * 4;
        }



        private static (bool ok, string? error, long totalRaw, long totalEncoded) ValidateAttachments(List<string>? paths, long limitBytes)
        {
            if (paths == null || paths.Count == 0)
                return (true, null, 0, 0);

            long totalRaw = 0;
            foreach (var p in paths)
            {
                if (string.IsNullOrWhiteSpace(p)) continue;
                if (!File.Exists(p))
                    return (false, $"ERROR ATTACHMENT_NOT_FOUND | Path={p}", 0, 0);

                totalRaw += new FileInfo(p).Length;
            }

            if (totalRaw > limitBytes)
            {
                long enc = EstimateBase64Size(totalRaw);
                return (false, $"ERROR ATTACHMENTS_LIMIT | LimitMB=25.00 | TotalRawMB={BytesToMB(totalRaw):0.00} | EstEncodedMB≈{BytesToMB(enc):0.00}", totalRaw, enc);
            }

            return (true, null, totalRaw, EstimateBase64Size(totalRaw));
        }
    }
}
