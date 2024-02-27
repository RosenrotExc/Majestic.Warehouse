using Majestic.WarehouseService.Models.Misc;

namespace Majestic.WarehouseService.Services.RabbitMq.Publisher
{
    public interface IMessagePublisher
    {
        ServiceResult PublishMessage<T>(T message, string exchangeName, string routingKey = "");
    }
}
