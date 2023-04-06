using CartingService.BLL.Interfaces;
using CartingService.BLL.Validation;
using CartingService.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.BLL
{
    public static class DependencyInjectionBLLExtensions
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ICartsService, Implementation.CartsService>();
            services.AddScoped<AbstractValidator<Cart>, CartValidator>();
            services.AddScoped<AbstractValidator<Item>, ItemValidator>();
            return services;
        }
    }
}
