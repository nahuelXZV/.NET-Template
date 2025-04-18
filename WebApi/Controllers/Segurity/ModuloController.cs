using Application.Features.Segurity.Module.Queries;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Segurity;

public class ModuloController : MainController
{
    private readonly ILogger<ModuloController> _logger;

    public ModuloController(ILogger<ModuloController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterDTO? filter)
    {
        return Ok(await Mediator.Send(new GetAllModulesQuery() { Filter = filter }));
    }

    //[HttpGet("{idProfile}")]
    //public async Task<IActionResult> Get(long idProfile)
    //{
    //    return Ok(await Mediator.Send(new GetProfileByIdQuery() { Id = idProfile }));
    //}

    //[HttpPost]
    //public async Task<IActionResult> Post(PerfilDTO perfil)
    //{
    //    return Ok(await Mediator.Send(new CreateProfileCommand { PerfilDTO = perfil }));
    //}

    //[HttpPut]
    //public async Task<IActionResult> Put(PerfilDTO perfil)
    //{
    //    return Ok(await Mediator.Send(new UpdateProfileCommand { PerfilDTO = perfil }));
    //}

    //[HttpDelete("Delete/{idProfile}")]
    //public async Task<IActionResult> Delete(long idProfile)
    //{
    //    return Ok(await Mediator.Send(new DeleteProfileCommand { Id = idProfile }));
    //}
}
