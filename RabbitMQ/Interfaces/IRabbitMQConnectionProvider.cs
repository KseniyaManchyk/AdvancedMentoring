using RabbitMQ.Client;

namespace RabbitMQ.Interfaces;

public interface IRabbitMQConnectionProvider: IDisposable
{
    IModel GetConnectionModel();
}
