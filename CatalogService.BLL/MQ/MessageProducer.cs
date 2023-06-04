using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CatalogService.Domain.ExceptionHandling;
using MessageQueue.Interfaces;
using MessageQueue.Models;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CatalogService.BLL.MQ;

public class MessageProducer: IMessageProducer, IDisposable
{
    private const int numberOfRetries = 5;
    private readonly string _messageQueueName;

    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly IModel _connectionModel;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(
        IRabbitMQConnectionProvider connectionProvider,
        ILogger<MessageProducer> logger,
        string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;
        _logger = logger;

        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void SendMessage<T>(T message) where T: ICorrelated
    {
        TryToConnect();

        message.SpanId = Activity.Current.SpanId.ToString();
        message.TraceId = Activity.Current.TraceId.ToString();

        var serializedMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(serializedMessage);

        _logger.LogInformation($"Message producer is sending a new message.");

        _connectionModel.BasicPublish(exchange: "",
                       routingKey: _messageQueueName,
                       basicProperties: null,
                       body: body);

        Activity.Current.Stop();
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }

    private void TryToConnect()
    {
        var triesCount = 0;

        _logger.LogInformation($"Message producer is trying to connect message queue with name {_messageQueueName}.");

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
                    _logger.LogError("Message producer could not connect message queue.");
                    throw new MessageQueueConectionException(_messageQueueName, ex);
                }
            }
        }
    }
}
