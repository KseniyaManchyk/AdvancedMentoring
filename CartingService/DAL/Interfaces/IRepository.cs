using System.Collections.Generic;

namespace CartingService.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        void Add(int id, TEntity item);

        void Update(int id, TEntity item);

        void Remove(int id);
    }
}
