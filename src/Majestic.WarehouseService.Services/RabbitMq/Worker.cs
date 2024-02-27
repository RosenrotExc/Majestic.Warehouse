using Majestic.WarehouseService.Services.RabbitMq.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Majestic.WarehouseService.Services.RabbitMq
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queuesToConsume = new[] { Constants.ProcessSellCarQueueName };

            var tasks = queuesToConsume.Select(queueName => Task.Run(() => ConsumeMessagesFromQueue(queueName, stoppingToken), stoppingToken));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
            }
        }

        private void ConsumeMessagesFromQueue(string queueName, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageConsumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();

                try
                {
                    _logger.LogInformation("Worker is consuming messages from queue: {queueName}", queueName);

                    messageConsumer.Consume(queueName, stoppingToken);

                    _logger.LogInformation("Worker has completed consuming messages from queue: {queueName}", queueName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while consuming messages from queue {queueName}", queueName);
                }
            }
        }
    }
}
