using RabbitMQ.Client;
using RabbitMQ.Interfaces;
using System.Threading.Channels;

namespace RabbitMQ.Implementation;

public class RabbitMQConnectionProvider : IRabbitMQConnectionProvider
{
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _model;

    public RabbitMQConnectionProvider(string connectionString)
    {
        _connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };
    }

    public IModel GetConnectionModel()
    {
        _connection = _connectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        return _model;
    }

    public void Dispose()
    {
        _model.Close();
        _connection.Close();
    }
}
