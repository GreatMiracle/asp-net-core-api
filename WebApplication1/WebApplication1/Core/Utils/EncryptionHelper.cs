using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Core.Utils
{
    public class EncryptionHelper
    {
        // Khóa và IV cần được lưu trữ và bảo vệ một cách an toàn
        private static readonly string key = Environment.GetEnvironmentVariable("AES_KEY"); // 32 ký tự cho AES-256
        private static readonly string iv = Environment.GetEnvironmentVariable("AES_IV"); // 16 ký tự cho AES

        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

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
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

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
