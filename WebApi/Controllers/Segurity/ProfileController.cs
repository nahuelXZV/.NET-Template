using Application.Features.Segurity.Profile.Commands;
using Application.Features.Segurity.Profile.Queries;
using Domain.DTOs.Segurity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

public class ProfileController : MainController
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetAllProfilesQuery()));
    }

    [HttpGet("{idProfile}")]
    public async Task<IActionResult> Get(long idProfile)
    {
        return Ok(await Mediator.Send(new GetProfileByIdQuery() { Id = idProfile }));
    }

    [HttpPost]
    public async Task<IActionResult> Post(PerfilDTO perfil)
    {
        return Ok(await Mediator.Send(new CreateProfileCommand { PerfilDTO = perfil }));
    }

    [HttpPut]
    public async Task<IActionResult> Put(PerfilDTO perfil)
    {
        return Ok(await Mediator.Send(new UpdateProfileCommand { PerfilDTO = perfil }));
    }

    [HttpDelete("Delete/{idProfile}")]
    public async Task<IActionResult> Delete(long idProfile)
    {
        return Ok(await Mediator.Send(new DeleteProfileCommand { Id = idProfile }));
    }
}
