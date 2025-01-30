using Domain.DTOs.Segurity.Request;
using Domain.Entities.Segurity;
using AutoMapper;

namespace Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Usuario, RequestRegisterDTO>();
        CreateMap<RequestRegisterDTO, Usuario>();
    }
}
