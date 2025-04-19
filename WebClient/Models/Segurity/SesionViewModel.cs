using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
using WebClient.Configs;
using WebClient.Extensions;
using WebClient.Extensions;

namespace WebClient.Models.Segurity;

public class SesionViewModel
{

    [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar su nombre de usuario")]
    public string Usuario { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar su contraseña")]
    public string Password { get; set; }

    public string DireccionIp { get; set; }

    public AdminConfig Configuraciones { get; set; }

    public List<ResponseError> Errores { get; set; } = new List<ResponseError>();
    public string RedirectUrl { get; set; }

    public SesionViewModel()
    {
        //Configuraciones = Startup.Configuraciones;
    }

    public SesionViewModel(HttpContext httpContext) : this()
    {
        var factory = httpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
        var tempData = factory?.GetTempData(httpContext);

        Usuario = tempData?.Get<string>(nameof(Usuario));
        Errores = tempData?.Get<List<ResponseError>>(nameof(Errores)) ?? new List<ResponseError>();

        RedirectUrl = httpContext?.Request?.Query["ReturnUrl"];
    }

    public void SetTempData(HttpContext httpContext)
    {
        var factory = httpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
        var tempData = factory?.GetTempData(httpContext);
        tempData?.Set(nameof(Usuario), Usuario);
        tempData?.Set(nameof(Errores), Errores);
    }
}
