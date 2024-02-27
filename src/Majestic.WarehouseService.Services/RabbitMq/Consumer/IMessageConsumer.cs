namespace Majestic.WarehouseService.Services.RabbitMq.Consumer
{
    public interface IMessageConsumer
    {
        void Consume(string queueName, CancellationToken stoppingToken);
    }
}
