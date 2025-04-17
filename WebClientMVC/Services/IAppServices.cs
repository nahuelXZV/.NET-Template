using Domain.Interfaces.Services.Segurity;

namespace WebClientMVC.Services;

public interface IAppServices
{
    public ISesionService SesionService { get; }
    public IPerfilService PerfilService { get; }
    public IModuloService ModuloService { get; }

}
