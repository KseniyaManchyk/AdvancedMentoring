using CartingService.BLL.Interfaces;
using CartingService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.WebApi.Controllers
{
    /// <summary>
    /// Allows to work with carts.
    /// </summary>
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

        /// <summary>
        /// Returns Cart information by cartId.
        /// </summary>
        /// <param name="cartId">Unique identifier of cart.</param>
        /// <returns>Returns cart model.</returns>
        /// <response code="200">Returns found carts.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("{cartId}")]
        public ActionResult GetCart([FromRoute] string cartId)
        {
            var cart = _cartsService.GetCartById(cartId);

            return Ok(cart);
        }

        /// <summary>
        /// Returns Carts list.
        /// </summary>
        /// <returns>Returns list of cart models.</returns>
        /// <response code="200">Returns found carts.</response>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public ActionResult GetCarts()
        {
            var carts = _cartsService.GetCarts();

            return Ok(carts);
        }

        /// <summary>
        /// Adds item into cart by cartId.
        /// </summary>
        /// <param name="cartId">Unique identifier of cart.</param>
        /// <param name="item">Item to add into Cart.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /carts/cartId
        ///     {
        ///        "id": cartId,
        ///        "name": "Item #1",
        ///        "price": 123,
        ///        "quantity": 12,
        ///        "image": {
        ///             "altText": "Image #1",
        ///             "URL": "https://url"
        ///        }
        ///     }
        /// </remarks>
        /// <response code="200">Item was added sucessfully.</response>
        [HttpPost]
        [Route("{cartId}")]
        public ActionResult AddItemToCart([FromRoute] string cartId, [FromForm] Item item)
        {
            _cartsService.AddItemToCart(cartId, item);

            return Ok();
        }

        /// <summary>
        /// Deletes item from cart using cartId and itemId.
        /// </summary>
        /// <param name="cartId">Unique identifier of cart.</param>
        /// <param name="itemId">Unique identifier of item.</param>
        /// <response code="200">Item was deleted sucessfully.</response>
        [HttpDelete]
        [Route("{cartId}/{itemId}")]
        public ActionResult DeleteItemFromCart([FromRoute] string cartId, [FromRoute] int itemId)
        {
            _cartsService.RemoveItemFromCart(cartId, itemId);

            return Ok();
        }
    }
}