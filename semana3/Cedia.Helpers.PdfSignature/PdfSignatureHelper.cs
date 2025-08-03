using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cedia.Common.Models;
using iTextSharp.text.pdf;

#if NET48
using iTextSharp.text.pdf.security;
#elif NET8_0_OR_GREATER
using BitMiracle.Docotic.Pdf;
#endif

namespace Cedia.Common.Helpers
{
    public static class PdfSignatureHelper
    {
        public static List<PdfCertificate> ReadSignaturesFromPdf(string pdfPath)
        {
#if NET48
            var certificates = new List<PdfCertificate>();

            try
            {
                using var reader = new PdfReader(pdfPath);
                var fields = reader.AcroFields;
                var signatureNames = fields.GetSignatureNames();

                foreach (var name in signatureNames)
                {
                    var pkcs7 = fields.VerifySignature(name);
                    var cert = new PdfCertificate
                    {
                        Reason = pkcs7.Reason,
                        Location = pkcs7.Location,
                        Name = ExtractCommonName(pkcs7.SignName) ?? ExtractCommonName(pkcs7.SigningCertificate.SubjectDN.ToString()),
                        SignedDate = pkcs7.SignDate.ToUniversalTime(),
                        ValidFrom = pkcs7.SigningCertificate.NotBefore,
                        ValidTo = pkcs7.SigningCertificate.NotAfter,
                        Subject = pkcs7.SigningCertificate.SubjectDN.ToString(),
                        Issuer = pkcs7.SigningCertificate.IssuerDN.ToString(),
                        SignatureOrigin = pkcs7.SigningCertificate.IssuerDN.ToString().IndexOf("DC=cedia", StringComparison.OrdinalIgnoreCase) >= 0
                            ? "CEDIA Internal"
                            : "External"
                    };

                    certificates.Add(cert);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Reading PDF signatures: {ex.Message}");
            }

            return certificates;
#else
            var certificates = new List<PdfCertificate>();
            try
            {
                using var reader = new PdfReader(pdfPath);
                var fields = reader.AcroFields;
                foreach (var name in fields.GetSignatureNames())
                {
                    var pkcs7 = fields.VerifySignature(name);
                    certificates.Add(new PdfCertificate
                    {
                        Reason = pkcs7.Reason,
                        Location = pkcs7.Location,
                        Name = ExtractCommonName(pkcs7.SignName)
                               ?? ExtractCommonName(pkcs7.SigningCertificate.SubjectDN.ToString()),
                        SignedDate = pkcs7.SignDate.ToUniversalTime(),
                        ValidFrom = pkcs7.SigningCertificate.NotBefore,
                        ValidTo = pkcs7.SigningCertificate.NotAfter,
                        Subject = pkcs7.SigningCertificate.SubjectDN.ToString(),
                        Issuer = pkcs7.SigningCertificate.IssuerDN.ToString(),
                        SignatureOrigin = (pkcs7.SigningCertificate.IssuerDN.ToString()
                            .IndexOf("DC=cedia", StringComparison.OrdinalIgnoreCase) >= 0)
                            ? "CEDIA Internal"
                            : "External"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR - iTextSharp] {ex.Message}");
            }
            return certificates;
#endif
        }

        private static string? ExtractCommonName(string input)
        {
            var match = Regex.Match(input, @"CN=([^,]+)");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
