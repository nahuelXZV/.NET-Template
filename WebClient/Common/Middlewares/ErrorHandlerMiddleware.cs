using System.Net;

namespace WebClient.Common.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.Redirect("/");
        }
        catch (Exception ex)
        {
            HandleExceptionAsync(context, ex);
            throw;
        }
    }

    private void HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;

        var errorMessage = $"{exception.Message}";
        if (exception.InnerException != null)
        {
            errorMessage += $"{Environment.NewLine}{exception.InnerException.Message}";
        }

        switch (exception)
        {

            case Exception ex:
                errorMessage = ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        _logger.LogError(exception, errorMessage);
    }
}
