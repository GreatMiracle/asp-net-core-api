using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.Utils;
using WebApplication1.Middleware;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger; // Tiêm logger qua Dependency Injection
        }

        [HttpGet("test-encryption")]
        [Authorize(Roles = "Writer,Reader")]
        public IActionResult TestEncryption(string text)
        {
            throw new CustomException("Resource not found", 404);
            //throw new Exception("This is a test exception.");
            //_logger.LogInformation("test-encryption action method was invoked.");

            string originalText = text.Trim(); // Dữ liệu cần mã hóa
            string encryptedText = EncryptionHelper.Encrypt(originalText); // Mã hóa
            string decryptedText = EncryptionHelper.Decrypt(encryptedText); // Giải mã

            return Ok(new
            {
                OriginalText = originalText,
                EncryptedText = encryptedText,
                DecryptedText = decryptedText
            });
        }

        [HttpGet("to-base64")]
        public IActionResult ConvertToBase64([FromQuery] string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input string is required.");
            }
            try
            {
                // Chuyển đổi chuỗi input sang mảng byte
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(input);

                // Chuyển đổi mảng byte sang chuỗi Base64
                string base64 = Convert.ToBase64String(textBytes);

                return Ok(new
                {
                    OriginalString = input,
                    Base64String = base64
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("from-base64")]
        public IActionResult ConvertFromBase64([FromQuery] string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input Base64 string is required.");
            }

            try
            {
                // Chuyển đổi chuỗi Base64 sang mảng byte
                byte[] base64Bytes = Convert.FromBase64String(input);

                // Chuyển đổi mảng byte sang chuỗi UTF8
                string decodedText = System.Text.Encoding.UTF8.GetString(base64Bytes);

                return Ok(new
                {
                    Base64String = input,
                    DecodedString = decodedText
                });
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
