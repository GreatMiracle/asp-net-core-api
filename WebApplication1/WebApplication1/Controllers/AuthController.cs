using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Invalid data.");

            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username // Treat username as email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (request.Roles != null && request.Roles.Length > 0)
                {
                    await _userManager.AddToRolesAsync(user, request.Roles);
                }
                return Ok("User registered successfully. Please log in.");
            }

            return BadRequest("User registration failed.");
        }
    }

}
