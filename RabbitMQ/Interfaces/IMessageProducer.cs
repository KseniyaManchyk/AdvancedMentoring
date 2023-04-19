using RabbitMQ.Models;

namespace RabbitMQ.Interfaces;

public interface IMessageProducer
{
    void SendMessage(Message message);
}
