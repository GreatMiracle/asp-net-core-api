using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WebApplication1.DTOs;

namespace WebApplication1.Services.Impl
{
    public class ThirdPartyApiServiceImpl : IThirdPartyApiService
    {
        private readonly HttpClient _httpClient;

        public ThirdPartyApiServiceImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<YourDataType>> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync("posts");

            response.EnsureSuccessStatusCode(); // Ném exception nếu không thành công

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<YourDataType>>(content);
        }
    }
}
