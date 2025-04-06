using Domain.Interfaces.Services;

namespace WebClientMVC.Services.Implementacion;

public class AppServices : IAppServices
{
    private readonly ILogger<AppServices> _logger;

    private readonly IServiceProvider _serviceProvider;

    private ISesionService _sesionService;

    public AppServices(IServiceProvider serviceProvider, ILogger<AppServices> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public ISesionService SesionService => _sesionService ??= _serviceProvider.GetService<ISesionService>();

}
