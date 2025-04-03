using Domain.DTOs.Segurity.Request;
using Domain.Entities.Segurity;
using AutoMapper;
using Domain.DTOs.Segurity;

namespace Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region Entity To DTO
        CreateMap<Usuario, RequestRegisterDTO>();
        CreateMap<Perfil, PerfilDTO>();
        #endregion

        #region  DTO To Entity
        CreateMap<RequestRegisterDTO, Usuario>();
        CreateMap<PerfilDTO, Perfil>();
        #endregion

    }
}
