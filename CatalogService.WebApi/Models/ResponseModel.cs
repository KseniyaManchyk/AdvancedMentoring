namespace CatalogService.WebApi.Models;

public class ResponseModel<T>
{
    public string? NextLink { get; set; }

    public IEnumerable<T> Items { get; set; } = new List<T>();
}
