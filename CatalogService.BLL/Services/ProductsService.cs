using CatalogService.Domain.ExceptionHandling;
using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using FluentValidation;
using MessageQueue.Interfaces;
using MessageQueue.Models;

namespace CatalogService.BLL.Services;

public class ProductsService : IService<Product>
{
    private readonly IRepository<Product> _productRepository;
    private readonly AbstractValidator<Product> _productValidator;
    private readonly IMessageProducer _messageProducer;

    public ProductsService(
        IRepository<Product> productRepository,
        AbstractValidator<Product> productValidator,
        IMessageProducer messageProducer
        )
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
        _messageProducer = messageProducer;
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

    public async Task<Product> AddAsync(Product product)
    {
        _productValidator.ValidateAndThrow(product);
        await _productRepository.AddAsync(product);

        var addedProduct = await _productRepository
                           .GetByExpressionAsync(p =>
                                                   p.Name == product.Name &&
                                                   p.Description == product.Description &&
                                                   p.Price == product.Price &&
                                                   p.Amount == product.Amount &&
                                                   p.CategoryId == product.CategoryId &&
                                                   p.Image == product.Image);

       return addedProduct.FirstOrDefault();
    }

    public async Task UpdateAsync(Product product)
    {
        _productValidator.ValidateAndThrow(product);
        await _productRepository.UpdateAsync(product);

        _messageProducer.SendMessage(new Message
        {
            Id = Guid.NewGuid(),
            UpdatedItem = new Item
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
            }
        });
    }

    public async Task DeleteAsync(Product product)
    {
        var foundProduct = await _productRepository.GetByIdAsync(product.Id);

        if (foundProduct is null)
        {
            throw new NotFoundException(nameof(Product));
        }

        await _productRepository.DeleteAsync(product);
    }
}