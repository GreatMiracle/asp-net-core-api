using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IUploadImgService _uploadImgService;
        public ImagesController(IUploadImgService uploadImgService)
        {
            _uploadImgService = uploadImgService;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequest request)
        {
            
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            // Kiểm tra phần mở rộng tệp
            var extension = Path.GetExtension(request.File.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            // Kiểm tra kích thước tệp
            if (request.File.Length > 10485760) // 10 MB
            {
                ModelState.AddModelError("file", "File size must be less than 10 MB");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var img = await _uploadImgService.UploadImage(request);
            return Ok(img);
        }
    }
}
