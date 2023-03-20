using CartingService.DAL.Interfaces;
using CartingService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CartingService.DAL.Implementation
{
    public class CartingRepository : IRepository<Cart>
    {
        private ILiteDBConnectionProvider _connectionProvider;

        public CartingRepository(ILiteDBConnectionProvider connectionProvider)
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

        public Cart GetById(int id)
        {
            using (var liteDatabase = _connectionProvider.GetConnection())
            {
                var cartsCollection = liteDatabase.GetCollection<Cart>();
                return cartsCollection.FindById(id);
            }
        }

        public void Add(int id, Cart item)
        {
            using (var liteDatabase = _connectionProvider.GetConnection())
            {
                var cartsCollection = liteDatabase.GetCollection<Cart>();
                cartsCollection.Insert(id, item);
                liteDatabase.Commit();
            }
        }

        public void Remove(int id)
        {
            using (var liteDatabase = _connectionProvider.GetConnection())
            {
                var cartsCollection = liteDatabase.GetCollection<Cart>();
                cartsCollection.Delete(id);
                liteDatabase.Commit();
            }
        }

        public void Update(int id, Cart item)
        {
            using (var liteDatabase = _connectionProvider.GetConnection())
            {
                var cartsCollection = liteDatabase.GetCollection<Cart>();
                cartsCollection.Update(id, item);
                liteDatabase.Commit();
            }
        }
    }
}
