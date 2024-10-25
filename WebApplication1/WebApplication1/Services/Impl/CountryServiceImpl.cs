using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using WebApplication1.DTOs;

namespace WebApplication1.Services.Impl
{
    public class CountryServiceImpl : ICountryService
    {

        private readonly HttpClient _httpClient;

        public CountryServiceImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Phương thức để lấy tất cả các quốc gia từ REST Countries API
        public async Task<List<CountryDTO>> GetAllCountriesAsync()
        {
            var response = await _httpClient.GetAsync("all");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CountryDTO>>(content);
        }

        // Phương thức để lấy thông tin quốc gia theo tên từ REST Countries API
        public async Task<CountryDTO> GetCountryByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"name/{name}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<CountryDTO>>(content);
            return countries.FirstOrDefault();
        }
    }
}
