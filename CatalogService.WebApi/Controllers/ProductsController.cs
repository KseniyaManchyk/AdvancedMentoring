using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using CatalogService.WebApi.Extensions;
using RabbitMQ.Interfaces;

namespace CatalogService.WebApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private const string categoriesPath = "categories";

        private readonly IService<Product> _productsService;
        private readonly IMessageProducer _messageProducer;
        private readonly IHelpUrlBuilder _helpUrlBuilder;

        public ProductsController(
            IService<Product> productsService,
            IMessageProducer messageProducer,
            IHelpUrlBuilder helpUrlBuilder)
        {
            _productsService = productsService;
            _messageProducer = messageProducer;
            _helpUrlBuilder = helpUrlBuilder;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productsService.GetAllAsync().ConfigureAwait(false);

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            var addedProduct = await _productsService.AddAsync(product).ConfigureAwait(false);

            return Created(_helpUrlBuilder.BuildUrl(Request), new ResponseModel<Product>
            {
                Items = new List<Product> { addedProduct },
                NextLink = _helpUrlBuilder.BuildUrl(Request, categoriesPath)
            });
        }

        [HttpPut]
        public async Task<ActionResult> Put(Product product)
        {
            await _productsService.UpdateAsync(product).ConfigureAwait(false);

            _messageProducer.SendMessage(new RabbitMQ.Models.Message
            {
                Id = Guid.NewGuid(),
                UpdatedItem = new RabbitMQ.Models.Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                }
            });

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Product product)
        {
            await _productsService.DeleteAsync(product).ConfigureAwait(false);

            return NoContent();
        }
    }
}