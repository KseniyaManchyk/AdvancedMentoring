using System.Linq.Expressions;

namespace CatalogService.Domain.Interfaces;

public interface IService<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    IEnumerable<TEntity> GetByExpression(Func<TEntity, bool> predicate);

    TEntity GetById(int id);

    void Add(TEntity item);

    void Update(TEntity item);

    void Delete(TEntity item);
}
