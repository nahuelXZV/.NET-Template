using System.Reflection;
using WebClientMVC.Services;
using WebClientMVC.Services.Implementacion;
using WebClientMVC.Configs;
using WebClientMVC.Common.Middlewares;
using Domain.Constants;
using Domain.Interfaces.Services.Segurity;
using WebClientMVC.Services.Segurity;

namespace WebClientMVC.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, AdminConfig configs)
    {
        services.AddTransient<AppServicesAuthorizationHandler>();
        services.AddHttpClient(Constantes.HttpClientNames.ApiRest, client =>
        {
            client.BaseAddress = new Uri(configs.General.ApiUrl);
            client.Timeout = TimeSpan.FromSeconds(configs.General.ServiceTimeout);
        }).AddHttpMessageHandler<AppServicesAuthorizationHandler>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        #region Services
        services.AddScoped<IAppServices, AppServices>();
        
        services.AddScoped<ISesionService, SesionService>();
        services.AddScoped<IPerfilService, PerfilService>();
        services.AddScoped<IModuloService, ModuloService>();
        #endregion

        return services;
    }
}
