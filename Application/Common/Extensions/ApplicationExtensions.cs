using Application.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Application.Common.Extensions;

public static class ApplicationExtensions
{
    public static void UseApplicationMiddlewares(this IApplicationBuilder app)
    {
        // el orden de los middlewares es importante
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
