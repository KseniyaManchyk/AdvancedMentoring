using CartingService.BLL.Interfaces;
using CartingService.Domain.ExceptionHandling;
using FluentValidation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MessageQueue.Interfaces;
using MessageQueue.Models;
using System.Text;
using System.Text.Json;

namespace CartingService.WebApi.MQ;

public class MessageConsumer : IMessageConsumer, IDisposable
{
    private const int numberOfRetries = 5;
    private readonly string _messageQueueName;

    private readonly IRabbitMQConnectionProvider _connectionProvider;
    private readonly IServiceProvider _serviceProvider;
    private IModel _connectionModel;

    public MessageConsumer(
        IRabbitMQConnectionProvider connectionProvider,
        IServiceProvider serviceProvider,
        string messageQueueName)
    {
        _connectionProvider = connectionProvider;
        _messageQueueName = messageQueueName;
        _connectionModel = _connectionProvider.GetConnectionModel();
        _serviceProvider = serviceProvider;
    }

    public void ProcessMessages()
    {
        TryToConnect();

        var consumer = new EventingBasicConsumer(_connectionModel);

        consumer.Received += (model, deliveryEventArgs) =>
        {
            var content = Encoding.UTF8.GetString(deliveryEventArgs.Body.ToArray());

            ProcessMessage(deliveryEventArgs, JsonSerializer.Deserialize<Message>(content));
        };

        _connectionModel.BasicConsume(_messageQueueName, false, consumer);
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

    private void ProcessMessage(BasicDeliverEventArgs deliveryEventArgs, Message message)
    {
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
                _connectionModel.BasicNack(deliveryEventArgs.DeliveryTag, false, false);
                break;
            }
            catch (Exception)
            {
                triesCount++;
                if (triesCount == numberOfRetries)
                {
                    _connectionModel.BasicNack(deliveryEventArgs.DeliveryTag, false, false);
                }
            }
        }
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }
}
