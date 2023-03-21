using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CatalogService.DAL.Tests
{
    public class GenericRepositoryTests
    {
        private const string CATEGORY_NAME_PREFIX = "category_prefix";
        private int categoriesCount = 0;

        private DbContextOptions<CatalogServiceContext> _contextOptions;

        public GenericRepositoryTests()
        {
            _contextOptions = new DbContextOptionsBuilder<CatalogServiceContext>()
                .UseInMemoryDatabase("CatalogServiceGenericRepositoryTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new CatalogServiceContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(GenerateData(2));
            context.SaveChanges();
        }

        [Fact]
        public void GetAll_WhenCategoriesExist_ShouldReturnAllCategories()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);

            var result = repository.GetAll();

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetByExpression_WhenApropriateDataExist_ShouldReturnData()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            Func<Category, bool> predicate = (category) => category.Name.StartsWith(CATEGORY_NAME_PREFIX);

            var result = repository.GetByExpression(predicate);

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public void GetByExpression_WhenApropriateDataDoNotExist_ShouldReturnEmptyCollection()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            Func<Category, bool> predicate = (category) => category.Name == "test";

            var result = repository.GetByExpression(predicate);

            Assert.Empty(result);
        }

        [Fact]
        public void GetById_WhenCategoryExist_ShouldReturnCategoryById()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            var id = 1;

            var result = repository.GetById(id);

            Assert.NotNull(result);
        }

        [Fact]
        public void Add_ShouldAddRecord()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            var newCategory = GenerateData();

            repository.Add(newCategory);

            var addedCategory = repository.GetById(newCategory.Id);
            Assert.NotNull(addedCategory);
            Assert.True(
                newCategory.Id == addedCategory.Id &&
                newCategory.Name == addedCategory.Name
                );
        }

        [Fact]
        public void Update_ShouldUpdateRecord()
        {
            using var firstContext = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(firstContext);
            var category = GenerateData();
            repository.Add(category);

            using var secondContext = new CatalogServiceContext(_contextOptions);
            repository = new GenericRepository<Category>(secondContext);
            var newCategoryName = $"{CATEGORY_NAME_PREFIX}_updated_by_test";
            var updatingCategory = new Category
            {
                Id = category.Id,
                Name = newCategoryName,
            };
            
            repository.Update(updatingCategory);

            var updatedCategory = repository.GetById(category.Id);
            Assert.True(updatedCategory.Name == newCategoryName);
        }

        [Fact]
        public void Delete_ShouldDeleteRecord()
        {
            using var firstContext = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(firstContext);
            var category = GenerateData();

            repository.Add(category);
            var addedCategory = repository.GetById(category.Id);

            Assert.NotNull(addedCategory);

            using var secondContext = new CatalogServiceContext(_contextOptions);
            repository = new GenericRepository<Category>(secondContext);
            repository.Delete(category);
            var removedCategory = repository.GetById(category.Id);

            Assert.Null(removedCategory);
        }

        private IEnumerable<Category> GenerateData(int itemsCount)
        {
            for (int i = 0; i < itemsCount; i++)
            {
                yield return GenerateData();
            }
        }

        private Category GenerateData()
        {
            categoriesCount++;
            return new Category { Id = categoriesCount, Name = $"{CATEGORY_NAME_PREFIX}_{categoriesCount}" };
        }
    }
}