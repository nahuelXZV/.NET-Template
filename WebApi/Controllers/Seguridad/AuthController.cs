using Application.Features.Seguridad.Usuario.Queries;
using Application.Features.Seguridad.Usuario.Command;
using Domain.DTOs.Seguridad.request;
using Domain.DTOs.Seguridad.Request;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Seguridad;
using Application.Wrappers;
using WebApi.Services;
using Domain.Config;

namespace WebApi.Controllers.Seguridad;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _configuracion;

    public AuthController(ILogger<WeatherForecastController> logger, IConfiguration configuracion)
    {
        _logger = logger;
        _configuracion = configuracion;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] RequestLoginDTO usuarioDTO)
    {
        try
        {
            UsuarioDTO usuario = (await Mediator.Send(new GetUsuarioByEmailQuery { Email = usuarioDTO.Email })).Data;
            if (usuario == null) return NotFound("Usuario no encontrado.");
            bool passwordValido = PasswordHasherService.VerifyPassword(usuario.Password, usuarioDTO.Password);
            if (!passwordValido) return Unauthorized("Usuario o Contraseña incorrecta.");

            JwtConfig jwtConfig = new JwtConfig
            {
                Key = _configuracion["Jwt:Key"] ?? "",
                Issuer = _configuracion["Jwt:Issuer"] ?? "",
                Audience = _configuracion["Jwt:Audience"] ?? "",
                ExpireTime = Convert.ToInt32(_configuracion["Jwt:ExpireTime"])
            };
            string token = JwtService.GenerateJwtToken(usuario, jwtConfig);

            return Ok(new Response<string>() { Data = token });
        }
        catch
        {
            return Unauthorized("Error");
        }
    }


    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RequestRegisterDTO usuarioDTO)
    {
        try
        {
            string passwordHashed = PasswordHasherService.HashPassword(usuarioDTO.Password);
            usuarioDTO.Password = passwordHashed;

            long id = (await Mediator.Send(new CrearUsuarioCommand { RequestRegisterDTO = usuarioDTO })).Data;
            return Ok(new Response<long>() { Data = id });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
