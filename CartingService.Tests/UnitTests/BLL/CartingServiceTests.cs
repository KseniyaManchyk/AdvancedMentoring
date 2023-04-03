using CartingService.BLL.Interfaces;
using CartingService.BLL.Validation;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CartingService.Tests.UnitTests.BLL;

public class CartingServiceTests
{
    private ICartingService _sut;
    private Mock<IRepository<Cart>> _repositoryMock;
    private Mock<AbstractValidator<Item>> _itemValidatorMock;

    public CartingServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Cart>>();
        _itemValidatorMock = new Mock<AbstractValidator<Item>>();

        _sut = new CartingService.BLL.Implementation.CartingService(_repositoryMock.Object, null, _itemValidatorMock.Object);
    }

    [Fact]
    public void GetById_WhenCartExists_ShouldReturnCorrectCart()
    {
        var cartId = 1;
        var expectedCart = GetCarts().Last();

        _repositoryMock
            .Setup(x => x.GetById(cartId))
            .Returns(expectedCart)
            .Verifiable();

        var result = _sut.GetCartById(cartId);

        _repositoryMock.Verify();
        Assert.Equal(expectedCart, result);
    }

    [Fact]
    public void GetCartItems_WhenCartExists_ShouldReturnCorrectItemsCollection()
    {
        var cartId = 1;
        var expectedCart = GetCarts().Last();

        _repositoryMock
            .Setup(x => x.GetById(cartId))
            .Returns(expectedCart)
            .Verifiable();

        var result = _sut.GetCartItems(cartId);

        _repositoryMock.Verify();
        Assert.Equal(expectedCart.Items.Count, result.Count());
    }


    [Fact]
    public void GetCartItems_WhenCartDoesNotExist_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _sut.GetCartItems(1));
    }

    [Fact]
    public void AddItemToCart_ShouldCallAddMethod()
    {
        var cartId = 1;
        var newItem = new Item { Id = 1, Name = "Test", Price = 1, Quantity = 1 };
        var cart = GetCarts().Last();

        _repositoryMock
            .Setup(x => x.GetById(cartId))
            .Returns(cart);

        _sut.AddItemToCart(cartId, newItem);

        _repositoryMock.Verify(x => x.GetById(cartId), Times.Once);
        _repositoryMock.Verify(x => x.Update(cartId, cart), Times.Once);
    }

    [Fact]
    public void AddItemToCart_WhenCartDoesNotExist_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _sut.AddItemToCart(1, new Item()));
    }

    [Fact]
    // Temp test, can't mock validator method since it's extension method
    public void AddItemToCart_WhenItemIsNotValid_ShouldThrowException()
    {
        var cartId = 1;
        var newItem = new Item();
        var cart = GetCarts().Last();

        _repositoryMock
            .Setup(x => x.GetById(cartId))
            .Returns(cart);

        _sut = new CartingService.BLL.Implementation.CartingService(_repositoryMock.Object, null, new ItemValidator());

        Assert.Throws<ValidationException>(() => _sut.AddItemToCart(cartId, newItem));
    }

    [Fact]
    public void RemoveItemFromCart_ShouldCallUpdateMethod()
    {
        var cartId = 1;
        var cart = GetCarts().Last();
        var removingItem = cart.Items.Last();

        _repositoryMock
            .Setup(x => x.GetById(cartId))
            .Returns(cart);

        _sut.RemoveItemFromCart(cartId, removingItem);

        _repositoryMock.Verify(x => x.GetById(cartId), Times.Once);
        _repositoryMock.Verify(x => x.Update(cartId, cart), Times.Once);
    }


    [Fact]
    public void RemoveItemFromCart_WhenCartDoesNotExist_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _sut.RemoveItemFromCart(1, new Item()));
    }

    private List<Cart> GetCarts()
    {
        return new List<Cart>
        {
            new Cart
            {
                Id = 1,
                Items = new List<Item>
                {
                    new Item { Id = 1, Name = "Item 1", Price = 123, Quantity = 23 },
                }
            },
            new Cart
            {
                Id = 2,
                Items = new List<Item>
                {
                    new Item { Id = 11, Name = "Item 11", Price = 63, Quantity = 11 },
                    new Item { Id = 22, Name = "Item 22", Price = 1, Quantity = 34 },
                    new Item { Id = 33, Name = "Item 33", Price = 2, Quantity = 2 },
                }
            },
            new Cart
            {
                Id = 3,
                Items = new List<Item>
                {
                    new Item { Id = 111, Name = "Item 111", Price = 11, Quantity = 3 },
                    new Item { Id = 333, Name = "Item 333", Price = 42, Quantity = 6 },
                }
            },
        };
    }
}
