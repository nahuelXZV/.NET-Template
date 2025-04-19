using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Interfaces.Services.Segurity;
using WebClient.Services.Implementacion;

namespace WebClient.Services.Segurity;

public class ModuloService : AppBaseServices, IModuloService
{
    public ModuloService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, ILogger<ModuloService> logger)
        : base("api/Modulo", httpClientFactory, contextAccessor, logger)
    {
    }

    public async Task<long> Create(ModuloDTO modulo)
    {
        var uri = $"Create";
        return await PostAsync<long>(uri, modulo);
    }

    public async Task<bool> Delete(long id)
    {
        var uri = $"Delete/{id}";
        return await DeleteAsync<bool>(uri);
    }

    public async Task<ResponseFilterDTO<ModuloDTO>> GetAll(FilterDTO? filter = null)
    {
        var uri = AplicarFiltro(filter);
        return await GetAsync<ResponseFilterDTO<ModuloDTO>>(uri);
    }

    public async Task<ModuloDTO> GetById(long id)
    {
        var uri = $"GetById/{id}";
        return await GetAsync<ModuloDTO>(uri);
    }

    public async Task<bool> Update(ModuloDTO modulo)
    {
        var uri = $"Update";
        return await PutAsync<bool>(uri, modulo);
    }
}
