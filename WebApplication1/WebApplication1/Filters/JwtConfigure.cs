using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace WebApplication1.Filters
{
    public static class JwtConfigure
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration) // Thêm IConfiguration vào đây
        {
            var jwtSettings = configuration.GetSection("JwtSettings"); // Sử dụng configuration

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = jwtSettings.GetValue<bool>("ValidateIssuer"),
                    ValidateAudience = jwtSettings.GetValue<bool>("ValidateAudience"),
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });
        }
    }
}
