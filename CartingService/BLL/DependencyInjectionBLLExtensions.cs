using CartingService.BLL.Interfaces;
using CartingService.BLL.Validation;
using CartingService.Domain;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.DAL
{
    public static class DependencyInjectionBLLExtensions
    {
        public static IServiceCollection RegisterBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ICartingService, BLL.Implementation.CartingService>();
            services.AddScoped<AbstractValidator<Cart>, CartValidator>();
            services.AddScoped<AbstractValidator<Item>, ItemValidator>();
            return services;
        }
    }
}
