using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;
using WebApplication1.Services;
using WebApplication1.Validators;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleDriveController : ControllerBase
    {
        private readonly IGoogleDriveService _googleDriveService;

        public GoogleDriveController(IGoogleDriveService googleDriveService)
        {
            _googleDriveService = googleDriveService;
        }

        // API để lấy danh sách các file từ Google Drive
        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _googleDriveService.GetFilesAsync();
            return Ok(files);
        }

        // API để tìm kiếm các file từ Google Drive theo điều kiện query
        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles([FromQuery] GoogleDriveSearchOptionsRequest request)
        {
            var validator = new SearchFileRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
             }

            var files = await _googleDriveService.SearchFilesAsync(request);
            return Ok(files);
        }

        //API để tải file lên Google Drive
       [HttpPost("upload")]
        public async Task<ActionResult<string>> Upload(IFormFile file, string folderId)
        {
            if (file.Length > 0)
            {
                // Lấy tên tệp từ đối tượng IFormFile
                var originalFileName = file.FileName;
                var filePath = Path.GetTempFileName(); // Temporary path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileId = await _googleDriveService.UploadFileAsync(filePath, file.ContentType, folderId, originalFileName);
                return Ok(fileId);
            }

            return BadRequest("File is empty");
        }

        [HttpGet("download/{fileId}")]
        public async Task<ActionResult> Download(string fileId)
        {
            var (stream, fileName, mimeType) = await _googleDriveService.DownloadFileAsync(fileId);
            return File(stream, mimeType, fileName);
        }

    }
}
