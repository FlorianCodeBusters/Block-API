using Blocks_api.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Blocks_api.Controllers
{
    [ApiController]
//    [Authorize(Policy = "StandardRights")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly Blocks_api.Logger.IBlockLogger _logger;
        public WeatherForecastController(Blocks_api.Logger.IBlockLogger logger)
        {
            _logger = logger;
        }

        [HttpGet("SecretAdminWeather")]
        [Authorize(Policy = "ElevatedRights")]
        public IActionResult GetSecretAdminWeather()
        {
            return Ok("This is a secret admin weather forecast");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            Log.Information("Weather forecast => {@result}", result);

            return result;
        }
    }
}