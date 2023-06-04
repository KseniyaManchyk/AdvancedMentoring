namespace CartingService.Domain.Models;

public class Cart
{
    public string? Id { get; set; }

    public List<Item> Items { get; set; } = new List<Item>();
}
