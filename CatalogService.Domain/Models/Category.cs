namespace CatalogService.Domain.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
