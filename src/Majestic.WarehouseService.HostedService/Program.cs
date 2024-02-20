using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Majestic.WarehouseService.HostedService
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                await CreateHostBuilder(args).RunConsoleAsync();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = "your_rabbitmq_connection_string";
                    var exchangeName = "car_sell_exchange";

                    //services.AddSingleton<IMessageQueuePublisher>(provider =>
                    //{
                    //    return new RabbitMQPublisher(connectionString, exchangeName);
                    //});

                    services.AddSingleton<CarSellMessageConsumer>(provider =>
                    {
                        var serviceProvider = provider.GetRequiredService<IServiceProvider>();
                        return new CarSellMessageConsumer(connectionString, exchangeName, serviceProvider);
                    });

                    // Добавьте ваши службы, сервисы и репозитории
                    // services.AddTransient<ICarSellService, CarSellService>();
                    // services.AddTransient<ICarsRepository, CarsRepository>();
                    // ...

                    // Добавьте другие зависимости, если это необходимо
                });
    }
}