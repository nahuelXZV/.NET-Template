
using AutoMapper;
using Domain.DTOs.Segurity.Request;
using Domain.Entities.Segurity;

namespace Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Usuario, RequestRegisterDTO>();
        CreateMap<RequestRegisterDTO, Usuario>();
    }
}
