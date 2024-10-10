using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var studentNames = new string[] { "Nguyễn Văn A", "Trần Thị B", "Lê Văn C" };
            return Ok(studentNames);
        }
    }
}
