using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;
using WebClient.Extensions;
using WebClient.Models;
using WebClient.Models.Segurity;
using WebClient.Services;

namespace WebClient.Controllers;

public class UsuarioController : MainController
{
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ViewModelFactory viewModelFactory, ILogger<UsuarioController> logger, IAppServices services)
        : base(viewModelFactory, services)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        var model = _viewModelFactory.Create<UsuarioViewModel>();
        model.IncluirBlazorComponents = true;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string search = "", [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        try
        {
            var ListaUsuarios = await _appServices.UsuarioService.GetAll(new FilterDTO()
            {
                Limit = limit,
                Offset = offset,
                Search = search
            });

            return Ok(ListaUsuarios);
        }
        catch
        {
            return Ok(new ResponseFilterDTO<UsuarioDTO>() { Total = 0, Data = new() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromForm] long id = 0)
    {
        var model = _viewModelFactory.Create<UsuarioViewModel>();

        model.IncluirBlazorComponents = true;
        model.ListaPerfiles = (await _appServices.PerfilService.GetAll(new FilterDTO())).Data;

        if (id != 0)
            model.Usuario = await _appServices.UsuarioService.GetById(id);
        else
            model.Usuario = new();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar([FromForm] long id)
    {
        try
        {
            await _appServices.UsuarioService.Delete(id);
            this.AddSuccessTempMessage("Usuario eliminado correctamente");
        }
        catch (Exception ex)
        {
            this.AddTempMessage(ex);
        }
        return RedirectToAction("Listado");
    }




}
