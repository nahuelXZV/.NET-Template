using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Entities.Segurity;
using Domain.Extensions;
using Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Profile.Queries;

public class GetAllProfilesQuery : ICommand<Response<ResponseFilterDTO<PerfilDTO>>>
{
    public FilterDTO? Filter { get; set; }
}

public class GetAllProfilesQueryHandler : ICommandHandler<GetAllProfilesQuery, Response<ResponseFilterDTO<PerfilDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Perfil> _repository;

    public GetAllProfilesQueryHandler(IMapper mapper, IRepository<Perfil> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<ResponseFilterDTO<PerfilDTO>>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _repository.Query().Where(p => !p.Eliminado);

        var total = await baseQuery.CountAsync(cancellationToken);

        var query = baseQuery
            .Include(p => p.ListaAccesos)
            .ThenInclude(pa => pa.Acceso)
            .ApplyFilter(
                request.Filter,
                p => string.IsNullOrEmpty(request.Filter.Search)
                     || p.Nombre.ToLower().Contains(request.Filter.Search.ToLower())
                     || p.Descripcion.ToLower().Contains(request.Filter.Search.ToLower())
            );

        var perfiles = await query.ToListAsync(cancellationToken);
        var perfilesDTO = _mapper.Map<List<PerfilDTO>>(perfiles);

        var response = new ResponseFilterDTO<PerfilDTO>
        {
            Data = perfilesDTO,
            Total = total
        };

        return new Response<ResponseFilterDTO<PerfilDTO>>(response);
    }
}
