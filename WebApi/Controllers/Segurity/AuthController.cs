using Application.Features.Segurity.Auth.Commands;
using Application.Features.Segurity.Users.Commands;
using Domain.DTOs.Segurity;
using Domain.DTOs.Segurity.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

public class AuthController : MainController
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] RequestLoginDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new LoginCommand { RequestLoginDTO = usuarioDTO }));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Create(UsuarioDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new CreateUserCommand { UsuarioDTO = usuarioDTO }));
    }
}
