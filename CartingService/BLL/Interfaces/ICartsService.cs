using CartingService.Domain.Models;

namespace CartingService.BLL.Interfaces;

public interface ICartsService
{
    IEnumerable<Cart> GetCarts();

    Cart GetCartById(string cartId);

    IEnumerable<Item> GetCartItems(string cartId);

    void AddItemToCart(string cartId, Item item);

    void UpdateItems(Item item);

    void RemoveItemFromCart(string cartId, int itemId);
}
