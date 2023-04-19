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
            services.AddSingleton<ICartsService, Implementation.CartsService>();
            services.AddSingleton<AbstractValidator<Cart>, CartValidator>();
            services.AddSingleton<AbstractValidator<Item>, ItemValidator>();
            return services;
        }
    }
}
