using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;

namespace CatalogService.BLL.Services;

// Has almost the same logic as ProductService, separated for the future
public class CategoryService : IService<Category>
{
    private IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categoryRepository.GetAll();
    }

    public IEnumerable<Category> GetByExpression(Func<Category, bool> predicate)
    {
        return _categoryRepository.GetByExpression(predicate);
    }

    public Category GetById(int id)
    {
        return _categoryRepository.GetById(id);
    }

    public void Add(Category item)
    {
        _categoryRepository.Add(item);
    }

    public void Update(Category item)
    {
        _categoryRepository.Update(item);
    }

    public void Delete(Category item)
    {
        _categoryRepository.Delete(item);
    }
}