using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Infrastructure.Repositories.Impl;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController: ControllerBase
    {

        private readonly IWalkService _iwalkService;
        public WalkController(IWalkService walkService)
        {
            _iwalkService = walkService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] CreateWalkRequest request)
        {
            var response = await _iwalkService.CreateWalk(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _iwalkService.GetAllWalks();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> DetailWalk([FromRoute] Guid id)
        {
            DetailWalkResponse walk = await _iwalkService.DetailWalk(id);
            if (walk == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy
            }

            return Ok(walk); // Trả về 200 OK cùng với thông tin walk
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalk(Guid id, [FromBody] UpdateWalkRequest request)
        {
            // Logic cập nhật walk
            var walk = await _iwalkService.UpdateWalk(id, request);

            return Ok(walk); // Trả về 200 OK cùng với thông tin walk
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var deleteRegion = await _iwalkService.DeleteWalk(id);
            return Ok(deleteRegion);
        }

        [HttpGet]
        [Route("/search")]
        public async Task<ActionResult<IEnumerable<DetailWalkResponse>>> GetAllWorks(
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null, 
        [FromQuery] string? sortBy = "NameWalk",
        [FromQuery] string? direction = "asc",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000
        )
        {
            var works = await _iwalkService.SearchWalks(filterOn, filterQuery, sortBy, direction, pageNumber, pageSize);
            return Ok(works);
        }

    }
}
