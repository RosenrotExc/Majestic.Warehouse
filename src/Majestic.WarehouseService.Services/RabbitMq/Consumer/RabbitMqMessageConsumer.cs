using System;
using System.Text;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Majestic.WarehouseService.Services.RabbitMq.Consumer
{
    public class RabbitMqMessageConsumer : IMessageConsumer
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqMessageConsumer> _logger;
        private readonly IProcessSellCardHandler _processSellCardHandler;

        public RabbitMqMessageConsumer(
            ILogger<RabbitMqMessageConsumer> logger,
            IConnectionProvider connectionProvider,
            IProcessSellCardHandler processSellCardHandler)
        {
            _logger = logger;
            _connection = connectionProvider.GetConnection();
            _processSellCardHandler = processSellCardHandler;
        }

        public void Consume(string queueName, CancellationToken stoppingToken)
        {
            using (var channel = _connection.CreateModel())
            {
                try
                {
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);

                            _logger.LogInformation("Received message from queue {queueName}: {message}", queueName, message);

                            await MessageProcessingAsync(message, queueName);

                            _logger.LogInformation("Message processed successfully from queue {queueName}", queueName);

                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message from queue {queueName}", queueName);
                            channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                        }
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        Task.Delay(1000, stoppingToken).Wait();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while setting up the consumer for queue {queueName}", queueName);
                }
            }
        }

        private async Task MessageProcessingAsync(string message, string queueName)
        {
            switch (queueName)
            {
                case Constants.ProcessSellCarQueueName:
                    var request = JsonConvert.DeserializeObject<ProcessSellCarEvent>(message);
                    await _processSellCardHandler.HandleAsync(request);
                    break;
                default:
                    break;
            }
        }
    }
}
