namespace WebClient.Models;

using Microsoft.Extensions.Options;
using WebClient.Configs;

public class ViewModelFactory
{
    private readonly AdminConfig _adminConfig;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ViewModelFactory(IOptions<AdminConfig> adminConfigOptions, IHttpContextAccessor httpContextAccessor)
    {
        _adminConfig = adminConfigOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public T Create<T>() where T : IMainViewModel, new()
    {
        var instance = new T();
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            throw new InvalidOperationException("HttpContext no está disponible.");

        instance.Initialize(context, _adminConfig);
        return instance;
    }
}
