using System.Net.Http.Headers;
using WebClient.Extensions;

namespace WebClient.Common.Middlewares;

public class AppServicesAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppServicesAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsync(request, cancellationToken).GetAwaiter().GetResult();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var jwtAuthorizationToken = httpContext.User.GetClaimValue(Constantes.ClaimTypes.Token);
            var idUsuario = httpContext.User.GetClaimValue(Constantes.ClaimTypes.UsuarioId);
            if (!string.IsNullOrWhiteSpace(jwtAuthorizationToken))
            {
                request.Headers.TryAddWithoutValidation(Constantes.ClaimTypes.UsuarioId, idUsuario);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtAuthorizationToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
