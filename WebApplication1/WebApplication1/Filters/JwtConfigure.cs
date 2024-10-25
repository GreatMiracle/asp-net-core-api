using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using WebApplication1.Core.Utils;
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
                // Sử dụng JwtBearer để xác thực JWT token mặc định
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // Thêm DefaultSignInScheme cho đăng nhập bằng Google
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            EncryptionHelper.Decrypt(Environment.GetEnvironmentVariable("JWT_SIGNATURE_KEY")) ?? jwtSettings["Key"])
                        )
                };

                // Xử lý các lỗi phát sinh trong quá trình xác thực JWT
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Log lỗi nếu có
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Token validation failed. Please ensure you have a valid token.",
                            Details = context.Exception.Message
                        });

                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        // Đảm bảo rằng không có phản hồi nào được gửi trước đó
                        if (!context.Response.HasStarted)
                        {
                            // Xử lý token bị từ chối
                            context.HandleResponse(); // Ngăn chặn bất kỳ response nào khác tự động được gửi đi
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Token is missing or invalid.",
                                Details = "Authentication required"
                            });

                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask; // Nếu response đã bắt đầu, không làm gì nữa
                    },
                    OnForbidden = context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "You do not have permission to access this resource.",
                                Details = "Access denied."
                            });

                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    }
                };

            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = EncryptionHelper.Decrypt(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"))??configuration["Authentication:Google:ClientId"];
                options.ClientSecret = EncryptionHelper.Decrypt(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")) ??configuration["Authentication:Google:ClientSecret"];
                options.Scope.Add("email");
                options.Scope.Add("profile");
                options.CallbackPath = new PathString("/signin-google");
                options.SaveTokens = true;
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, GoogleDefaults.AuthenticationScheme)
                    .Build();
            });

            ;
        }
    }
}
