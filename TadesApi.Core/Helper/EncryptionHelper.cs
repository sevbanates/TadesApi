using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TadesApi.Core.Helper
{
    public static class EncryptionHelper
    {
        public static string DecryptText(string text, string privateKey = "2909012565820034") //to do private key 
        {
            try
            {
                if (string.IsNullOrEmpty(text) || text == "null")
                    return string.Empty;

                using var provider = TripleDES.Create();
                provider.Key = Encoding.ASCII.GetBytes(privateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(privateKey.Substring(8, 8));

                var buffer = Convert.FromBase64String(text);
                return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
            }
            catch
            {
                //throw new InvalidTokenException();
                return text;
            }
        }

        public static string EncryptText(string text, string privateKey = "2909012565820034")
        {
            if (string.IsNullOrEmpty(text) || text == "null")
                return string.Empty;


            using var provider = TripleDES.Create();
            provider.Key = Encoding.ASCII.GetBytes(privateKey.Substring(0, 16));
            provider.IV = Encoding.ASCII.GetBytes(privateKey.Substring(8, 8));

            var encryptedBinary = EncryptTextToMemory(text, provider.Key, provider.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        private static byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, TripleDES.Create().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var toEncrypt = Encoding.Unicode.GetBytes(data);
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        private static string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream(data);
            using var cs = new CryptoStream(ms, TripleDES.Create().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }
    }
}
