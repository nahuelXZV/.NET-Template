using Domain.Interfaces.Services.Segurity;

namespace WebClient.Services;

public interface IAppServices
{
    public ISesionService SesionService { get; }
    public IPerfilService PerfilService { get; }
    public IModuloService ModuloService { get; }
    public IUsuarioService UsuarioService { get; }

}
