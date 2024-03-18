using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Majestic.WarehouseService.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                var webHost = CreateHostBuilder(args).Build();
                await webHost.RunAsync();
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();

        private static IConfigurationRoot GetConfigurations() =>
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }
}
