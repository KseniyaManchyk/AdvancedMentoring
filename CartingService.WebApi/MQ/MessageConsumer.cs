using CartingService.BLL.Interfaces;
using CartingService.Domain.ExceptionHandling;
using FluentValidation;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using MessageQueue.Interfaces;
using MessageQueue.Models;
using System.Text;
using System.Text.Json;
using System.Diagnostics;

namespace CartingService.WebApi.MQ;

public class MessageConsumer : IMessageConsumer, IDisposable
{
    private const int numberOfRetries = 5;
    private readonly string _messageQueueName;

    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageConsumer> _logger;
    private IModel _connectionModel;

    public MessageConsumer(
        IRabbitMQConnectionProvider connectionProvider,
        IServiceProvider serviceProvider,
        ILogger<MessageConsumer> logger,
        string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;
        _connectionModel = _connectionProvider.GetConnectionModel();
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void ProcessMessages()
    {
        TryToConnect();

        var consumer = new EventingBasicConsumer(_connectionModel);

        consumer.Received += (model, deliveryEventArgs) =>
        {
            var content = Encoding.UTF8.GetString(deliveryEventArgs.Body.ToArray());
            var message = JsonSerializer.Deserialize<Message>(content);

            ProcessMessage(deliveryEventArgs, message);
        };

        _connectionModel.BasicConsume(_messageQueueName, false, consumer);
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }

    private void TryToConnect()
    {
        var triesCount = 0;

        _logger.LogInformation($"Message consumer is trying to connect message queue with name {_messageQueueName}.");

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
                    _logger.LogError("Message consumer could not connect message queue.");
                    throw new MessageQueueConectionException(_messageQueueName, ex);
                }
            }
        }
    }

    private void ProcessMessage(BasicDeliverEventArgs deliveryEventArgs, Message message)
    {
        InitializeActivity(message);

        _logger.LogInformation($"Message consumer get a new message. Trying to process it.");

        var triesCount = 0;

        while (triesCount < numberOfRetries)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var cartsService = scope.ServiceProvider.GetRequiredService<ICartsService>();

                    cartsService.UpdateItems(new Domain.Models.Item
                    {
                        Id = message.UpdatedItem.Id,
                        Name = message.UpdatedItem.Name,
                        Price = message.UpdatedItem.Price,
                    });
                }

                _connectionModel.BasicAck(deliveryEventArgs.DeliveryTag, false);
                break;
            }
            catch (ValidationException)
            {
                _logger.LogError($"Validation for new item failed.");
                _connectionModel.BasicNack(deliveryEventArgs.DeliveryTag, false, false);
                break;
            }
            catch (Exception)
            {
                triesCount++;
                if (triesCount == numberOfRetries)
                {
                    _logger.LogError($"Unknown error occured during updating item.");
                    _connectionModel.BasicNack(deliveryEventArgs.DeliveryTag, false, false);
                }
            }
        }
    }

    private void InitializeActivity(Message message)
    {
        var activity = new Activity("Processing");
        var traceId = ActivityTraceId.CreateFromString(message.TraceId);
        var spanId = ActivitySpanId.CreateFromString(message.SpanId);
        activity.SetParentId(traceId, spanId);
        activity.Start();
    }
}
