using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services.Impl
{
    public class TokenServiceImpl : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenServiceImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResponse CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            // Tạo claims cho email và roles
            // Tạo Access Token
            var accessToken = CreateToken(user, roles, int.Parse(jwtSettings["AccessTokenLifetime"])); 

            // Tạo Refresh Token (thường thời hạn dài hơn, )
            var refreshToken = CreateToken(user, roles, int.Parse(jwtSettings["RefreshTokenLifetime"]));

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string CreateToken(IdentityUser user, List<string> roles, int minutesValid)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };


            foreach (var role in roles)
            {
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(minutesValid),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
