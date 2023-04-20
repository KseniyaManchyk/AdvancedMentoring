using MessageQueue.Interfaces;

namespace CatalogService.WebApi.Tests
{
    public class ProductsControllerTests
    {
        private ProductsController _sut;
        private Mock<IService<Product>> _productsServiceMock;
        private Mock<IHelpUrlBuilder> _helpUrlBuilder;

        public ProductsControllerTests()
        {
            _productsServiceMock = new Mock<IService<Product>>();
            _helpUrlBuilder = new Mock<IHelpUrlBuilder>();

            _sut = new ProductsController(_productsServiceMock.Object, _helpUrlBuilder.Object);
        }

        [Fact]
        public async Task Get_ShouldCallGetAllServiceMethod()
        {
            var result = await _sut.Get();

            _productsServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Post_ShouldCallAddServiceMethod()
        {
            var newProduct = new Product();

            _helpUrlBuilder
                .Setup(x => x.BuildUrl(It.IsAny<HttpRequest>(), It.IsAny<string>()))
                .Returns("https://test");

            var result = await _sut.Post(newProduct);

            _productsServiceMock.Verify(x => x.AddAsync(newProduct), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Put_ShouldCallUpdateServiceMethod()
        {
            var product = new Product();

            var result = await _sut.Put(product);

            _productsServiceMock.Verify(x => x.UpdateAsync(product), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldCallDeleteServiceMethod()
        {
            var product = new Product();

            var result = await _sut.Delete(product);

            _productsServiceMock.Verify(x => x.DeleteAsync(product), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}