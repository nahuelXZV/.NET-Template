using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;

namespace Domain.Interfaces.Services.Segurity;

public interface IModuloService
{
    Task<long> Create(ModuloDTO modulo);
    Task<bool> Update(ModuloDTO modulo);
    Task<bool> Delete(long id);
    Task<ModuloDTO> GetById(long id);
    Task<ResponseFilterDTO<ModuloDTO>> GetAll(FilterDTO? filter);
}
