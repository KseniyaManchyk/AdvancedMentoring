using CartingService.Domain;

namespace CartingService.BLL.Interfaces;

public interface ICartingService
{
    Cart GetCartById(int cartId);

    IEnumerable<Item> GetCartItems(int cartId);

    void AddItemToCart(int cartId, Item item);

    void RemoveItemFromCart(int cartId, Item item);
}
