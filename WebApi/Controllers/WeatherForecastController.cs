using Application.Features.Weather.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("clima")]
        public async Task<ActionResult> Get()
        {
            var response = await Mediator.Send(new GetWeatherQuery { });
            return Ok(response);
        }

        [HttpGet("auth")]
        public async Task<ActionResult> auth()
        {
            var response = await Mediator.Send(new GetWeatherQuery { });
            return Ok(response);
        }
    }
}
