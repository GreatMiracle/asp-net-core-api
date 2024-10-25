using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThirdPartyApiController : ControllerBase
    {
        private readonly IThirdPartyApiService _thirdPartyApiService;
        private readonly ICountryService _countryService;

        public ThirdPartyApiController(IThirdPartyApiService thirdPartyApiService, ICountryService countryService)
        {
            _thirdPartyApiService = thirdPartyApiService;
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _thirdPartyApiService.GetPostsAsync();
                return Ok(data); // Trả về dữ liệu nếu thành công
            }
            catch (Exception ex)
            {
                // Xử lý exception và trả về thông báo lỗi
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Lấy tất cả các quốc gia từ REST Countries API
        [HttpGet("countries")]
        public async Task<ActionResult<List<CountryDTO>>> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        // Lấy thông tin quốc gia theo tên từ REST Countries API
        [HttpGet("countries/{name}")]
        public async Task<ActionResult<CountryDTO>> GetCountryByName(string name)
        {
            var country = await _countryService.GetCountryByNameAsync(name);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
    }
}
