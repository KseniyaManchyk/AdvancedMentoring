using CartingService.BLL;
using CartingService.DAL;

namespace CartingService.WebApi.Helpers
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, string connectionString)
        {
            services.AddDALServices(connectionString);
            services.AddBLLServices();
            return services;
        }
    }
}
