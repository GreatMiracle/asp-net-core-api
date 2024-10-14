using Microsoft.AspNetCore.Identity;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services
{
    public interface ITokenService
    {
        TokenResponse CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
