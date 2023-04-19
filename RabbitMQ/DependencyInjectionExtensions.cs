using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Implementation;
using RabbitMQ.Interfaces;

namespace RabbitMQ;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMQConnectionProvider(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IRabbitMQConnectionProvider>(s => new RabbitMQConnectionProvider(connectionString));
        return services;
    }
}