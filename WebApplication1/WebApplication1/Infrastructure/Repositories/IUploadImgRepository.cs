using WebApplication1.Core.Entities;

namespace WebApplication1.Infrastructure.Repositories
{
    public interface IUploadImgRepository
    {
        Task<Image> UploadImg(Image image);
    }
}
