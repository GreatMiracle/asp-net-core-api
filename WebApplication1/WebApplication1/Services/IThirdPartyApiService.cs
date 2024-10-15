using Microsoft.Extensions.Hosting;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IThirdPartyApiService
    {
        Task<List<YourDataType>> GetPostsAsync();
    }
}
