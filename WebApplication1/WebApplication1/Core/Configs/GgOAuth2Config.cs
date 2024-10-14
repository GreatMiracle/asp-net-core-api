using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace WebApplication1.Core.Configs
{
    public static class GgOAuth2Config
    {
        public static void AddGgOAuth2Config(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Chỉ định phương thức xác thực để đăng nhập
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.Scope.Add("email");
                options.Scope.Add("profile");
                options.CallbackPath = new PathString("/signin-google");
                options.SaveTokens = true;
            });

            services.AddAuthorization();

            services.AddControllers();
        }
    }
}
