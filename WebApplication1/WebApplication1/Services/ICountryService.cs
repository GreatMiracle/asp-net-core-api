using System.Diagnostics.Metrics;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface ICountryService
    {
        Task<List<CountryDTO>> GetAllCountriesAsync();
        Task<CountryDTO> GetCountryByNameAsync(string name);
    }
}
 