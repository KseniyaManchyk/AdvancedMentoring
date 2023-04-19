using CartingService.BLL.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Interfaces;
using RabbitMQ.Models;
using System.Text;
using System.Text.Json;

namespace CartingService.WebApi.MQ;

public class MessageConsumer : IMessageConsumer, IDisposable
{
    private readonly string _messageQueueName;
    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly ICartsService _cartsService;
    private IModel _connectionModel;

    public MessageConsumer(IRabbitMQConnectionProvider connectionProvider, string messageQueueName, ICartsService cartsService)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;
        _cartsService = cartsService;
        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void ProcessMessages()
    {
        _connectionModel.QueueDeclare(queue: _messageQueueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var consumer = new EventingBasicConsumer(_connectionModel);

        consumer.Received += (model, deliveryEventArgs) =>
        {
            var content = Encoding.UTF8.GetString(deliveryEventArgs.Body.ToArray());

            var deserializedMessage = JsonSerializer.Deserialize<Message>(content);

            _cartsService.UpdateItems(new Domain.Models.Item
            {
                Id = deserializedMessage.UpdatedItem.Id,
                Name = deserializedMessage.UpdatedItem.Name,
                Price = deserializedMessage.UpdatedItem.Price,
            });

            _connectionModel.BasicAck(deliveryEventArgs.DeliveryTag, false);
        };

        _connectionModel.BasicConsume(_messageQueueName, false, consumer);
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }
}
