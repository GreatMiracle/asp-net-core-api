using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Data;

namespace WebApplication1.Infrastructure.Repositories.Impl
{
    public class UploadImgRepository : IUploadImgRepository
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly WalksDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadImgRepository(IWebHostEnvironment webHostEnvironment, WalksDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Image> UploadImg(Image image)
        {
            // Đường dẫn thư mục hình ảnh
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "ImageUp", image.FileName + image.FileExtension);
            // Tạo file stream để upload
            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                await image.File.CopyToAsync(stream);
            }

            image.FilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            // Lưu thông tin hình ảnh vào cơ sở dữ liệu
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return image;

        }
    }
}
