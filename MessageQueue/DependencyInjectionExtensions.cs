using MessageQueue.Implementation;
using MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueue;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMQConnectionProvider(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IRabbitMQConnectionProvider>(s => new RabbitMQConnectionProvider(connectionString));
        return services;
    }
}