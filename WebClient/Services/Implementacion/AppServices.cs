using Domain.Interfaces.Services.Segurity;
using WebClient.Services;

namespace WebClient.Services.Implementacion;

public class AppServices : IAppServices
{
    private readonly ILogger<AppServices> _logger;

    private readonly IServiceProvider _serviceProvider;

    private ISesionService _sesionService;
    private IPerfilService _perfilService;
    private IModuloService _moduloService;

    public AppServices(IServiceProvider serviceProvider, ILogger<AppServices> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public ISesionService SesionService => _sesionService ??= _serviceProvider.GetService<ISesionService>();

    public IPerfilService PerfilService => _perfilService ??= _serviceProvider.GetService<IPerfilService>();

    public IModuloService ModuloService => _moduloService ??= _serviceProvider.GetService<IModuloService>();
}
