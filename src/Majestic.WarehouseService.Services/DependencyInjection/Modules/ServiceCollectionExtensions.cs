using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.DependencyInjection;

namespace Majestic.WarehouseService.Services.DependencyInjection.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ICreateCarCommandService, CreateCarCommandService>()
                .AddTransient<IGetCarQueryService, GetCarQueryService>()
                .AddTransient<IProcessSellCarCommandService, ProcessSellCarCommandService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddTransient<ICreateCarValidator, CreateCarValidator>();
        }

        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            return services.AddTransient<ICreateCarMapper, CreateCarMapper>();
        }
    }
}
