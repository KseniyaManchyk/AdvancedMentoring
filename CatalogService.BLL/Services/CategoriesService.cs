using CatalogService.Domain.ExceptionHandling;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Services;

public class CategoriesService : IService<Category>
{
    private IRepository<Category> _categoryRepository;
    private IService<Product> _productsService;
    private AbstractValidator<Category> _categoryValidator;

    public CategoriesService(
        IRepository<Category> categoryRepository,
        IService<Product> productsService,
        AbstractValidator<Category> categoryValidator
        )
    {
        _categoryRepository = categoryRepository;
        _productsService = productsService;
        _categoryValidator = categoryValidator;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Category>> GetByExpressionAsync(Func<Category, bool> predicate)
    {
        return await _categoryRepository.GetByExpressionAsync(predicate);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _categoryValidator.ValidateAndThrow(category);
        await _categoryRepository.AddAsync(category);

        var addedCategory = await _categoryRepository
                           .GetByExpressionAsync(c =>
                                                   c.Name == category.Name &&
                                                   c.ParentCategoryId == category.ParentCategoryId &&
                                                   c.Image == category.Image);

        return addedCategory.FirstOrDefault();
    }

    public async Task UpdateAsync(Category category)
    {
        _categoryValidator.ValidateAndThrow(category);
        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(Category category)
    {
        var foundCategory = await _categoryRepository.GetByIdAsync(category.Id);

        if (foundCategory is null)
        {
            throw new NotFoundException(nameof(Category));
        }

        foreach (var product in category.Products)
        {
            await _productsService.DeleteAsync(product);
        }

        await _categoryRepository.DeleteAsync(category);
    }
}