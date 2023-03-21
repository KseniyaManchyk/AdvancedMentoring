using CatalogService.BLL.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;

namespace CatalogService.BLL.Tests;

public class ProductServiceTests
{
    private IService<Product> _sut;
    private Mock<IRepository<Product>> _repositoryMock;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _sut = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public void GetAll_ShouldCallGetAllRepositoryMethod()
    {
        _repositoryMock
            .Setup(x => x.GetAll())
            .Returns(new List<Product> { new Product() });

        var result = _sut.GetAll();

        _repositoryMock.Verify(x => x.GetAll(), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public void GetByExpression_ShouldCallGetByExpressionRepositoryMethod()
    {
        Func<Product, bool> predicate = (product) => product.Name == "test";

        _repositoryMock
            .Setup(x => x.GetByExpression(predicate))
            .Returns(new List<Product> { new Product() });

        var result = _sut.GetByExpression(predicate);

        _repositoryMock.Verify(x => x.GetByExpression(predicate), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public void GetById_ShouldCallGetByIdRepositoryMethod()
    {
        var id = 1;

        _repositoryMock
            .Setup(x => x.GetById(id))
            .Returns(new Product());

        var result = _sut.GetById(id);

        _repositoryMock.Verify(x => x.GetById(id), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public void Add_ShouldCallAddRepositoryMethod()
    {
        var newProduct = new Product();

        _sut.Add(newProduct);

        _repositoryMock.Verify(x => x.Add(newProduct), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallUpdateRepositoryMethod()
    {
        var product = new Product();

        _sut.Update(product);

        _repositoryMock.Verify(x => x.Update(product), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallDeleteRepositoryMethod()
    {
        var product = new Product();

        _sut.Delete(product);

        _repositoryMock.Verify(x => x.Delete(product), Times.Once);
    }
}