using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider
{
    public class RabbitMqConnectionProvider : IConnectionProvider
    {
        private readonly IConnection _connection;

        public RabbitMqConnectionProvider(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
            };

            _connection = factory.CreateConnection();
        }

        public IConnection GetConnection() => _connection;
    }
}
