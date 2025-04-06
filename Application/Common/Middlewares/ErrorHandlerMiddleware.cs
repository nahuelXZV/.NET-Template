using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Domain.Extensions;
using Domain.Exceptions;
using FluentValidation;
using AutoMapper;
using System.Net;
using Domain.Common;

namespace Application.Common.Middlewares;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;

        var errorMessage = $"{exception.Message}";
        if (exception.InnerException != null)
        {
            errorMessage += $"{Environment.NewLine}{exception.InnerException.Message}";
        }

        var clientErrorMessage = "Ocurrió un error inesperado";
        var stackTrace = exception.StackTrace ?? "Sin StackTrace para el error";

        var errorDetails = new Dictionary<string, MessageError>();

        switch (exception)
        {
            case ServiceException ex:
                clientErrorMessage = ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case ResourceNotFoundException ex:
                clientErrorMessage = ex.Message;
                errorMessage = $"{nameof(ResourceNotFoundException)}: {ex.Message} - Fuente: [{ex.Source}]";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case DomainException ex:
                clientErrorMessage = exception.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case TimeoutException ex:
                clientErrorMessage = "El servidor no respondió a tiempo";
                response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                break;
            case SqlException ex:
                errorMessage = $"{nameof(SqlException)}: {ex.Message} - Fuente: [{ex.Source}]";
                clientErrorMessage = "Ocurrió un error al realizar una operación en la base de datos";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case AutoMapperMappingException ex:
                errorMessage = $"{nameof(AutoMapperMappingException)}: {ex.Message} - Fuente: [{ex.Source}]";
                break;
            case ValidationException ex:
                clientErrorMessage = "Ocurrieron errores de validación";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = $"{ex.Message}";

                ex.Errors.ToList().ForEach(error =>
                {
                    var errorKey = error.PropertyName;
                    var errorValue = error.ErrorMessage;
                    errorDetails.Add(errorKey, new MessageError(errorValue));
                });
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var messageResponse = new Response<string>()
        {
            Succeded = false,
            Errors = new MessageError(clientErrorMessage, errorMessage, stackTrace, errorDetails)
        };
        var errorModel = messageResponse.Serialize();

        response.ContentType = "application/json";
        await response.WriteAsync(errorModel);
    }
}
