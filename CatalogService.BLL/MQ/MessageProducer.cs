using RabbitMQ.Client;
using MessageQueue.Interfaces;
using System.Text;
using System.Text.Json;
using CatalogService.Domain.ExceptionHandling;

namespace CatalogService.BLL.MQ;

public class MessageProducer: IMessageProducer, IDisposable
{
    private const int numberOfRetries = 5;
    private readonly string _messageQueueName;

    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly IModel _connectionModel;

    public MessageProducer(IRabbitMQConnectionProvider connectionProvider, string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;

        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void SendMessage<T>(T message)
    {
        TryToConnect();

        var serializedMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(serializedMessage);

        _connectionModel.BasicPublish(exchange: "",
                       routingKey: _messageQueueName,
                       basicProperties: null,
                       body: body);
    }

    private void TryToConnect()
    {
        var triesCount = 0;

        while (triesCount < numberOfRetries)
        {
            try
            {
                _connectionModel.QueueDeclare(queue: _messageQueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                break;
            }
            catch (Exception ex)
            {
                triesCount++;
                if (triesCount == numberOfRetries)
                {
                    throw new MessageQueueConectionException(_messageQueueName, ex);
                }
            }
        }
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }
}
