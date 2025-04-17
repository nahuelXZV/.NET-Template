using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;

namespace Domain.Interfaces.Services.Segurity;

public interface IPerfilService
{
    Task<long> Create(PerfilDTO perfil);
    Task<bool> Update(PerfilDTO perfil);
    Task<bool> Delete(long id);
    Task<PerfilDTO> GetById(long id);
    Task<ResponseFilterDTO<PerfilDTO>> GetAll(FilterDTO? filter);
}
