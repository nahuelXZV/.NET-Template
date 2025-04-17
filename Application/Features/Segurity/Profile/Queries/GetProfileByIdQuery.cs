using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;
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
    private readonly IRepository<PerfilAcceso> _repositoryAcceso;

    public GetProfileByIdQueryHandler(IMapper mapper, IRepository<Perfil> repository, IRepository<PerfilAcceso> repositoryAcceso)
    {
        _mapper = mapper;
        _repository = repository;
        _repositoryAcceso = repositoryAcceso;
    }

    public async Task<Response<PerfilDTO>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query().Where(p => !p.Eliminado)
            .Where(p => p.Id == request.Id);

        var perfil = await query.FirstOrDefaultAsync();
        if (perfil == null) throw new Exception("Perfil no encontrado.");

        var queryAcceso = _repositoryAcceso.Query()
                            .Where(a => a.PerfilId == perfil.Id)
                            .Where(a => !a.Eliminado)
                            .Include(la => la.Acceso)
                            .ThenInclude(ac => ac.Modulo);
        var listaAccesos = await queryAcceso.ToListAsync();

        perfil.ListaAccesos = listaAccesos.Where(a => !a.Acceso.Eliminado).ToList();

        var perfilDTO = _mapper.Map<PerfilDTO>(perfil);
        return new Response<PerfilDTO>(perfilDTO);
    }
}
