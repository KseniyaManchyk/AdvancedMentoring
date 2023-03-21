using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Services;

// Has almost the same logic as CategoryService, separated for the future
public class ProductService : IService<Product>
{
    private IRepository<Product> _productRepository;
    private AbstractValidator<Product> _productValidator;

    public ProductService(
        IRepository<Product> productRepository,
        AbstractValidator<Product> productValidator
        )
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Product>> GetByExpressionAsync(Func<Product, bool> predicate)
    {
        return await _productRepository.GetByExpressionAsync(predicate);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product item)
    {
        _productValidator.ValidateAndThrow(item);
        await _productRepository.AddAsync(item);
    }

    public async Task UpdateAsync(Product item)
    {
        _productValidator.ValidateAndThrow(item);
        await _productRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(Product item)
    {
        await _productRepository.DeleteAsync(item);
    }
}