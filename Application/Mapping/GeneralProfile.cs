
using AutoMapper;
using Domain.DTOs.Seguridad.Request;
using Domain.Entities.Seguridad;

namespace Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Usuario, RequestRegisterDTO>();
        CreateMap<RequestRegisterDTO, Usuario>();
    }
}
