using Microsoft.AspNetCore.Identity;
using WebApplication1.Infrastructure.Data;

namespace WebApplication1.Filters
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {

            // Cấu hình Identity
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<AuthWalksDbContext>()
            //    .AddDefaultTokenProviders();

            // Set up Identity
            services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<IdentityRole>() // Add support for roles
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("AuthWalksProvider") // Add Token Providers
            .AddEntityFrameworkStores<AuthWalksDbContext>() // Use Entity Framework Store
            .AddDefaultTokenProviders(); // Add default token providers

            return services;
        }
    }
}
