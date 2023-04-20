using RabbitMQ.Client;

namespace MessageQueue.Interfaces;

public interface IRabbitMQConnectionProvider: IDisposable
{
    IModel GetConnectionModel();
}
