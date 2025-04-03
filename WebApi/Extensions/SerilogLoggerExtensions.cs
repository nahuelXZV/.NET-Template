using Serilog;
using Serilog.Events;

namespace WebApi.Extensions;

public static class SerilogLoggerExtensions
{
    public const LogEventLevel SerilogLogEventLevel = LogEventLevel.Information;
    public const string LoggerOutputTemplate = "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    public static Serilog.ILogger Create(IConfiguration configuration = null)
    {
        var loggerConfiguration = new LoggerConfiguration().Enrich.FromLogContext();

        if (configuration != null)
        {
            loggerConfiguration.ReadFrom.Configuration(configuration);
        }
        else
        {
            loggerConfiguration
            .WriteTo.Console(SerilogLogEventLevel, LoggerOutputTemplate);
        }

        Log.Logger = loggerConfiguration.CreateBootstrapLogger();

        return Log.Logger;
    }

    public static void Destroy()
    {
        Log.CloseAndFlush();
    }

    public static IHostBuilder UseCustomLogger(this IHostBuilder hostBuilder)
    {
        Create();

        hostBuilder.ConfigureServices(services =>
        {
            services.AddLogging(services => services.AddSerilog(dispose: true));
        });


        hostBuilder.ConfigureLogging((context, logging) =>
        {
            logging.ClearProviders();
            logging.AddSerilog(dispose: true);
        });

        hostBuilder.UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        //.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
        .Enrich.FromLogContext());

        return hostBuilder;
    }

    public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder application)
    {
        application.UseSerilogRequestLogging();
        return application;
    }
}
