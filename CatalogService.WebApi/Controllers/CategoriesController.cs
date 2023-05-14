using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Models;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using CatalogService.WebApi.Extensions;

namespace CatalogService.WebApi.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private const string productsPath = "products";

        private readonly IService<Category> _categoriesService;
        private readonly IHelpUrlBuilder _helpUrlBuilder;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            IService<Category> categoriesService,
            IHelpUrlBuilder helpUrlBuilder,
            ILogger<CategoriesController> logger)
        {
            _categoriesService = categoriesService;
            _helpUrlBuilder = helpUrlBuilder;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            _logger.LogInformation("Start getting all categories.");

            var categories = await _categoriesService.GetAllAsync().ConfigureAwait(false);

            return Ok(new ResponseModel<Category>
            {
                Items = categories,
                NextLink = _helpUrlBuilder.BuildUrl(Request, productsPath)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post(Category category)
        {
            _logger.LogInformation($"Start adding new category with name {category.Name}.");

            var addedCategory = await _categoriesService.AddAsync(category).ConfigureAwait(false);

            _logger.LogInformation($"Category with name {category.Name} was added.");

            return Created(_helpUrlBuilder.BuildUrl(Request), new ResponseModel<Category>
            {
                Items = new List<Category> { addedCategory },
                NextLink = _helpUrlBuilder.BuildUrl(Request, productsPath)
            });
        }

        [HttpPut]
        public async Task<ActionResult> Put(Category category)
        {
            _logger.LogInformation($"Start updating category with name {category.Name}.");

            await _categoriesService.UpdateAsync(category).ConfigureAwait(false);

            _logger.LogInformation($"Category with id {category.Id} was updated.");

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Category category)
        {
            _logger.LogInformation($"Start deleting category with name {category.Name}.");

            await _categoriesService.DeleteAsync(category).ConfigureAwait(false);

            _logger.LogInformation($"Category with name {category.Name} was deleted.");

            return NoContent();
        }
    }
}