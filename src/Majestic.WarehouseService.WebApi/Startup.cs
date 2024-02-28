using Majestic.WarehouseService.Repository.DependencyInjection.Modules;
using Majestic.WarehouseService.Services.DependencyInjection.Modules;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

namespace Majestic.WarehouseService.WebApi
{
    public class Startup
    {
        private string ServiceName = "Majestic.WarehouseService.WebApi";

        public IConfiguration _configuration { get; }
        public IHostEnvironment _environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Register
            services.AddServices();
            services.AddMappers();
            services.AddValidators();
            services.AddRepositories();
            services.AddDbContext(_configuration);
            services.AddRabbitMqProducer();
            services.AddRedisCache(_configuration);
            #endregion

            services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(so =>
            {
                so.SwaggerDoc("v1", new OpenApiInfo { Title = ServiceName, Version = "v1" });

                var swaggerBaseAddress = _configuration["ASPNETCORE_URLS"];
                if (!string.IsNullOrEmpty(swaggerBaseAddress))
                {
                    so.AddServer(new OpenApiServer { Url = swaggerBaseAddress });
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(so =>
            {
                so.RouteTemplate = "/docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(so =>
            {
                so.RoutePrefix = "docs";
                so.SwaggerEndpoint("/docs/v1/swagger.json", $"{ServiceName} V1");
            });

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
