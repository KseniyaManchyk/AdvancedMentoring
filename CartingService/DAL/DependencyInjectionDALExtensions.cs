using CartingService.DAL.Implementation;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.DAL
{
    public static class DependencyInjectionDALExtensions
    {
        public static IServiceCollection RegisterDALServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Cart>, CartingRepository>();
            services.AddScoped<ILiteDBConnectionProvider, LiteDBConnectionProvider>();
            return services;
        }
    }
}
