using CartingService.BLL.Interfaces;
using CartingService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartsService _cartsService;

        public CartsController(ICartsService cartingService)
        {
            _cartsService = cartingService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("{cartId}")]
        public ActionResult GetCart([FromRoute] string cartId)
        {
            var cart = _cartsService.GetCartById(cartId);

            return Ok(cart);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public ActionResult GetCarts()
        {
            var carts = _cartsService.GetCarts();

            return Ok(carts);
        }

        [HttpPost]
        [Route("{cartId}")]
        public ActionResult AddItemToCart([FromRoute] string cartId, [FromForm] Item item)
        {
            _cartsService.AddItemToCart(cartId, item);

            return Ok();
        }

        [HttpDelete]
        [Route("{cartId}/{itemId}")]
        public ActionResult DeleteItemFromCart([FromRoute] string cartId, [FromRoute] int itemId)
        {
            _cartsService.RemoveItemFromCart(cartId, itemId);

            return Ok();
        }
    }
}