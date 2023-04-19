using CartingService.BLL.Interfaces;
using FluentValidation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Interfaces;
using RabbitMQ.Models;
using System.Text;
using System.Text.Json;

namespace CartingService.WebApi.MQ;

public class MessageConsumer : IMessageConsumer, IDisposable
{
    private const int numberOfRetries = 5;
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
            var triesCount = 0;

            while (triesCount <= numberOfRetries)
            {
                try
                {
                    _cartsService.UpdateItems(new Domain.Models.Item
                    {
                        Id = deserializedMessage.UpdatedItem.Id,
                        Name = deserializedMessage.UpdatedItem.Name,
                        Price = deserializedMessage.UpdatedItem.Price,
                    });

                    _connectionModel.BasicAck(deliveryEventArgs.DeliveryTag, false);
                }
                catch (ValidationException)
                {
                    _connectionModel.BasicReject(deliveryEventArgs.DeliveryTag, true);
                }
                catch(Exception)
                {
                    triesCount++;
                }
            }
        };

        _connectionModel.BasicConsume(_messageQueueName, false, consumer);
    }

    public void Dispose()
    {
        _connectionProvider.Dispose();
    }
}
