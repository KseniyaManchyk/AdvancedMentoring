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

        public CategoriesController(
            IService<Category> categoriesService,
            IHelpUrlBuilder helpUrlBuilder)
        {
            _categoriesService = categoriesService;
            _helpUrlBuilder = helpUrlBuilder;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
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
            await _categoriesService.AddAsync(category).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Put(Category category)
        {
            await _categoriesService.UpdateAsync(category).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Category category)
        {
            await _categoriesService.DeleteAsync(category).ConfigureAwait(false);

            return NoContent();
        }
    }
}