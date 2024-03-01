using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Majestic.WarehouseService.Services.RabbitMq.HostedServices.ProcessSellCar;
using Majestic.WarehouseService.Services.RabbitMq.Publisher;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
using Majestic.WarehouseService.Services.Services.Cars.GetCarsMetrics;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler;
using Majestic.WarehouseService.Services.Services.Cars.UpdateCarCommand;
using Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Majestic.WarehouseService.Services.DependencyInjection.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ICreateCarCommandService, CreateCarCommandService>()
                .AddTransient<IUpdateCarCommandService, UpdateCarCommandService>()
                .AddTransient<IDeleteCarCommandService, DeleteCarCommandService>()
                .AddTransient<IGetCarQueryService, GetCarQueryService>()
                .AddTransient<IGetCarsMetricsQueryService, GetCarsMetricsQueryService>()
                .AddTransient<IProcessSellCarCommandService, ProcessSellCarCommandService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddTransient<ICarValidator, CarValidator>();
        }

        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            return services.AddTransient<ICarMapper, CarMapper>();
        }

        public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services)
        {
            services.AddTransient<IConnectionProvider, RabbitMqConnectionProvider>(); 
            services.AddTransient<IMessagePublisher, RabbitMqMessagePublisher>();
            return services;
        }

        public static IServiceCollection AddRabbitMqConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConnectionProvider, RabbitMqConnectionProvider>();

            #region ProcessSellCar
            services.AddHostedService<ProcessSellCarHostedService>();
            services.AddTransient<IProcessSellCardHandler, ProcessSellCardHandler>();
            #endregion

            var hostName = configuration["RabbitMQ:HostName"];
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                #region ProcessSellCar
                channel.QueueDeclare(RabbitMq.Constants.ProcessSellCarQueueName, false, false, false, null);
                channel.ExchangeDeclare(RabbitMq.Constants.ProcessSellCarExchangeName, ExchangeType.Direct, false, false, null);
                channel.QueueBind(RabbitMq.Constants.ProcessSellCarQueueName, RabbitMq.Constants.ProcessSellCarExchangeName, "");

                channel.QueueDeclare(RabbitMq.Constants.ProcessSellCarDeadLetterQueueName, true, false, false);
                var exchangeArgs = new Dictionary<string, object>
                {
                    { "x-delayed-type", "direct" }
                };
                channel.ExchangeDeclare(RabbitMq.Constants.ProcessSellCarDeadLetterExchangeName, RabbitMq.Constants.SystemExchangeTypeXDelayedMessage, true, false, exchangeArgs);
                channel.QueueBind(RabbitMq.Constants.ProcessSellCarDeadLetterQueueName, RabbitMq.Constants.ProcessSellCarDeadLetterExchangeName, "");
                #endregion
            }

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
                options.InstanceName = configuration["Redis:InstanceName"];
            });

            return services;
        }
    }
}
