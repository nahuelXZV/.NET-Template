using Domain.DTOs.Segurity.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Common;
using WebClient.Extensions;
using WebClient.Models;
using WebClient.Models.Segurity;
using WebClient.Services;

namespace WebClient.Controllers;

[AllowAnonymous]
public class SesionController : Controller
{
    private readonly IAppServices _appServices;

    public SesionController(IAppServices appServices)
    {
        _appServices = appServices;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        var model = new SesionViewModel(HttpContext);
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Iniciar(SesionViewModel model)
    {
        try
        {
            model.DireccionIp = Request.GetClientIpAddress();
            var usuarioDTO = await _appServices.SesionService.Login(new RequestLoginDTO
            {
                Username = model.Usuario,
                Password = model.Password
            });

            if (usuarioDTO == null) throw new Exception("Crendenciales no validas.");

            var claimsUsuario = usuarioDTO.GetClaims(model);
            await HttpContext.IniciarSesion(claimsUsuario, false);

            HttpContext.Session.Set(Constantes.ClaimTypes.ListaAccesos, usuarioDTO.Perfil.ListaAccesos);
            await HttpContext.IniciarSesion(claimsUsuario);

            if (!string.IsNullOrWhiteSpace(model.RedirectUrl))
            {
                return Redirect(model.RedirectUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            model.Password = null;
            model.Errores.Add(new ResponseError("Las credenciales ingresadas no son válidas. Por favor, verifique e intente nuevamente.", ex.Message, ex.StackTrace));
            model.SetTempData(HttpContext);

            return RedirectToAction("Index");
        }
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Cerrar()
    {
        await HttpContext.CerrarSesion();
        HttpContext.Session.Clear();
        return Redirect("~/");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> AccesoDenegado()
    {
        await Task.CompletedTask;
        return View();
    }
}
