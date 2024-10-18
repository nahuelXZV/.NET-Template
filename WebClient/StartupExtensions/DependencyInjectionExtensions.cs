using Domain.Constants;
using Domain.Interfaces;
using WebClient.Services;

namespace WebClient.StartupExtensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddReportService(this IServiceCollection services)
    {
        services.AddHttpClient(Constantes.HttpClientNames.ReportServer, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(1000);
        });

        services.AddScoped<IClimaService, ClimaService>();

        return services;
    }
}
