using Majestic.WarehouseService.Services.DependencyInjection.Configurations;
using Majestic.WarehouseService.Services.Mappers.Cars;
using Majestic.WarehouseService.Services.RabbitMq;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Majestic.WarehouseService.Services.RabbitMq.Consumer;
using Majestic.WarehouseService.Services.RabbitMq.Publisher;
using Majestic.WarehouseService.Services.Services.Cars.CreateCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.DeleteCarCommand;
using Majestic.WarehouseService.Services.Services.Cars.GetCarQuery;
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
                .AddTransient<IProcessSellCarCommandService, ProcessSellCarCommandService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddTransient<ICreateCarValidator, CreateCarValidator>();
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
            services.AddTransient<IMessageConsumer, RabbitMqMessageConsumer>();
            services.AddHostedService<Worker>();
            services.AddTransient<IProcessSellCardHandler, ProcessSellCardHandler>();

            var hostName = configuration["RabbitMQ:HostName"];
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                #region ProcessSellCar
                channel.QueueDeclare(queue: RabbitMq.Constants.ProcessSellCarQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.ExchangeDeclare(exchange: RabbitMq.Constants.ProcessSellCarExchangeName, type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: RabbitMq.Constants.ProcessSellCarQueueName, exchange: RabbitMq.Constants.ProcessSellCarExchangeName, routingKey: "");
                #endregion
            }

            return services;
        }
    }
}
