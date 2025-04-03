using Application.Wrappers;
using AutoMapper;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Profile.Queries;

public class GetAllProfilesQuery : ICommand<Response<List<PerfilDTO>>>
{
}

public class GetAllProfilesQueryHandler : ICommandHandler<GetAllProfilesQuery, Response<List<PerfilDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Perfil> _repository;

    public GetAllProfilesQueryHandler(IMapper mapper, IRepository<Perfil> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<List<PerfilDTO>>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query()
            .Where(p => !p.Eliminado)
            .Include(p => p.ListaAccesos)
            .ThenInclude(pa => pa.Acceso); 

        var perfiles = await query.ToListAsync(cancellationToken);

        var perfilesDTO = _mapper.Map<List<PerfilDTO>>(perfiles);

        return new Response<List<PerfilDTO>>(perfilesDTO);
    }
}
