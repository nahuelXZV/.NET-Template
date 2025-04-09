using Domain.DTOs.Segurity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using WebClientMVC.Common;
using WebClientMVC.Configs;
using WebClientMVC.Extensions;

namespace WebClientMVC.Models;

public class MessageModel
{
    public MessageType Type { get; set; }
    public string Message { get; set; }
}

public class MainViewModel
{
    public long IdUsuarioLoggedIn { get; set; }
    public long IdPerfil { get; set; }
    public string NombreUsuarioLoggedIn { get; set; }
    public string ApellidoUsuarioLoggedIn { get; set; }
    public string NombreCompletoUsuarioLoggedIn { get; set; }
    public string FotoUsuarioLoggedIn { get; set; }
    public string CorreoLoggedIn { get; set; }
    public string InformacionSesion { get; set; }
    public AdminConfig Configuraciones { get; set; }
    public List<ModuloDTO> ModulosMenu { get; set; }
    public bool IncluirBlazorComponents { get; set; } = false;
    public List<MessageModel> Messages { get; set; }

    public MainViewModel()
    {
    }

    public MainViewModel(HttpContext context)
    {
        //Configuraciones = Startup.Configuraciones;
        CargarTempMessages(context);
        CargarAccesos(context);
        CargarDatosUsuarioLoggedIn(context.User);
    }

    private void CargarTempMessages(HttpContext context)
    {
        var factory = context?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
        var tempData = factory?.GetTempData(context);

        Messages = tempData.Get<List<MessageModel>>("Messages") ?? new List<MessageModel>();
    }

    private void CargarDatosUsuarioLoggedIn(ClaimsPrincipal userClaims)
    {
        IdUsuarioLoggedIn = long.Parse(userClaims.GetClaimValue(Constantes.ClaimTypes.UsuarioId));
        IdPerfil = long.Parse(userClaims.GetClaimValue(Constantes.ClaimTypes.PerfilId));
        NombreUsuarioLoggedIn = userClaims.GetClaimValue(Constantes.ClaimTypes.NombreUsuario);
        ApellidoUsuarioLoggedIn = userClaims.GetClaimValue(Constantes.ClaimTypes.ApellidoUsuario);
        NombreCompletoUsuarioLoggedIn = userClaims.GetClaimValue(Constantes.ClaimTypes.NombreCompleto);
        CorreoLoggedIn = userClaims.GetClaimValue(Constantes.ClaimTypes.Correo);
        FotoUsuarioLoggedIn = "";
    }

    private void CargarAccesos(HttpContext context)
    {
        List<PerfilAccesoDTO> listaAccesos = context.Session.Get<List<PerfilAccesoDTO>>(Constantes.ClaimTypes.ListaAccesos) ?? new List<PerfilAccesoDTO>();
        var accesos = listaAccesos.Select(x => x.Acceso).ToList();

        ModulosMenu = accesos
            .GroupBy(a => a.Modulo.Id)
            .Select(g => new ModuloDTO
            {
                Id = g.First().Modulo.Id,
                Nombre = g.First().Modulo.Nombre,
                Icono = g.First().Modulo.Icono,
                Secuencia = g.First().Modulo.Secuencia,
                ListaAccesos = g
                    .OrderBy(a => a.Secuencia)
                    .Select(a => new AccesoDTO
                    {
                        Id = a.Id,
                        Nombre = a.Nombre,
                        Secuencia = a.Secuencia,
                        Controlador = a.Controlador,
                        Vista = a.Vista,
                        Url = a.Url,
                        Icono = a.Icono,
                        Descripcion = a.Descripcion,
                        ModuloId = a.ModuloId
                    }).ToList()
            })
            .OrderBy(m => m.Secuencia)
            .ToList();
    }
}
