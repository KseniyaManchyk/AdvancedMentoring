using Microsoft.AspNetCore.Http.Extensions;

namespace CatalogService.WebApi.Extensions
{
    public class HelpUrlBuilder: IHelpUrlBuilder
    {
        public string BuildUrl(HttpRequest request, string? endpointPath = null)
        {
            var requestUri = new Uri(request.GetDisplayUrl());
            var host = $"{requestUri.Scheme}://{request.Host}";

            return string.IsNullOrWhiteSpace(endpointPath) ? $"{host}{request.Path}" : $"{host}/{endpointPath}";
        }
    }
}
