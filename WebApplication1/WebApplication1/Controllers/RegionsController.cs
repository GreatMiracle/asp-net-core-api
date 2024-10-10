using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.Entities;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;
        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           IEnumerable<Region> regions = await _regionService.GetAllRegions();

            // Trả về phản hồi 200 với danh sách khu vực
            return Ok(regions);
        }
    }
}
