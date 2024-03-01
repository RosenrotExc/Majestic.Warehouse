using System.Text;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Majestic.WarehouseService.Services.RabbitMq.HostedServices.ProcessSellCar
{
    public sealed class ProcessSellCarHostedService : BackgroundService, IDisposable
    {
        public const string QueueName = Constants.ProcessSellCarQueueName;

        private readonly ILogger<ProcessSellCarHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ProcessSellCarHostedService(
            ILogger<ProcessSellCarHostedService> logger,
            IServiceProvider serviceProvider,
            IConnectionProvider connectionProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _connection = connectionProvider.GetConnection();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var channel = _connection.CreateModel())
            {
                try
                {
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (_, args) =>
                    {
                        try
                        {
                            var body = args.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);

                            var result = await ProcessMessageAsync(message);
                            if (result)
                            {
                                channel.BasicAck(args.DeliveryTag, false);
                            }
                            else
                            {
                                channel.BasicNack(args.DeliveryTag, false, true);
                            }
                        }
                        catch
                        {
                            channel.BasicNack(args.DeliveryTag, false, true);
                        }
                    };

                    channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                    while (!_cancellationTokenSource.Token.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while setting up the consumer for queue {queueName}", QueueName);
                }
            }
        }

        private async Task<bool> ProcessMessageAsync(string jsonMessage)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IProcessSellCardHandler>();
                    var request = JsonConvert.DeserializeObject<ProcessSellCarEvent>(jsonMessage);

                    var result = await handler.HandleAsync(request);
                    if (result.IsSuccess)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message {@jsonMessage}", jsonMessage);
                return false;
            }
        }
    }
}
