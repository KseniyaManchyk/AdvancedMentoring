using RabbitMQ.Client;
using MessageQueue.Interfaces;
using System.Text;
using System.Text.Json;
using CatalogService.Domain.ExceptionHandling;
using Microsoft.Extensions.Logging;
using CorrelationId.Abstractions;
using MessageQueue.Models;

namespace CatalogService.BLL.MQ;

public class MessageProducer: IMessageProducer, IDisposable
{
    private const int numberOfRetries = 5;
    private readonly string _messageQueueName;

    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly IModel _connectionModel;
    private readonly ILogger<MessageProducer> _logger;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public MessageProducer(
        IRabbitMQConnectionProvider connectionProvider,
        ILogger<MessageProducer> logger,
        ICorrelationContextAccessor correlationContextAccessor,
        string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;
        _logger = logger;
        _correlationContextAccessor = correlationContextAccessor;

        _connectionModel = _connectionProvider.GetConnectionModel();
    }

    public void SendMessage<T>(T message) where T: ICorrelated
    {
        if (Guid.TryParse(_correlationContextAccessor.CorrelationContext.CorrelationId, out Guid correlationId))
        {
            using (_logger.BeginScope(new Dictionary<string, Guid> { ["CorrelationId"] = message.CorrelationId }))
            {
                message.CorrelationId = correlationId;
                Send(message);
            }
        }
        else
        {
            _logger.LogWarning($"Message producer does not find correlation id.");
            Send(message);
        }
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

    private void Send<T>(T message)
    {
        TryToConnect();

        var serializedMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(serializedMessage);

        _logger.LogInformation($"Message producer is sending a new message.");

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
