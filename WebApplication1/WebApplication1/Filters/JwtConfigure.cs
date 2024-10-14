using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.WebRequestMethods;
namespace WebApplication1.Filters
{
    public static class JwtConfigure
    {

           /* Khi một JWT token được truyền vào ứng dụng(thông qua header của yêu cầu HTTP), 
            ASP.NET Core sẽ sử dụng các tham số cấu hình này để xác thực token.
            Nếu tất cả các kiểm tra này đều thành công, 
            ASP.NET Core sẽ giải mã token và lưu trữ thông tin claims(bao gồm cả vai trò) trong đối tượng User, 
            mà bạn có thể truy cập trong các controller hoặc middleware của ứng dụng.*/

        public static void ConfigureJWTServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings"); 

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
