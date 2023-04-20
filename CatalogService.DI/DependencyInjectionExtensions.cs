using CatalogService.BLL.MQ;
using CatalogService.BLL.Services;
using CatalogService.BLL.Validation;
using CatalogService.DAL;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;
using MessageQueue.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // DAL
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        // BLL
        services.AddScoped<IService<Category>, CategoriesService>();
        services.AddScoped<IService<Product>, ProductsService>();
        services.AddScoped<AbstractValidator<Category>, CategoryValidator>();
        services.AddScoped<AbstractValidator<Product>, ProductValidator>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DbContext, CatalogServiceContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, string messageQueueName)
    {
        services.AddScoped<IMessageProducer>(s => new MessageProducer(s.GetService<IRabbitMQConnectionProvider>(), messageQueueName));
        return services;
    }
}
