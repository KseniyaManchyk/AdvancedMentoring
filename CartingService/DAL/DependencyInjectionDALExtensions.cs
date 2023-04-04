using CartingService.DAL.Implementation;
using CartingService.DAL.Interfaces;
using CartingService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.DAL
{
    public static class DependencyInjectionDALExtensions
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IRepository<Cart, string>, CartsRepository>();
            services.AddScoped<ILiteDBConnectionProvider>(s => new LiteDBConnectionProvider(connectionString));
            return services;
        }
    }
}
