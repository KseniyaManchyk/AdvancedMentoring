using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Services;

// Has almost the same logic as ProductService, separated for the future
public class CategoryService : IService<Category>
{
    private IRepository<Category> _categoryRepository;
    private AbstractValidator<Category> _categoryValidator;

    public CategoryService(
        IRepository<Category> categoryRepository,
        AbstractValidator<Category> categoryValidator
        )
    {
        _categoryRepository = categoryRepository;
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

    public async Task AddAsync(Category item)
    {
        _categoryValidator.ValidateAndThrow(item);
        await _categoryRepository.AddAsync(item);
    }

    public async Task UpdateAsync(Category item)
    {
        _categoryValidator.ValidateAndThrow(item);
        await _categoryRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(Category item)
    {
        await _categoryRepository.DeleteAsync(item);
    }
}