using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;

namespace CatalogService.BLL.Services;

// Has almost the same logic as CategoryService, separated for the future
public class ProductService : IService<Product>
{
    private IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetAll()
    {
        return _productRepository.GetAll();
    }

    public IEnumerable<Product> GetByExpression(Func<Product, bool> predicate)
    {
        return _productRepository.GetByExpression(predicate);
    }

    public Product GetById(int id)
    {
        return _productRepository.GetById(id);
    }

    public void Add(Product item)
    {
        _productRepository.Add(item);
    }

    public void Update(Product item)
    {
        _productRepository.Update(item);
    }

    public void Delete(Product item)
    {
        _productRepository.Delete(item);
    }
}