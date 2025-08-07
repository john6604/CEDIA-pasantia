using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cedia.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Newtonsoft.Json;

namespace Cedia.Common.Helpers
{
    public class PdfSignatureHelper
    {
        public static List<PdfCertificate> ReadSignaturesFromPdf(string pdfPath)
        {
            var certificates = new List<PdfCertificate>();

            try
            {
                using (var reader = new PdfReader(pdfPath))
                {
                    var fields = reader.AcroFields;
                    var signatureNames = fields.GetSignatureNames();

                    foreach (var name in signatureNames)
                    {
                        try
                        {
                            var pkcs7 = fields.VerifySignature(name);

                            var cert = new PdfCertificate
                            {
                                Reason = pkcs7.Reason,
                                Location = pkcs7.Location,
                                Name = !string.IsNullOrWhiteSpace(pkcs7.SignName)
                                    ? ExtractCommonName(pkcs7.SignName)
                                    : ExtractCommonName(pkcs7.SigningCertificate.SubjectDN?.ToString() ?? ""),
                                SignedDate = pkcs7.SignDate.ToUniversalTime(),
                                ValidFrom = pkcs7.SigningCertificate.NotBefore,
                                ValidTo = pkcs7.SigningCertificate.NotAfter,
                                Subject = pkcs7.SigningCertificate.SubjectDN.ToString(),
                                Issuer = pkcs7.SigningCertificate.IssuerDN.ToString(),
                                SignatureOrigin = pkcs7.SigningCertificate.IssuerDN.ToString()
                                    .IndexOf("DC=cedia", StringComparison.OrdinalIgnoreCase) >= 0
                                    ? "Interna CEDIA"
                                    : "Externo"
                            };

                            certificates.Add(cert);
                        }
                        catch (Exception exInner)
                        {
                            Console.WriteLine($"[ERROR] Processing signature '{name}': {exInner.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Reading PDF signatures: {ex.Message}");
            }

            return certificates;
        }

        private static string ExtractCommonName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var match = Regex.Match(input, @"CN=([^,]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public static string ReadSignaturesAsJson(string pdfPath)
        {
            try
            {
                var certificates = ReadSignaturesFromPdf(pdfPath);
                return JsonConvert.SerializeObject(certificates);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Serializing PDF certificates: {ex.Message}");
                return "[]";
            }
        }
    }
}
