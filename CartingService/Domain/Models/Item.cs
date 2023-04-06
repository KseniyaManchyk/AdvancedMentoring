namespace CartingService.Domain.Models;

public class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int? Quantity { get; set; }

    public Image? Image { get; set; }
}