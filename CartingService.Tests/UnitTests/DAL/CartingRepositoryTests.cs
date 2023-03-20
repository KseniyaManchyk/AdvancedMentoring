using CartingService.DAL.Implementation;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using System.Collections.Generic;
using System.Linq;
using Moq;
using LiteDB;
using Xunit;

namespace CartingService.Tests.UnitTests.DAL;

public class CartingRepositoryTests
{
    private IRepository<Cart> _sut;
    private Mock<ILiteDBConnectionProvider> _connectionProviderMock;
    private Mock<ILiteDatabase> _liteDBMock;
    private Mock<ILiteCollection<Cart>> _liteCollectionMock;

    public CartingRepositoryTests()
    {
        _connectionProviderMock = new Mock<ILiteDBConnectionProvider>();
        _liteDBMock = new Mock<ILiteDatabase>();
        _liteCollectionMock = new Mock<ILiteCollection<Cart>>();

        _connectionProviderMock
            .Setup(x => x.GetConnection())
            .Returns(_liteDBMock.Object)
            .Verifiable();

        _liteDBMock
            .Setup(x => x.GetCollection<Cart>())
            .Returns(_liteCollectionMock.Object)
            .Verifiable();

        _sut = new CartingRepository(_connectionProviderMock.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnCartsCollection()
    {
        var expectedCarts = GetCarts();

        _liteCollectionMock
            .Setup(x => x.FindAll())
            .Returns(expectedCarts)
            .Verifiable();

        var result = _sut.GetAll();

        _connectionProviderMock.Verify();
        _liteDBMock.Verify();
        _liteCollectionMock.Verify();

        Assert.Equal(expectedCarts.Count, result.Count());
    }

    [Fact]
    public void GetById_WhenCartExists_ShouldReturnCorrectCart()
    {
        var cartId = 1;
        var expectedCart = GetCarts().Last();

        _liteCollectionMock
            .Setup(x => x.FindById(cartId))
            .Returns(expectedCart)
            .Verifiable();

        var result = _sut.GetById(cartId);

        _connectionProviderMock.Verify();
        _liteDBMock.Verify();
        _liteCollectionMock.Verify();

        Assert.Equal(expectedCart, result);
    }

    [Fact]
    public void GetById_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var cartId = 10;

        var result = _sut.GetById(cartId);

        _connectionProviderMock.Verify();
        _liteDBMock.Verify();
        _liteCollectionMock.Verify(x => x.FindById(cartId), Times.Once);

        Assert.Null(result);
    }

    [Fact]
    public void Add_ShouldCallInsertAndCommitMethods()
    {
        var newCartId = 1;
        var newCart = GetCarts().First();

        _sut.Add(newCartId, newCart);

        _connectionProviderMock.Verify();
        _liteDBMock.Verify(x => x.GetCollection<Cart>(), Times.Once);
        _liteDBMock.Verify(x => x.Commit(), Times.Once);
        _liteCollectionMock.Verify(x => x.Insert(newCartId, newCart), Times.Once);
    }

    [Fact]
    public void Remove_ShouldCallDeleteAndCommitMethods()
    {
        var removingCartId = 1;

        _sut.Remove(removingCartId);

        _connectionProviderMock.Verify();
        _liteDBMock.Verify(x => x.GetCollection<Cart>(), Times.Once);
        _liteDBMock.Verify(x => x.Commit(), Times.Once);
        _liteCollectionMock.Verify(x => x.Delete(removingCartId), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallUpdateAndCommitMethods()
    {
        var updatingCartId = 1;
        var updatingCart = GetCarts().First();

        _sut.Update(updatingCartId, updatingCart);

        _connectionProviderMock.Verify();
        _liteDBMock.Verify(x => x.GetCollection<Cart>(), Times.Once);
        _liteDBMock.Verify(x => x.Commit(), Times.Once);
        _liteCollectionMock.Verify(x => x.Update(updatingCartId, updatingCart), Times.Once);
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
                    new Item { Id = 2, Name = "Item 2", Price = 567, Quantity = 2 },
                    new Item { Id = 3, Name = "Item 3", Price = 891, Quantity = 5 },
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
                    new Item { Id = 222, Name = "Item 222", Price = 23, Quantity = 35 },
                    new Item { Id = 333, Name = "Item 333", Price = 42, Quantity = 6 },
                }
            },
        };
    }
}
