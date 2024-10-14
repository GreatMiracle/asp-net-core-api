using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Services
{
    public interface IUploadImgService
    {
        Task<Image> UploadImage(ImageUploadRequest request);
    }
}
