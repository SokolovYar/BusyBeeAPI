using BusyBee.API.DTOs;
using BusyBee.API.DTOs.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusyBeeBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<ApiResponse<IEnumerable<WeatherForecast>>> Get()
        {
            return Ok(new ApiResponse<IEnumerable<WeatherForecast>>
            {
                Status = new StatusInfo
                {
                    Code = 200,
                    Message = "Success",
                    IsSuccess = true
                },
                Meta = new MetaInfo(HttpContext),
                Data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray()
            });
        }
    }
}
