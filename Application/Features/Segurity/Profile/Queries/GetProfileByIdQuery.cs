using Application.Wrappers;
using AutoMapper;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Profile.Queries;

public class GetProfileByIdQuery : ICommand<Response<PerfilDTO>>
{
    public required long Id { get; set; }
}

public class GetProfileByIdQueryHandler : ICommandHandler<GetProfileByIdQuery, Response<PerfilDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Perfil> _repository;

    public GetProfileByIdQueryHandler(IMapper mapper, IRepository<Perfil> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<PerfilDTO>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query().Where(p => !p.Eliminado)
            .Where(p => p.Id == request.Id)
            .Include(p => p.ListaAccesos)
            .ThenInclude(la => la.Acceso);

        var perfil = await query.FirstOrDefaultAsync();
        if (perfil == null) throw new Exception("Perfil no encontrado.");

        var perfilDTO = _mapper.Map<PerfilDTO>(perfil);
        return new Response<PerfilDTO>(perfilDTO);
    }
}
