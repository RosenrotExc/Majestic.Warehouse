using System.Text;
using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider;
using Majestic.WarehouseService.Services.Services.Cars.ProcessSellCarHandler;
using Microsoft.Extensions.Caching.Distributed;
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
        public const string DeadLetterQueueName = Constants.ProcessSellCarDeadLetterQueueName;
        public const string DeadLetterExchangeName = Constants.ProcessSellCarDeadLetterExchangeName;

        private readonly ILogger<ProcessSellCarHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IDistributedCache _cache;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ProcessSellCarHostedService(
            ILogger<ProcessSellCarHostedService> logger,
            IServiceProvider serviceProvider,
            IConnectionProvider connectionProvider,
            IDistributedCache cache)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _connection = connectionProvider.GetConnection();
            _cache = cache;
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
                            if (!result)
                            {
                                await RepublishDeadLetter(args, channel);
                            }

                            channel.BasicAck(args.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            await RepublishDeadLetter(args, channel);
                            channel.BasicAck(args.DeliveryTag, false);
                            _logger.LogError(ex, "An error occurred while processing message from {queueName}", QueueName);
                        }
                    };

                    channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
                    channel.BasicConsume(queue: DeadLetterQueueName, autoAck: false, consumer: consumer);

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

        private T ConvertMessage<T>(BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var request = JsonConvert.DeserializeObject<T>(message);
            return request;
        }

        #region Republish
        private async Task RepublishDeadLetter(BasicDeliverEventArgs args, IModel channel)
        {
            var deadLetterProperties = channel.CreateBasicProperties();

            var basicEvent = ConvertMessage<BasicEvent>(args);
            var delay = await PrepareRepublishDelay(basicEvent.RequestId);

            deadLetterProperties.Persistent = true;
            deadLetterProperties.Headers = new Dictionary<string, object>
            {
                { "x-delay", delay }
            };

            var body = args.Body.ToArray();
            channel.BasicPublish(DeadLetterExchangeName, "", deadLetterProperties, body);

            return;
        }

        private async Task<int> PrepareRepublishDelay(string requestId)
        {
            const int MinDelayInMs = 2 * 1000;
            const int MaxDelayInMs = 128 * 1000;

            var republishMessageKey = $"{requestId}-{Models.Misc.Constants.RedisContants.RepublishMessageKey}";

            var republishKey = await _cache.GetStringAsync(republishMessageKey);
            if (!string.IsNullOrEmpty(republishKey) && int.TryParse(republishKey, out var result))
            {
                var delayTime = Math.Min(result * 2, MaxDelayInMs);
                await SetCache(delayTime);

                return delayTime;
            }

            await SetCache(MinDelayInMs);

            return MinDelayInMs;

            #region SetCache
            async Task SetCache(int delay)
            {
                await _cache.SetStringAsync(republishMessageKey, delay.ToString(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                    });
            }
            #endregion
        }
        #endregion
    }
}
