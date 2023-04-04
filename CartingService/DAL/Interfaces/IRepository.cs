namespace CartingService.DAL.Interfaces;

public interface IRepository<TEntity, TIdentifier> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    TEntity GetById(TIdentifier id);

    void Add(TIdentifier id, TEntity item);

    void Update(TIdentifier id, TEntity item);

    void Remove(TIdentifier id);
}
