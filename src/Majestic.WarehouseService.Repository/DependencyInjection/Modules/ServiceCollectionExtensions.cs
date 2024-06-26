﻿using Majestic.WarehouseService.Repository.Contexts;
using Majestic.WarehouseService.Repository.Repository.Cars;
using Majestic.WarehouseService.Repository.Repository.Initiators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Majestic.WarehouseService.Repository.DependencyInjection.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<WarehouseDbContext>(options =>  options.UseSqlServer(configuration.GetConnectionString("Db")), ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<ICarsRepository, CarsRepository>()
                .AddTransient<IInitiatorRepository, InitiatorRepository>();
        }
    }
}
