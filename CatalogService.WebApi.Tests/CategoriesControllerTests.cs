using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using CatalogService.WebApi.Controllers;
using CatalogService.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogService.WebApi.Tests
{
    public class CategoriesControllerTests
    {
        private CategoriesController _sut;
        private Mock<IService<Category>> _categoriesServiceMock;

        public CategoriesControllerTests()
        {
            _categoriesServiceMock = new Mock<IService<Category>>();
            var helpUrlBuilder = new Mock<IHelpUrlBuilder>();

            _sut = new CategoriesController(_categoriesServiceMock.Object, helpUrlBuilder.Object);
        }

        [Fact]
        public async Task Get_ShouldCallGetAllServiceMethod()
        {
            var result = await _sut.Get();

            _categoriesServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_ShouldCallAddServiceMethod()
        {
            var newCategory = new Category();

            var result = await _sut.Post(newCategory);

            _categoriesServiceMock.Verify(x => x.AddAsync(newCategory), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ShouldCallUpdateServiceMethod()
        {
            var category = new Category();

            var result = await _sut.Put(category);

            _categoriesServiceMock.Verify(x => x.UpdateAsync(category), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldCallDeleteServiceMethod()
        {
            var category = new Category();

            var result = await _sut.Delete(category);

            _categoriesServiceMock.Verify(x => x.DeleteAsync(category), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}