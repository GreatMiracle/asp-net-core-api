using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.Utils;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("test-encryption")]
        //[Authorize(Roles = "Reader")]
        [Authorize(Roles = "Writer,Reader")]
        public IActionResult TestEncryption()
        {
            string originalText = "your-sensitive-data"; // Dữ liệu cần mã hóa
            string encryptedText = EncryptionHelper.Encrypt(originalText); // Mã hóa
            string decryptedText = EncryptionHelper.Decrypt(encryptedText); // Giải mã

            return Ok(new
            {
                OriginalText = originalText,
                EncryptedText = encryptedText,
                DecryptedText = decryptedText
            });
        }
    }
}
