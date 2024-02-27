using System.Text;
using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Majestic.WarehouseService.Services.RabbitMq.Publisher
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly ILogger<RabbitMqMessagePublisher> _logger;
        private readonly IConnection _connection;

        public RabbitMqMessagePublisher(
            ILogger<RabbitMqMessagePublisher> logger,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _connection = connectionProvider.GetConnection();
        }

        public ServiceResult PublishMessage<T>(T message, string exchangeName, string routingKey = "")
        {
            using (var channel = _connection.CreateModel())
            {
                try
                {
                    var jsonMessage = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);

                    return new ServiceResult(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while publishing message to queue {@message} {exchangeName} {routingKey}",
                        message, exchangeName, routingKey);

                    return new ServiceResult();
                }
            }
        }
    }
}
