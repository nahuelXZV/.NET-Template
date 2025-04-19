using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Interfaces.Services.Segurity;
using WebClient.Services.Implementacion;

namespace WebClient.Services.Segurity;

public class PerfilService : AppBaseServices, IPerfilService
{
    public PerfilService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, ILogger<PerfilService> logger)
        : base("api/Perfil", httpClientFactory, contextAccessor, logger)
    {
    }

    public async Task<long> Create(PerfilDTO perfil)
    {
        var uri = $"";
        return await PostAsync<long>(uri, perfil);
    }

    public async Task<bool> Delete(long id)
    {
        var uri = $"Delete/{id}";
        return await DeleteAsync<bool>(uri);
    }

    public async Task<ResponseFilterDTO<PerfilDTO>> GetAll(FilterDTO? filter)
    {
        var uri = AplicarFiltro(filter);
        return await GetAsync<ResponseFilterDTO<PerfilDTO>>(uri);
    }

    public async Task<PerfilDTO> GetById(long id)
    {
        var uri = $"{id}";
        return await GetAsync<PerfilDTO>(uri);
    }

    public async Task<bool> Update(PerfilDTO perfil)
    {
        var uri = $"";
        return await PutAsync<bool>(uri, perfil);
    }
}
