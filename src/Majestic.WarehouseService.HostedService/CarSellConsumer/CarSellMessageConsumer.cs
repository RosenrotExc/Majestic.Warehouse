using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class CarSellMessageConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;

    public CarSellMessageConsumer(string connectionString, string exchangeName, IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceProvider = serviceProvider;

        _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false);
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await ProcessMessageAsync(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    private async Task ProcessMessageAsync(string message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            //var carSellService = scope.ServiceProvider.GetRequiredService<ICarSellService>();
            //var carSellDto = Newtonsoft.Json.JsonConvert.DeserializeObject<CarSellDto>(message);

            //await carSellService.ProcessCarSellAsync(carSellDto);
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}