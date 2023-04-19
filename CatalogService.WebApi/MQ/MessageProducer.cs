using RabbitMQ.Client;
using RabbitMQ.Interfaces;
using RabbitMQ.Models;
using System.Text;
using System.Text.Json;

namespace CatalogService.WebApi.MQ;

public class MessageProducer: IMessageProducer, IDisposable
{
    private readonly string _messageQueueName;
    private IRabbitMQConnectionProvider _connectionProvider;
    private IModel _connectionModel;

    public MessageProducer(IRabbitMQConnectionProvider connectionProvider, string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;

        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void SendMessage(Message message)
    {
        _connectionModel.QueueDeclare(queue: _messageQueueName,
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

        var serializedMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(serializedMessage);

        _connectionModel.BasicPublish(exchange: "",
                       routingKey: _messageQueueName,
                       basicProperties: null,
                       body: body);
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }
}
