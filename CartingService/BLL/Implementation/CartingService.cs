using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace CartingService.BLL.Implementation;

public class CartingService : ICartingService
{
    IRepository<Cart> _cartingRepository;
    AbstractValidator<Cart> _cartValidator;
    AbstractValidator<Item> _itemValidator;

    public CartingService(
        IRepository<Cart> cartingRepository,
        AbstractValidator<Cart> cartValidator,
        AbstractValidator<Item> itemValidator)
    {
        _cartingRepository = cartingRepository;
        _cartValidator = cartValidator;
        _itemValidator = itemValidator;
    }
    // Added for convinience
    public Cart GetCartById(int cartId)
    {
        return _cartingRepository.GetById(cartId);
    }

    public IEnumerable<Item> GetCartItems(int cartId)
    {
        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            throw new ArgumentException($"{nameof(cartId)} is not valid. System does not contain cart with this id.");
        }

        return cart.Items;
    }

    public void AddItemToCart(int cartId, Item item)
    {
        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            throw new ArgumentException($"{nameof(cartId)} is not valid. System does not contain cart with this id.");
        }

        _itemValidator.ValidateAndThrow(item);

        cart.Items.Add(item);

        _cartingRepository.Update(cartId, cart);
    }

    public void RemoveItemFromCart(int cartId, Item item)
    {
        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            throw new ArgumentException($"{nameof(cartId)} is not valid. System does not contain cart with this id.");
        }

        cart.Items.Remove(item);

        _cartingRepository.Update(cartId, cart);
    }
}
