using Application.Features.Segurity.Users.Commands;
using Application.Features.Segurity.Users.Queries;
using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

public class UsuarioController : MainController
{
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterDTO? filter)
    {
        return Ok(await Mediator.Send(new GetAllUsersQuery() { Filter = filter }));
    }

    [HttpGet("{idUser}")]
    public async Task<IActionResult> GetById(long idUser)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery() { Id = idUser }));
    }

    [HttpPost]
    public async Task<IActionResult> Create(UsuarioDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new CreateUserCommand { UsuarioDTO = usuarioDTO }));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UsuarioDTO usuarioDTO)
    {
        return Ok(await Mediator.Send(new UpdateUserCommand { UsuarioDTO = usuarioDTO }));
    }

    [HttpDelete("Delete/{idUser}")]
    public async Task<IActionResult> Delete(long idUser)
    {
        return Ok(await Mediator.Send(new DeleteUserCommand { Id = idUser }));
    }
}
