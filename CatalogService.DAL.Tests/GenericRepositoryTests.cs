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
        public async Task GetAll_WhenCategoriesExist_ShouldReturnAllCategories()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);

            var result = await repository.GetAllAsync();

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task GetByExpression_WhenApropriateDataExist_ShouldReturnData()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            Func<Category, bool> predicate = (category) => category.Name.StartsWith(CATEGORY_NAME_PREFIX);

            var result = await repository.GetByExpressionAsync(predicate);

            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task GetByExpression_WhenApropriateDataDoNotExist_ShouldReturnEmptyCollection()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            Func<Category, bool> predicate = (category) => category.Name == "test";

            var result = await repository.GetByExpressionAsync(predicate);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetById_WhenCategoryExist_ShouldReturnCategoryById()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            var id = 1;

            var result = await repository.GetByIdAsync(id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_ShouldAddRecord()
        {
            using var context = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(context);
            var newCategory = GenerateData();

            await repository.AddAsync(newCategory);

            var addedCategory = await repository.GetByIdAsync(newCategory.Id);
            Assert.NotNull(addedCategory);
            Assert.True(
                newCategory.Id == addedCategory.Id &&
                newCategory.Name == addedCategory.Name
                );
        }

        [Fact]
        public async Task Update_ShouldUpdateRecord()
        {
            using var firstContext = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(firstContext);
            var category = GenerateData();
            await repository.AddAsync(category);

            using var secondContext = new CatalogServiceContext(_contextOptions);
            repository = new GenericRepository<Category>(secondContext);
            var newCategoryName = $"{CATEGORY_NAME_PREFIX}_updated_by_test";
            var updatingCategory = new Category
            {
                Id = category.Id,
                Name = newCategoryName,
            };
            
            await repository.UpdateAsync(updatingCategory);

            var updatedCategory = await repository.GetByIdAsync(category.Id);
            Assert.True(updatedCategory.Name == newCategoryName);
        }

        [Fact]
        public async Task Delete_ShouldDeleteRecord()
        {
            using var firstContext = new CatalogServiceContext(_contextOptions);
            var repository = new GenericRepository<Category>(firstContext);
            var category = GenerateData();

            await repository.AddAsync(category);
            var addedCategory = await repository.GetByIdAsync(category.Id);

            Assert.NotNull(addedCategory);

            using var secondContext = new CatalogServiceContext(_contextOptions);
            repository = new GenericRepository<Category>(secondContext);
            await repository.DeleteAsync(category);
            var removedCategory = await repository.GetByIdAsync(category.Id);

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