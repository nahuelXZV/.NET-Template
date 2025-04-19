using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Interfaces.Services.Segurity;
using WebClient.Services.Implementacion;

namespace WebClient.Services.Segurity;

public class UsuarioService : AppBaseServices, IUsuarioService
{
    public UsuarioService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, ILogger<UsuarioService> logger)
        : base("api/Usuario", httpClientFactory, contextAccessor, logger)
    {
    }

    public async Task<long> Create(UsuarioDTO usuario)
    {
        var uri = $"";
        return await PostAsync<long>(uri, usuario);
    }

    public async Task<bool> Delete(long id)
    {
        var uri = $"Delete/{id}";
        return await DeleteAsync<bool>(uri);
    }

    public async Task<ResponseFilterDTO<UsuarioDTO>> GetAll(FilterDTO? filter)
    {
        var uri = AplicarFiltro(filter);
        return await GetAsync<ResponseFilterDTO<UsuarioDTO>>(uri);
    }

    public async Task<UsuarioDTO> GetById(long id)
    {
        var uri = $"{id}";
        return await GetAsync<UsuarioDTO>(uri);
    }

    public async Task<bool> Update(UsuarioDTO usuario)
    {
        var uri = $"";
        return await PutAsync<bool>(uri, usuario);
    }
}
