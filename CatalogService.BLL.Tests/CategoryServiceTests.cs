using CatalogService.BLL.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;

namespace CatalogService.BLL.Tests;

public class CategoryServiceTests
{
    private IService<Category> _sut;
    private Mock<IRepository<Category>> _repositoryMock;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Category>>();
        _sut = new CategoryService(_repositoryMock.Object);
    }

    [Fact]
    public void GetAll_ShouldCallGetAllRepositoryMethod()
    {
        _repositoryMock
            .Setup(x => x.GetAll())
            .Returns(new List<Category> { new Category() });

        var result = _sut.GetAll();

        _repositoryMock.Verify(x => x.GetAll(), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public void GetByExpression_ShouldCallGetByExpressionRepositoryMethod()
    {
        Func<Category, bool> predicate = (category) => category.Name == "test";

        _repositoryMock
            .Setup(x => x.GetByExpression(predicate))
            .Returns(new List<Category> { new Category() });

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
            .Returns(new Category());

        var result = _sut.GetById(id);

        _repositoryMock.Verify(x => x.GetById(id), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public void Add_ShouldCallAddRepositoryMethod()
    {
        var newCategory = new Category();

        _sut.Add(newCategory);

        _repositoryMock.Verify(x => x.Add(newCategory), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallUpdateRepositoryMethod()
    {
        var category = new Category();

        _sut.Update(category);

        _repositoryMock.Verify(x => x.Update(category), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallDeleteRepositoryMethod()
    {
        var category = new Category();

        _sut.Delete(category);

        _repositoryMock.Verify(x => x.Delete(category), Times.Once);
    }
}