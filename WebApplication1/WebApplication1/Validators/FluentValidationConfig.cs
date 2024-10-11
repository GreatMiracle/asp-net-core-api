using FluentValidation;
using FluentValidation.AspNetCore;

namespace WebApplication1.Validators
{
    public static class FluentValidationConfig
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UpdateRegionRequestValidator>();
        }
    }
}
