﻿using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.Domain.ExceptionHandling;
using CartingService.Domain.Models;
using FluentValidation;

namespace CartingService.BLL.Implementation;

public class CartsService : ICartsService
{
    IRepository<Cart, string> _cartingRepository;
    AbstractValidator<Cart> _cartValidator;
    AbstractValidator<Item> _itemValidator;

    public CartsService(
        IRepository<Cart, string> cartingRepository,
        AbstractValidator<Cart> cartValidator,
        AbstractValidator<Item> itemValidator)
    {
        _cartingRepository = cartingRepository;
        _cartValidator = cartValidator;
        _itemValidator = itemValidator;
    }

    public IEnumerable<Cart> GetCarts()
    {
        return _cartingRepository.GetAll();
    }

    public Cart GetCartById(string cartId)
    {
        return _cartingRepository.GetById(cartId);
    }

    public IEnumerable<Item> GetCartItems(string cartId)
    {
        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), cartId);
        }

        return cart.Items;
    }

    public void AddItemToCart(string cartId, Item item)
    {
        _itemValidator.ValidateAndThrow(item);

        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            CreateCartWithItem(cartId, item);
        }
        else
        {
            AddItemToExistingCart(cart, item);
        }
    }

    public void RemoveItemFromCart(string cartId, int itemId)
    {
        var cart = _cartingRepository.GetById(cartId);

        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), cartId);
        }

        var itemToRemove = cart.Items.FirstOrDefault(x => x.Id == itemId);

        if (itemToRemove is null)
        {
            throw new NotFoundException(nameof(Item), itemId.ToString());
        }

        cart.Items.Remove(itemToRemove);
        _cartingRepository.Update(cartId, cart);
    }

    private void AddItemToExistingCart(Cart cart, Item item)
    {
        cart.Items.Add(item);
        _cartingRepository.Update(cart.Id, cart);
    }

    private void CreateCartWithItem(string cartId, Item item)
    {
        _cartingRepository.Add(cartId, new Cart
        {
            Id = cartId,
            Items = new List<Item> { item }
        });
    }
}
