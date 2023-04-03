using CartingService.DAL;

namespace CartingService.WebApi.DI
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.RegisterDALServices();
            services.RegisterBLLServices();
            return services;
        }
    }
}
