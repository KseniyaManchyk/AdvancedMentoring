namespace CatalogService.Domain.Interfaces;

public interface IService<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetByExpressionAsync(Func<TEntity, bool> predicate);

    Task<TEntity> GetByIdAsync(int id);

    Task AddAsync(TEntity item);

    Task UpdateAsync(TEntity item);

    Task DeleteAsync(TEntity item);
}
