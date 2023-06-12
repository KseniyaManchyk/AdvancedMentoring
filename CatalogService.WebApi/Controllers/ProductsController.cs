using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using CatalogService.WebApi.Extensions;
using CatalogService.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogService.WebApi.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private const string categoriesPath = "categories";

        private readonly IService<Product> _productsService;
        private readonly IHelpUrlBuilder _helpUrlBuilder;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IService<Product> productsService,
            IHelpUrlBuilder helpUrlBuilder,
            ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _helpUrlBuilder = helpUrlBuilder;
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            _logger.LogInformation("Start getting all products.");

            var products = await _productsService.GetAllAsync().ConfigureAwait(false);

            return Ok(products);
        }

        [HttpGet]
        [Route("properties")]
        public async Task<ActionResult<IEnumerable<Product>>> GetItemProperties(int itemId)
        {
            _logger.LogInformation("Start getting item properties.");

            return Ok(new Dictionary<string, string>
            {
                { "id", itemId.ToString() },
                { "prop1", "prop1 value" },
                { "prop2", "prop2 value" },
                { "prop3", "prop3 value" },
            });
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Post(Product product)
        {
            _logger.LogInformation($"Start adding new product with name {product.Name}.");

            var addedProduct = await _productsService.AddAsync(product).ConfigureAwait(false);

            _logger.LogInformation($"Product with name {product.Name} was added.");

            return Created(_helpUrlBuilder.BuildUrl(Request), new ResponseModel<Product>
            {
                Items = new List<Product> { addedProduct },
                NextLink = _helpUrlBuilder.BuildUrl(Request, categoriesPath)
            });
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Put(Product product)
        {
            _logger.LogInformation($"Start updating product with name {product.Name}.");

            await _productsService.UpdateAsync(product).ConfigureAwait(false);

            _logger.LogInformation($"Product with id {product.Id} was updated.");

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(Product product)
        {
            _logger.LogInformation($"Start deleting product with name {product.Name}.");

            await _productsService.DeleteAsync(product).ConfigureAwait(false);

            _logger.LogInformation($"Product with name {product.Name} was deleted.");

            return NoContent();
        }
    }
}