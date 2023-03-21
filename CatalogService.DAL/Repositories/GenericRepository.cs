using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return 
            await _dbSet.AsNoTracking()
                        .ToAsyncEnumerable()
                        .ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByExpressionAsync(Func<TEntity, bool> predicate)
    {
        return 
            await _dbSet.AsNoTracking()
                        .Where(predicate)
                        .ToAsyncEnumerable()
                        .ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity item)
    {
        _dbSet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity item)
    {
        _context.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity item)
    {
        _dbSet.Remove(item);
        await _context.SaveChangesAsync();
    }
}
