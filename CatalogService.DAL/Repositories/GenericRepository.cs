using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Domain.Interfaces;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    DbContext _context;
    DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.AsNoTracking().ToList();
    }

    public IEnumerable<TEntity> GetByExpression(Func<TEntity, bool> predicate)
    {
        return _dbSet.AsNoTracking().Where(predicate).ToList();
    }

    public TEntity GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Add(TEntity item)
    {
        _dbSet.Add(item);
        _context.SaveChanges();
    }

    public void Update(TEntity item)
    {
        //_context.Entry(item).State = EntityState.Modified;
        _context.Update(item);
        _context.SaveChanges();
    }

    public void Delete(TEntity item)
    {
        _dbSet.Remove(item);
        _context.SaveChanges();
    }
}
