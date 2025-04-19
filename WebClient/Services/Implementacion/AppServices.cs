using Domain.Interfaces.Services.Segurity;

namespace WebClient.Services.Implementacion;

public class AppServices : IAppServices
{
    private readonly ILogger<AppServices> _logger;

    private readonly IServiceProvider _serviceProvider;

    private ISesionService _sesionService;
    private IPerfilService _perfilService;
    private IModuloService _moduloService;
    private IUsuarioService _usuarioService;

    public AppServices(IServiceProvider serviceProvider, ILogger<AppServices> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    #region SEGURITY
    public ISesionService SesionService => _sesionService ??= _serviceProvider.GetService<ISesionService>();
    public IPerfilService PerfilService => _perfilService ??= _serviceProvider.GetService<IPerfilService>();
    public IModuloService ModuloService => _moduloService ??= _serviceProvider.GetService<IModuloService>();
    public IUsuarioService UsuarioService => _usuarioService ??= _serviceProvider.GetService<IUsuarioService>();
    #endregion

    #region CONFIGURACION

    #endregion
}