using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/v{version:apiVersion}/carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartsService _cartsService;
        private readonly ILogger<CartsController> _logger;

        public CartsController(ICartsService cartingService, ILogger<CartsController> logger)
        {
            _cartsService = cartingService;
            _logger = logger;
        }

        /// <summary>
        /// Returns Cart information by cartId.
        /// </summary>
        /// <param name="cartId">Unique identifier of cart.</param>
        /// <returns>Returns cart model.</returns>
        /// <response code="200">Returns found carts.</response>
        /// <response code="400">API couldn't handle request due to thrown exceptions.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("{cartId}")]
        public ActionResult GetCart([FromRoute] string cartId)
        {
            _logger.LogInformation($"Start getting cart with id {cartId}.");

            var cart = _cartsService.GetCartById(cartId);

            return Ok(cart);
        }

        /// <summary>
        /// Returns Carts list.
        /// </summary>
        /// <returns>Returns list of cart models.</returns>
        /// <response code="200">Returns found carts.</response>
        /// <response code="400">API couldn't handle request due to thrown exceptions.</response>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public ActionResult GetCarts()
        {
            _logger.LogInformation("Start getting all carts.");

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
        /// <response code="400">API couldn't handle request due to thrown exceptions.</response>
        [HttpPost]
        [Route("{cartId}")]
        public ActionResult AddItemToCart([FromRoute] string cartId, [FromForm] Item item)
        {
            _logger.LogInformation($"Start adding new cart with id {cartId}.");

            _cartsService.AddItemToCart(cartId, item);

            _logger.LogInformation($"Cart with id {cartId} was added.");

            return Ok();
        }

        /// <summary>
        /// Deletes item from cart using cartId and itemId.
        /// </summary>
        /// <param name="cartId">Unique identifier of cart.</param>
        /// <param name="itemId">Unique identifier of item.</param>
        /// <response code="200">Item was deleted sucessfully.</response>
        /// <response code="404">Cart or item does not exist in the system.</response>
        /// <response code="400">API couldn't handle request due to thrown exceptions.</response>
        [HttpDelete]
        [Route("{cartId}/{itemId}")]
        public ActionResult DeleteItemFromCart([FromRoute] string cartId, [FromRoute] int itemId)
        {
            _logger.LogInformation($"Start deleting item with id {itemId} from cart with id {cartId}.");

            _cartsService.RemoveItemFromCart(cartId, itemId);

            _logger.LogInformation($"Item with id {itemId} from cart with id {cartId} was deleted.");

            return Ok();
        }
    }
}