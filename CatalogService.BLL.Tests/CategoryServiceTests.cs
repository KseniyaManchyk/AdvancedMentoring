using CatalogService.BLL.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Tests;

public class CategoryServiceTests
{
    private IService<Category> _sut;
    private Mock<IRepository<Category>> _repositoryMock;
    private Mock<AbstractValidator<Category>> _validatorMock;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Category>>();
        _validatorMock = new Mock<AbstractValidator<Category>>();
        _sut = new CategoryService(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldCallGetAllRepositoryMethod()
    {
        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Category> { new Category() });

        var result = await _sut.GetAllAsync ();

        _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        Assert.True(result.Count() != 0);
    }

    [Fact]
    public async Task GetByExpressionAsync_ShouldCallGetByExpressionRepositoryMethod()
    {
        Func<Category, bool> predicate = (category) => category.Name == "test";

        _repositoryMock
            .Setup(x => x.GetByExpressionAsync(predicate))
            .ReturnsAsync(new List<Category> { new Category() });

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
            .ReturnsAsync(new Category());

        var result = await _sut.GetByIdAsync(id);

        _repositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_ShouldCallAddRepositoryMethod()
    {
        var newCategory = new Category();

        await _sut.AddAsync(newCategory);

        _repositoryMock.Verify(x => x.AddAsync(newCategory), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallUpdateRepositoryMethod()
    {
        var category = new Category();

        await _sut.UpdateAsync(category);

        _repositoryMock.Verify(x => x.UpdateAsync(category), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteRepositoryMethod()
    {
        var category = new Category();

        await _sut.DeleteAsync(category);

        _repositoryMock.Verify(x => x.DeleteAsync(category), Times.Once);
    }
}