using ILogger = Serilog.ILogger;
using Serilog.Events;
using Serilog;

namespace WebClient.Extensions;

public static class SerilogLoggerExtensions
{
    //public const LogEventLevel SerilogLogEventLevel = LogEventLevel.Information;
    //public const string LoggerOutputTemplate = "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    //public static ILogger Create(IConfiguration configuration = null)
    //{
    //    var loggerConfiguration = new LoggerConfiguration().Enrich.FromLogContext();

    //    if (configuration != null)
    //    {
    //        loggerConfiguration.ReadFrom.Configuration(configuration);
    //    }
    //    else
    //    {
    //        loggerConfiguration.WriteTo.Console(SerilogLogEventLevel, LoggerOutputTemplate);
    //    }

    //    Log.Logger = loggerConfiguration.CreateBootstrapLogger();

    //    return Log.Logger;
    //}

    //public static void Destroy()
    //{
    //    Log.CloseAndFlush();
    //}

    //public static IHostBuilder UseCustomLogger(this IHostBuilder hostBuilder)
    //{
    //    Create();

    //    hostBuilder.ConfigureServices(services =>
    //    {
    //        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    //    });

    //    hostBuilder.ConfigureLogging((context, logging) =>
    //    {
    //        logging.ClearProviders();
    //        logging.AddSerilog();
    //    });

    //    hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
    //    {
    //        loggerConfiguration
    //            .ReadFrom.Configuration(context.Configuration)
    //            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    //            .Enrich.FromLogContext()
    //            .WriteTo.Console(outputTemplate: LoggerOutputTemplate);
    //    });

    //    return hostBuilder;
    //}


    //public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder application)
    //{
    //    application.UseSerilogRequestLogging();
    //    return application;
    //}
}
