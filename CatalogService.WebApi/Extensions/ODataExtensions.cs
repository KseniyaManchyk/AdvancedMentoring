using CatalogService.Domain.Models;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;

namespace CatalogService.WebApi.Extensions
{
    public static class ODataExtensions
    {
        public static IServiceCollection AddControllersAndOData(this IServiceCollection services)
        {
            var modelBuilder = new ODataConventionModelBuilder();

            var entityTypeConfig = modelBuilder.EntitySet<Product>("Products");

            services.AddControllers()
                    .AddOData(options => options.Select()
                                                .Filter()
                                                .Count()
                                                .SetMaxTop(null)
                                                .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

            return services;
        }
    }
}
