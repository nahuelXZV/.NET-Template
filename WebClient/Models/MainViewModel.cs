using Domain.DTOs.Segurity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using WebClient.Common;
using WebClient.Configs;
using WebClient.Extensions;

namespace WebClient.Models;

public interface IMainViewModel
{
    void Initialize(HttpContext context, AdminConfig adminConfig);
}

public class ItemMenu
{
    public MenuItemId Id { get; set; }
    public string Text { get; set; }
    public string IconCss { get; set; }
    public string Url { get; set; }
    public bool Separator { get; set; }
    public object Data { get; set; }
    //public List<ContextMenuItem> Items { get; set; } = new List<ContextMenuItem>();
    public MenuItemActionType ActionType { get; set; } = MenuItemActionType.GET;
    public bool Confirm { get; set; }
    public string ConfirmMessage { get; set; }
}

public class MessageModel
{
    public MessageType Type { get; set; }
    public string Message { get; set; }
}

public class MainViewModel : IMainViewModel
{
    public long IdUsuarioLoggedIn { get; set; }
    public long IdPerfil { get; set; }
    public string NombreUsuarioLoggedIn { get; set; }
    public string ApellidoUsuarioLoggedIn { get; set; }
    public string NombreCompletoUsuarioLoggedIn { get; set; }
    public string FotoUsuarioLoggedIn { get; set; }
    public string CorreoLoggedIn { get; set; }
    public AdminConfig Configuraciones { get; set; }
    public List<ModuloDTO> ModulosMenu { get; set; }
    public bool IncluirBlazorComponents { get; set; } = false;
    public List<MessageModel> Messages { get; set; }
    public List<ItemMenu> ListaItemsMenu { get; set; } = new();

    public MainViewModel() { }

    public MainViewModel(HttpContext context)
    {
        CargarTempMessages(context);
        CargarAccesos(context);
        CargarDatosUsuarioLoggedIn(context.User);
    }

    public virtual void Initialize(HttpContext context, AdminConfig adminConfig)
    {
        Configuraciones = adminConfig;
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
