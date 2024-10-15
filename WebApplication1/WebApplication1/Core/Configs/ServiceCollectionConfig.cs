using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Repositories.Impl;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Services.Impl;
using WebApplication1.Services;
using WebApplication1.DTOs.Request;

namespace WebApplication1.Core.Configs
{
    public static class ServiceCollectionConfig
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký Repository
            services.AddScoped<IRepository<RequestBase, Region>, RegionRepository>();
            services.AddScoped<IRepository<RequestBase, Walk>, WalkRepository>();

            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IWalkRepository, WalkRepository>();
            services.AddScoped<IUploadImgRepository, UploadImgRepository>();

            // Đăng ký Service
            services.AddScoped<IRegionService, RegionServiceImpl>();
            services.AddScoped<IWalkService, WalkServiceImpl>();
            services.AddScoped<ITokenService, TokenServiceImpl>();
            services.AddScoped<IUploadImgService, UploadImgServiceImpl>();
            services.AddScoped<IGChatService, GChatService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IThirdPartyApiService, ThirdPartyApiServiceImpl>();
            services.AddScoped<ICountryService, CountryServiceImpl>();

        }
    }
}
