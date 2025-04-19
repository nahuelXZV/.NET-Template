using Domain.DTOs.Segurity;
using Domain.DTOs.Segurity.request;
using Domain.Interfaces.Services.Segurity;
using WebClient.Services.Implementacion;

namespace WebClient.Services.Segurity;

public class SesionService : AppBaseServices, ISesionService
{
    public SesionService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, ILogger<SesionService> logger)
      : base("api/Auth", httpClientFactory, contextAccessor, logger)
    {
    }

    public async Task<UsuarioDTO> Login(RequestLoginDTO loginDTO)
    {
        var uri = $"login";
        return await PostAsync<UsuarioDTO>(uri, loginDTO);
    }

}
