using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Persistence;
using Domain.Interfaces;

namespace Infraestructure;

public static class DependencyInjection
{
    public static void AddInfrastructureBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext"));
        });
        services.AddScoped<IDbContext, AppDbContext>();
    }
}
