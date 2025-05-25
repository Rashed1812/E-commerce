using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Service.MappingProfiles;
using ServiceAbstraction;

namespace Service
{
    public static class ApplicationServicesRegestiration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<Func<IProductService>>(Provider =>
            () => Provider.GetRequiredService<IProductService>());

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Func<IBasketService>>(Provider =>
            () => Provider.GetRequiredService<IBasketService>());

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(Provider =>
            () => Provider.GetRequiredService<IAuthenticationService>());

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Func<IOrderService>>(Provider =>
            () => Provider.GetRequiredService<IOrderService>());

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            return services;
        }
    }
}
