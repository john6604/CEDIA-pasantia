using System;
using System.Security.Cryptography;
using System.Text;

#if NET48
using System.Web;
#else
using System.Net;
#endif

namespace Cedia.Common.Helpers
{
    public static class UrlEncryptionHelper
    {
        private const string DefaultSiteKeyValue = "clave-secreta-site";

        public static string GetSiteKey()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(DefaultSiteKeyValue));
        }

        public static string GenerateChecksum(string input, int length)
        {
            using var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString().Substring(0, length);
        }

        public static string EncryptToBase64Url(string plainText, string key)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using var aes = Aes.Create();
            using (var sha256 = SHA256.Create())
            {
                aes.Key = sha256.ComputeHash(keyBytes).AsSpan(0, 16).ToArray();
            }
            aes.IV = new byte[16];

            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);

            return UrlEncodeBase64(Convert.ToBase64String(encryptedBytes));
        }

        public static string GenerateSecureLink(string baseUrl, string route, string code, string siteKey)
        {
            string encryptedInput = route.Trim() + UrlEncodeBase64(code);
            string checksum = GenerateChecksum(encryptedInput, 6);
            string fullText = encryptedInput + checksum;
            string token = EncryptToBase64Url(fullText, siteKey);

            return $"{baseUrl.TrimEnd('/')}/{route}?{token}";
        }

        private static string UrlEncodeBase64(string input)
        {
#if NET48
    return System.Web.HttpUtility.UrlEncode(input);
#else
            return System.Net.WebUtility.UrlEncode(input);
#endif
        }
    }
}
