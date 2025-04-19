using Domain.DTOs.Segurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;
using WebClient.Common;
using WebClient.Models.Segurity;

namespace WebClient.Extensions;

public static class ClaimExtensions
{
    public static async Task IniciarSesion(this HttpContext httpContext, List<Claim> claims, bool writeSession = true)
    {
        var user = httpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            user.UpdateClaims(claims);
        }
        else
        {
            user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }

        if (writeSession)
        {
            await httpContext.SignInAsync(user);
        }

        httpContext.User = user;
    }

    public static async Task CerrarSesion(this HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public static void UpdateClaims(this ClaimsPrincipal currentPrincipal, List<Claim> claims)
    {
        var identity = currentPrincipal.Identity as ClaimsIdentity;
        if (identity == null) throw new InvalidOperationException("No se puede actualizar los claims");

        var existingClaims = identity.FindAll(cidentity => claims.Select(c => c.Type).Contains(cidentity.Type)).ToList();
        if (existingClaims.Any()) existingClaims.ForEach(claim => identity.RemoveClaim(claim));

        identity.AddClaims(claims);
    }

    public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
    {
        try
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null) return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            return null;
        }
    }

    public static List<Claim> GetClaims(this UsuarioDTO usuario, SesionViewModel model)
    {
        var claims = new List<Claim> {
            new Claim(Constantes.ClaimTypes.UsuarioId, usuario.Id.ToString()),
            new Claim(Constantes.ClaimTypes.PerfilId, usuario.PerfilId.ToString()),
            new Claim(Constantes.ClaimTypes.NombreUsuario, usuario.Nombre),
            new Claim(Constantes.ClaimTypes.ApellidoUsuario, usuario.Apellido),
            new Claim(Constantes.ClaimTypes.NombreCompleto, usuario.Nombre + " " + usuario.Apellido),
            new Claim(Constantes.ClaimTypes.Correo, usuario.Email),
            new Claim(Constantes.ClaimTypes.IpSesion, model.DireccionIp),
            new Claim(Constantes.ClaimTypes.Token, usuario.Token??""),
            new Claim(Constantes.ClaimTypes.IdsAccesosPermitidos, string.Join(",", usuario.Perfil.ListaAccesos.Select(acceso => acceso.Acceso.Id.ToString()))),
            new Claim(Constantes.ClaimTypes.AccesosPermitidos, usuario.Perfil.ListaAccesos.Select(acceso => acceso.Acceso.Controlador).Distinct().Serialize()),
        };
        return claims;
    }

}
