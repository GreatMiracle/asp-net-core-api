using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _itokenService;
        public AccountController(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _itokenService = tokenService;
        }

        // API Login - chuyển hướng tới trang đăng nhập của Google
        [HttpGet("login")]
        public IActionResult Login()
        {

            // URL callback sẽ được gọi sau khi người dùng đăng nhập thành công
            //var redirectUrl = Url.Action("GoogleLoginCallback", "Account");
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/Account/GoogleLoginCallback";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            // Redirect đến trang Google đăng nhập
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // API Callback - được gọi sau khi Google trả về thông tin đăng nhập
        //[HttpGet("GoogleLoginCallback")]
        //public async Task<IActionResult> GoogleLoginCallback()
        //{
        //    // Xác thực lại cookie để lấy thông tin đăng nhập từ Google
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    if (!result.Succeeded)
        //    {
        //        return Unauthorized();
        //    }

        //    var claims = result.Principal.Claims
        //        .Select(c => new
        //        {
        //            Type = c.Type,
        //            Value = c.Value
        //        })
        //        .ToList();


        //    // Trả về phản hồi thành công, có thể kèm thông tin user
        //    return Ok(new { message = "Login successful", claims });
        //}

        [HttpGet("GoogleLoginCallback")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var user = result.Principal;
            var emailClaim = user.FindFirst(ClaimTypes.Email);
            var nameClaim = user.FindFirst(ClaimTypes.Name);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found.");
            }

            // Làm sạch tên người dùng
            var username = CleanUsername(nameClaim?.Value);

            // Kiểm tra người dùng trong cơ sở dữ liệu
            var existingUser = await _userManager.FindByEmailAsync(emailClaim.Value);
            if (existingUser == null)
            {
                // Nếu người dùng không tồn tại, tạo tài khoản mới
                var newUser = new IdentityUser
                {
                    Email = emailClaim.Value,
                    UserName = username // Sử dụng tên người dùng đã làm sạch
                };

                var createUserResult = await _userManager.CreateAsync(newUser);
                if (!createUserResult.Succeeded)
                {
                    return BadRequest(createUserResult.Errors);
                }

                // Thêm vai trò mặc định
                await _userManager.AddToRoleAsync(newUser, "Reader");
                existingUser = newUser;
            }

            // Tạo token
            var roles = await _userManager.GetRolesAsync(existingUser);
            var tokenResponse = _itokenService.CreateJWTToken(existingUser, roles.ToList());

            return Ok(new
            {
                message = "Login successful",
                token = tokenResponse.AccessToken,
                refreshToken = tokenResponse.RefreshToken
            });
        }

        // API Logout - đăng xuất người dùng
        [HttpGet("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("login-url")]
        public IActionResult GetLoginUrl()
        {
            var redirectUrl = Url.Action("GoogleLoginCallback", "Account", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            var loginUrl = Url.Action("Login", "Account", null, Request.Scheme);
            return Ok(new { LoginUrl = loginUrl });
        }


        private string CleanUsername(string username)
        {
            // Thay thế ký tự không hợp lệ trong tên người dùng
            if (string.IsNullOrWhiteSpace(username))
            {
                return "User_" + Guid.NewGuid(); // Trả về tên mặc định nếu tên trống
            }

            // Giữ lại chỉ các ký tự chữ cái, số, dấu gạch dưới và dấu chấm
            var cleanedUsername = new StringBuilder();
            foreach (var c in username)
            {
                if (char.IsLetterOrDigit(c) || c == '_' || c == '.')
                {
                    cleanedUsername.Append(c);
                }
            }

            return cleanedUsername.ToString();
        }

    }
}
