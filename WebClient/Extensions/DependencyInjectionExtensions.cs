using System.Reflection;
using Domain.Constants;
using Domain.Interfaces.Services.Segurity;
using FluentValidation;
using WebClient.Services;
using WebClient.Configs;
using WebClient.Services.Segurity;
using WebClient.Common.Middlewares;
using WebClient.Services.Implementacion;

namespace WebClient.Extensions;

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

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        #region Services
        services.AddScoped<IAppServices, AppServices>();

        services.AddScoped<ISesionService, SesionService>();
        services.AddScoped<IPerfilService, PerfilService>();
        services.AddScoped<IModuloService, ModuloService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        #endregion

        return services;
    }
}
