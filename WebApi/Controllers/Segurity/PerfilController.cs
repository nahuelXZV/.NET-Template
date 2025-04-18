using Application.Features.Segurity.Profile.Commands;
using Application.Features.Segurity.Profile.Queries;
using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

public class PerfilController : MainController
{
    private readonly ILogger<PerfilController> _logger;

    public PerfilController(ILogger<PerfilController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterDTO? filter)
    {
        return Ok(await Mediator.Send(new GetAllProfilesQuery() { Filter = filter }));
    }

    [HttpGet("{idProfile}")]
    public async Task<IActionResult> GetById(long idProfile)
    {
        return Ok(await Mediator.Send(new GetProfileByIdQuery() { Id = idProfile }));
    }

    [HttpPost]
    public async Task<IActionResult> Create(PerfilDTO perfil)
    {
        return Ok(await Mediator.Send(new CreateProfileCommand { PerfilDTO = perfil }));
    }

    [HttpPut]
    public async Task<IActionResult> Update(PerfilDTO perfil)
    {
        return Ok(await Mediator.Send(new UpdateProfileCommand { PerfilDTO = perfil }));
    }

    [HttpDelete("Delete/{idProfile}")]
    public async Task<IActionResult> Delete(long idProfile)
    {
        return Ok(await Mediator.Send(new DeleteProfileCommand { Id = idProfile }));
    }
}
