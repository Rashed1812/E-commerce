using Service.MappingProfiles;
using Service;
using ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce
{
    public static class ServiceRegestriation
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddWebApplicationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>((options) =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiVaidationErrorResponse;
            });
            return services;
        }
    }
}
