using CartingService.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartingController : ControllerBase
    {
        private readonly ICartingService _cartingService;
        private readonly ILogger<CartingController> _logger;

        public CartingController(
            ICartingService cartingService,
            ILogger<CartingController> logger)
        {
            _logger = logger;
            _cartingService = cartingService;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}