using Domain.DTOs.Segurity;
using Domain.DTOs.Segurity.request;

namespace Domain.Interfaces.Services.Segurity;

public interface ISesionService
{
    Task<UsuarioDTO> Login(RequestLoginDTO model);
}
