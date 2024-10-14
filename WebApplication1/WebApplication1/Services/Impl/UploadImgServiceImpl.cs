using System.Web.Mvc;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.Infrastructure.Repositories;

namespace WebApplication1.Services.Impl
{
    public class UploadImgServiceImpl : IUploadImgService
    {
        private readonly IUploadImgRepository _uploadImgRepository;

        public UploadImgServiceImpl(IUploadImgRepository uploadImgRepository)
        {
            _uploadImgRepository = uploadImgRepository;
        }

        public async Task<Image> UploadImage(ImageUploadRequest request)
        {
            var image = new Image
            {
                File = request.File, 
                FileName = request.FileName,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileDescription = request.FileDescription
            };

            // Gọi phương thức upload từ repository
            return await _uploadImgRepository.UploadImg(image);
        }
    }
}
