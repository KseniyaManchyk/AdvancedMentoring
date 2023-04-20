using CatalogService.BLL.Services;
using CatalogService.Domain.ExceptionHandling;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;
using MessageQueue.Interfaces;
using MessageQueue.Models;

namespace CatalogService.BLL.Tests;

public class ProductsServiceTests
{
    private IService<Product> _sut;
    private Mock<IRepository<Product>> _repositoryMock;
    private Mock<AbstractValidator<Product>> _validatorMock;
    private Mock<IMessageProducer> _messageProducerMock;

    public ProductsServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _validatorMock = new Mock<AbstractValidator<Product>>();
        _messageProducerMock = new Mock<IMessageProducer>();
        _sut = new ProductsService(_repositoryMock.Object, _validatorMock.Object, _messageProducerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldCallGetAllRepositoryMethod()
    {
        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Product> { new Product() });

        var result = await _sut.GetAllAsync();

        _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public async Task GetByExpressionAsync_ShouldCallGetByExpressionRepositoryMethod()
    {
        Func<Product, bool> predicate = (product) => product.Name == "test";

        _repositoryMock
            .Setup(x => x.GetByExpressionAsync(predicate))
            .ReturnsAsync(new List<Product> { new Product() });

        var result = await _sut.GetByExpressionAsync(predicate);

        _repositoryMock.Verify(x => x.GetByExpressionAsync(predicate), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldCallGetByIdRepositoryMethod()
    {
        var id = 1;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(new Product());

        var result = await _sut.GetByIdAsync(id);

        _repositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_ShouldCallAddRepositoryMethod()
    {
        var newProduct = new Product();

        await _sut.AddAsync(newProduct);

        _repositoryMock.Verify(x => x.AddAsync(newProduct), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallUpdateRepositoryMethod()
    {
        var product = new Product();

        await _sut.UpdateAsync(product);

        _messageProducerMock.Verify(x => x.SendMessage(It.IsAny<Message>()), Times.Once);
        _repositoryMock.Verify(x => x.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteRepositoryMethod()
    {
        var product = new Product();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        await _sut.DeleteAsync(product);

        _repositoryMock.Verify(x => x.DeleteAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryIsNotFound_ShouldThrowNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteAsync(new Product()));
    }
}