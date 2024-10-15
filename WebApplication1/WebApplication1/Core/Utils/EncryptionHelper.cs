using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Core.Utils
{
    public class EncryptionHelper
    {
        //key và iv không mã hóa sang base64
        //private static readonly string key = Environment.GetEnvironmentVariable("AES_KEY"); // 32 ký tự cho AES-256
        //private static readonly string iv = Environment.GetEnvironmentVariable("AES_IV"); // 16 ký tự cho AES

        private static readonly byte[] key;
        private static readonly byte[] iv;

        static EncryptionHelper()
        {
            var base64Key = Environment.GetEnvironmentVariable("AES_KEY");
            var base64IV = Environment.GetEnvironmentVariable("AES_IV");

            if (string.IsNullOrEmpty(base64Key) || string.IsNullOrEmpty(base64IV))
            {
                throw new InvalidOperationException("AES_KEY or AES_IV environment variable is not set.");
            }

            key = Convert.FromBase64String(base64Key);
            iv = Convert.FromBase64String(base64IV);

            if (key.Length != 32) // AES-256 yêu cầu key 32 bytes
            {
                throw new InvalidOperationException("AES key is invalid.");
            }

            if (iv.Length != 16) // IV cần 16 bytes cho AES
            {
                throw new InvalidOperationException("AES IV is invalid.");
            }
        }

        public static string Encrypt(string plainText)
        {
            //key và iv không mã hóa sang base64
            //if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));

            //using (Aes aes = Aes.Create())
            //{
            //    aes.Key = Encoding.UTF8.GetBytes(key);
            //    aes.IV = Encoding.UTF8.GetBytes(iv);

            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            //key và iv không mã hóa sang base64
            //if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException(nameof(encryptedText));

            //using (Aes aes = Aes.Create())
            //{
            //    aes.Key = Encoding.UTF8.GetBytes(key);
            //    aes.IV = Encoding.UTF8.GetBytes(iv);

            if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException(nameof(encryptedText));
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
