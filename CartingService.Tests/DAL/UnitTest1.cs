using CartingService.DAL.Implementation;
using CartingService.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace CartingService.Tests.DAL
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var repository = new CartingRepository(new LiteDBConnectionProvider(@"C:\Users\Kseniya\source\repos\AdvancedMentoring\save.db"));
            //repository.Add(2, new Cart
            //{
            //    Items = new List<Item>
            //    {
            //        new Item { Id = 1, Name = "test", Price = 12.54m, Quantity = 13 },
            //        new Item { Id = 2, Name = "test1", Price = 11.54m, Quantity = 1 },
            //        new Item { Id = 3, Name = "test2", Price = 14.54m, Quantity = 18 },
            //    }
            //});
            var carts = repository.GetAll();
        }
    }
}
