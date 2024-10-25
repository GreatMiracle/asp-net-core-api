using WebApplication1.Core.Utils;
using System;
namespace WebApplication1.Test.ENC
{
    public class EncryptionEnv
    {
        public static void Main()
        {
            string originalText = "your-sensitive-data"; // Dữ liệu cần mã hóa
            string encryptedText = EncryptionHelper.Encrypt(originalText); // Mã hóa
            string decryptedText = EncryptionHelper.Decrypt(encryptedText); // Giải mã

            Console.WriteLine($"Original Text: {originalText}");
            Console.WriteLine($"Encrypted Text: {encryptedText}");
            Console.WriteLine($"Decrypted Text: {decryptedText}");
        }
    }
}
