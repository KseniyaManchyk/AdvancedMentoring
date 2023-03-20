using CartingService.DAL.Implementation;
using CartingService.DAL.Interfaces;
using CartingService.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CartingService.Tests.IntegrationTests
{
    public class CartingRepositoryIntegrationTests : IDisposable
    {
        private IRepository<Cart> _sut;

        public CartingRepositoryIntegrationTests()
        {
            _sut = new CartingRepository(new LiteDBConnectionProvider(GetTestDBPath()));
        }

        public void Dispose()
        {
            var dbPath = GetTestDBPath();
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }

        [Fact]
        public void GetAllAndAdd_ShouldReturnCartsCollection()
        {
            var cartId = GenerateId();
            var newCart = new Cart
            {
                Items = new List<Item> {
                    new Item { Id = 1, Name = "Item 1", Price = 123, Quantity = 23 },
                }
            };

            _sut.Add(cartId, newCart);
            _sut.Add(cartId + 1, newCart);

            var result = _sut.GetAll();

            Assert.True(result.Count() > 1);
        }

        [Fact]
        public void GetById_WhenCartExists_ShouldReturnCorrectCart()
        {
            var cartId = GenerateId();
            var newCart = new Cart
            {
                Items = new List<Item> {
                    new Item { Id = 1, Name = "Item 1", Price = 123, Quantity = 23 },
                }
            };

            _sut.Add(cartId, newCart);
            _sut.Add(cartId + 1, newCart);

            var result = _sut.GetById(cartId);

            Assert.NotNull(result);
        }

        [Fact]
        public void Remove_ShouldCallDeleteAndCommitMethods()
        {
            var cartId = GenerateId();
            var newCart = new Cart
            {
                Items = new List<Item> {
                    new Item { Id = 1, Name = "Item 1", Price = 123, Quantity = 23 },
                }
            };

            _sut.Add(cartId, newCart);
            _sut.Add(cartId + 1, newCart);

            _sut.Remove(cartId);

            var result = _sut.GetById(cartId);

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldCallUpdateAndCommitMethods()
        {
            var cartId = GenerateId();
            var oldCart = new Cart
            {
                Items = new List<Item> {
                    new Item { Id = 1, Name = "Item 1", Price = 123, Quantity = 23 },
                }
            };
            var updatedName = "Updated Item 1";
            var updatedCart = new Cart
            {
                Items = new List<Item> {
                    new Item { Id = 1, Name = updatedName, Price = 123, Quantity = 23 },
                }
            };

            _sut.Add(cartId, oldCart);

            var notUpdatedCart = _sut.GetById(cartId);

            Assert.True(notUpdatedCart.Items.First().Name != updatedName);

            _sut.Update(cartId, updatedCart);

            var updatedCartResult = _sut.GetById(cartId);

            Assert.True(updatedCartResult.Items.First().Name == updatedName);
        }

        private string GetTestDBPath()
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            return $@"{projectPath}/integration_tests.db";
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next();
        }
    }
}
