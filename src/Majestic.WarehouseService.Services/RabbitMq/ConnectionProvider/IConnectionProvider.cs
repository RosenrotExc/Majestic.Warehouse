using RabbitMQ.Client;

namespace Majestic.WarehouseService.Services.RabbitMq.ConnectionProvider
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}
