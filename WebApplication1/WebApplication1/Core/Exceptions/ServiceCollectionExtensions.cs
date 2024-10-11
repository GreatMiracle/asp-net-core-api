using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Repositories.Impl;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Services.Impl;
using WebApplication1.Services;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Core.Exceptions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký Repository
            services.AddScoped<IRepository<RegionRequestBase, Region>, RegionRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            // Đăng ký Service
            services.AddScoped<IRegionService, RegionServiceImpl>();
            services.AddScoped<IWalkService, RegionServiceImpl>();
        }
    }
}
