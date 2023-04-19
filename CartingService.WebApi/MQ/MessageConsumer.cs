using CartingService.BLL.Implementation;
using CartingService.BLL.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Interfaces;
using RabbitMQ.Models;
using System.Text;
using System.Text.Json;

namespace CartingService.WebApi.MQ;

public class MessageConsumer : BackgroundService, IMessageConsumer, IDisposable
{
    private readonly string _messageQueueName;
    private readonly IServiceProvider _services;
    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private IModel _connectionModel;

    public MessageConsumer(IServiceProvider services, string messageQueueName)
    {
        using var scope = _services.CreateScope();
        _connectionProvider = scope.ServiceProvider.GetRequiredService<IRabbitMQConnectionProvider>();
        _messageQueueName = messageQueueName;
        _services = services;

        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void ProcessMessages()
    {
        using (var scope = _services.CreateScope())
        {
            var cartsService = scope.ServiceProvider.GetRequiredService<ICartsService>();

            _connectionModel.QueueDeclare(queue: _messageQueueName,
                               durable: false,
                               exclusive: false,
                               autoDelete: true,
                               arguments: null);

            var consumer = new EventingBasicConsumer(_connectionModel);

            consumer.Received += (model, deliveryEventArgs) =>
            {
                var content = Encoding.UTF8.GetString(deliveryEventArgs.Body.ToArray());

                var deserializedMessage = JsonSerializer.Deserialize<Message>(content);

                cartsService.UpdateItems(new Domain.Models.Item
                {
                    Id = deserializedMessage.UpdatedItem.Id,
                    Name = deserializedMessage.UpdatedItem.Name,
                    Price = deserializedMessage.UpdatedItem.Price,
                });

                _connectionModel.BasicAck(deliveryEventArgs.DeliveryTag, false);
            };

            _connectionModel.BasicConsume(_messageQueueName, false, consumer);
        }
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        ProcessMessages();

        return Task.CompletedTask;
    }
}
