using Majestic.WarehouseService.Services.DependencyInjection.Modules;
using Majestic.WarehouseService.Repository.DependencyInjection.Modules;
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
                    services.AddServices();
                    services.AddMappers();
                    services.AddValidators();
                    services.AddRepositories();
                    services.AddDbContext(hostContext.Configuration);
                    services.AddRabbitMqConsumer(hostContext.Configuration);
                })
                .UseSerilog();
    }
}
