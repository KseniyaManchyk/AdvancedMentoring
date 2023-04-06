using CartingService.DAL.Interfaces;
using CartingService.Domain.Models;

namespace CartingService.DAL.Implementation;

public class CartsRepository : IRepository<Cart, string>
{
    private ILiteDBConnectionProvider _connectionProvider;

    public CartsRepository(ILiteDBConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public IEnumerable<Cart> GetAll()
    {
        using (var liteDatabase = _connectionProvider.GetConnection())
        {
            var cartsCollection = liteDatabase.GetCollection<Cart>();
            return cartsCollection.FindAll().ToList();
        }
    }

    public Cart GetById(string id)
    {
        using (var liteDatabase = _connectionProvider.GetConnection())
        {
            var cartsCollection = liteDatabase.GetCollection<Cart>();
            return cartsCollection.FindById(id);
        }
    }

    public void Add(string id, Cart item)
    {
        using (var liteDatabase = _connectionProvider.GetConnection())
        {
            var cartsCollection = liteDatabase.GetCollection<Cart>();
            cartsCollection.Insert(id, item);
            liteDatabase.Commit();
        }
    }

    public void Remove(string id)
    {
        using (var liteDatabase = _connectionProvider.GetConnection())
        {
            var cartsCollection = liteDatabase.GetCollection<Cart>();
            cartsCollection.Delete(id);
            liteDatabase.Commit();
        }
    }

    public void Update(string id, Cart item)
    {
        using (var liteDatabase = _connectionProvider.GetConnection())
        {
            var cartsCollection = liteDatabase.GetCollection<Cart>();
            cartsCollection.Update(id, item);
            liteDatabase.Commit();
        }
    }
}
