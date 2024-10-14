using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Request;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _itokenService;
        public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _itokenService = tokenService;
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


        [HttpPost("login")]
        public async Task<IActionResult> LoginWeb([FromBody] LoginRequest loginRequest)
        {
            // Tìm người dùng qua email (username)
            var user = await _userManager.FindByEmailAsync(loginRequest.username);
            if (user == null)
            {
                return BadRequest("Username or password is incorrect.");
            }

            // Kiểm tra mật khẩu
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!checkPassword)
            {
                return BadRequest("Username or password is incorrect.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokens = _itokenService.CreateJWTToken(user, roles.ToList());

            // Đăng nhập thành công, cần tạo token (bước này sẽ thực hiện sau)
            return Ok(tokens);
        }
    }

}
