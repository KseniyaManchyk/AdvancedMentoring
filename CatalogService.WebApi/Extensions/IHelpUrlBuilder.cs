namespace CatalogService.WebApi.Extensions
{
    public interface IHelpUrlBuilder
    {
        string BuildUrl(HttpRequest request, string? endpointPath = null);
    }
}