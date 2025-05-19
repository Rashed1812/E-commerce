using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Service.MappingProfiles;
using Service;
using ServiceAbstraction;
using Domain.Contracts;
using Persistence.Repositotories;
using Persistence.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence.Identity;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public static class InfrastructureRegestiration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfwork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
               return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RediosConnection"));
            });
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("IdentityConnection");
                options.UseSqlServer(connectionString);
            });
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
        }
    }
}
