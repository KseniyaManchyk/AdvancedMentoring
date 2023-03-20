namespace CatalogService.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    TEntity Get(int id);

    void Add(int id, TEntity item);

    void Update(int id, TEntity item);

    void Delete(int id);
}
