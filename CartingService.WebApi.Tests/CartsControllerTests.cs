using CartingService.Domain.Models;

namespace CartingService.WebApi.Tests
{
    public class CartsControllerTests
    {
        private CartsController _sut;
        private Mock<ICartsService> _cartsServiceMock;

        public CartsControllerTests()
        {
            _cartsServiceMock = new Mock<ICartsService>();

            _sut = new CartsController(_cartsServiceMock.Object);
        }

        [Fact]
        public void GetCart_ShouldCallGetCartByIdServiceMethod()
        {
            var cartId = "test";

            var result = _sut.GetCart(cartId);

            _cartsServiceMock.Verify(x => x.GetCartById(cartId), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCarts_ShouldCallGetCartsServiceMethod()
        {
            var result = _sut.GetCarts();

            _cartsServiceMock.Verify(x => x.GetCarts(), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddItemToCart_ShouldCallAddItemToCartServiceMethod()
        {
            var cartId = "test";
            var item = new Item();

            var result = _sut.AddItemToCart(cartId, item);

            _cartsServiceMock.Verify(x => x.AddItemToCart(cartId, item), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteItemFromCart_ShouldCallRemoveItemFromCartServiceMethod()
        {
            var id = 1;

            var result = _sut.DeleteItemFromCart(id.ToString(), id);

            _cartsServiceMock.Verify(x => x.RemoveItemFromCart(id.ToString(), id), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}