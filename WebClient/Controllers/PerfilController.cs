using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;
using WebClient.Extensions;
using WebClient.Models;
using WebClient.Models.Segurity;
using WebClient.Services;

namespace WebClient.Controllers;

public class PerfilController : MainController
{
    private readonly ILogger<PerfilController> _logger;

    public PerfilController(ViewModelFactory viewModelFactory, ILogger<PerfilController> logger, IAppServices appServices)
        : base(viewModelFactory, appServices)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        var model = _viewModelFactory.Create<PerfilViewModel>();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string search = "", [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        try
        {
            var ListaPerfiles = await _appServices.PerfilService.GetAll(new FilterDTO()
            {
                Limit = limit,
                Offset = offset,
                Search = search
            });

            return Ok(ListaPerfiles);
        }
        catch
        {
            return Ok(new ResponseFilterDTO<PerfilDTO>() { Total = 0, Data = new() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromForm] long id = 0)
    {
        var model = _viewModelFactory.Create<PerfilViewModel>();

        model.IncluirBlazorComponents = true;
        model.ListaModulos = (await _appServices.ModuloService.GetAll(new FilterDTO())).Data;

        if (id != 0)
            model.Perfil = await _appServices.PerfilService.GetById(id);
        else
            model.Perfil = new();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar([FromForm] long id)
    {
        try
        {
            await _appServices.PerfilService.Delete(id);
            this.AddSuccessTempMessage("Perfil eliminado correctamente");
        }
        catch (Exception ex)
        {
            this.AddTempMessage(ex);
        }
        return RedirectToAction("Listado");
    }
}
