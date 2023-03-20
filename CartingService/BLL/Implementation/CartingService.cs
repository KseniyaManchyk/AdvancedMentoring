using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using System.Collections.Generic;

namespace CartingService.BLL.Implementation
{
    public class CartingService : ICartingService
    {
        IRepository<Cart> _cartingRepository;

        public CartingService(IRepository<Cart> cartingRepository)
        {
            _cartingRepository = cartingRepository;
        }

        public void AddItemToCart(int cartId, Item item)
        {
            var cart = _cartingRepository.GetById(cartId);

            cart.Items.Add(item);

            _cartingRepository.Update(cartId, cart);
        }

        public Cart GetCartById(int cartId)
        {
            return _cartingRepository.GetById(cartId);
        }

        public IEnumerable<Item> GetCartItems(int cartId)
        {
            var cart = _cartingRepository.GetById(cartId);
            return cart.Items;
        }

        public void RemoveItemFromCart(int cartId, Item item)
        {
            var cart = _cartingRepository.GetById(cartId);

            cart.Items.Remove(item);

            _cartingRepository.Update(cartId, cart);
        }
    }
}
