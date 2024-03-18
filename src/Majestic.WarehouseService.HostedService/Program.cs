using Majestic.WarehouseService.Repository.DependencyInjection.Modules;
using Majestic.WarehouseService.Services.DependencyInjection.Modules;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Majestic.WarehouseService.HostedService
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configurations = GetConfigurations();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configurations["Elasticsearch:Uri"])))
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
                    services.AddServices();
                    services.AddMappers();
                    services.AddValidators();
                    services.AddRepositories();
                    services.AddDbContext(hostContext.Configuration);
                    services.AddRabbitMqConsumer(hostContext.Configuration);
                    services.AddRedisCache(hostContext.Configuration);
                })
                .UseSerilog();

        private static IConfigurationRoot GetConfigurations() =>
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }
}
