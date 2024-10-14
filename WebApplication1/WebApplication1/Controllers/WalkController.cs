using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Utils;
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
        public async Task<ActionResult<IEnumerable<DetailWalkResponse>>> GetAllWalks(
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel(
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = "NameWalk",
            [FromQuery] string? direction = "asc"
        )
        {
            var walks = await _iwalkService.SearchWalks(filterOn, filterQuery, sortBy, direction, 1, 99999999); // Lấy tất cả dữ liệu

            // Sử dụng ExcelExportHelper để tạo tệp Excel
            var excelHelper = new ExcelExportHelper("Walks");

            // Đặt tiêu đề cho các cột với định dạng
            excelHelper.AddHeader(new[] { "ID", "Name", "Length (km)", "Difficulty", "Region" },
                                  Color.LightBlue, Color.Black, FontStyle.Bold);

            // Điền dữ liệu
            for (int i = 0; i < walks.Count(); i++)
            {
                var walk = walks.ElementAt(i);
                excelHelper.AddRow(new object[]
                {
                    walk.Id,
                    walk.Name,
                    walk.Length,
                    walk.Difficulty?.Name.ToString(),
                    walk.Region?.Code
                        }, i + 2); // Bắt đầu từ dòng thứ 2 vì dòng 1 là tiêu đề
            }

            // Trả về tệp Excel
            var stream = excelHelper.GetExcelStream();
            var fileName = $"WalkData_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
