using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
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

        [HttpPost]
        [Route("/search")]
        public async Task<IActionResult> SearchRegion([FromBody] RegionSearchRequest request)
        {
            IEnumerable<SearchRegionResponse> regions = await
                _regionService.SearchRegions(request.NameCondition,
                    request.CodeCondition,
                    request.UrlImgCondition);

            // Trả về phản hồi 200 với danh sách khu vực
            return Ok(regions);
        }

        [HttpGet]
        [Route("/detail/{id:guid}")]
        public async Task<IActionResult> DetailRegion([FromRoute] Guid id)
        {
            var region = await
                _regionService.DetailRegion(id);

            return Ok(region);
        }

        [HttpPost]
        [Route("/new")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequest request)
        {
            DetailRegionResponse response = await _regionService.CreateRegion(request);
            return Ok(response);
        }

        [HttpPut("/update/{id}")]
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionRequest request)
        {
            var updatedRegion = await _regionService.UpdateRegion(id, request);

            if (updatedRegion == null)
            {
                return NotFound();
            }

            return Ok(updatedRegion);

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var deleteRegion = await _regionService.DeleteRegion(id);
            return Ok(deleteRegion);
        }

    }
}
