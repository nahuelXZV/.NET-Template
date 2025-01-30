using Application.Features.Segurity.Users.Commands;
using Domain.DTOs.Segurity.request;
using Domain.DTOs.Segurity.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public AuthController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] RequestLoginDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new IniciarSesionCommand { RequestLoginDTO = usuarioDTO }));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RequestRegisterDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new CrearUsuarioCommand { RequestRegisterDTO = usuarioDTO }));
    }
}
