using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Entities.Segurity;
using Domain.Extensions;
using Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Module.Queries;

public class GetAllModulesQuery : ICommand<Response<ResponseFilterDTO<ModuloDTO>>>
{
    public FilterDTO? Filter { get; set; }
}

public class GetAllModulesQueryHandler : ICommandHandler<GetAllModulesQuery, Response<ResponseFilterDTO<ModuloDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Modulo> _repository;
    private readonly IRepository<Acceso> _repositoryAcceso;

    public GetAllModulesQueryHandler(IMapper mapper, IRepository<Modulo> repository, IRepository<Acceso> repositoryAcceso)
    {
        _mapper = mapper;
        _repository = repository;
        _repositoryAcceso = repositoryAcceso;
    }

    public async Task<Response<ResponseFilterDTO<ModuloDTO>>> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _repository.Query().Where(p => !p.Eliminado);

        var total = await baseQuery.CountAsync(cancellationToken);

        var query = baseQuery.ApplyFilter(
                request.Filter,
                p => string.IsNullOrEmpty(request.Filter.Search) || p.Nombre.ToLower().Contains(request.Filter.Search.ToLower())
            );

        var modulos = await query.ToListAsync(cancellationToken);
        var modulosDTO = _mapper.Map<List<ModuloDTO>>(modulos);

        var ListaAccesos = await _repositoryAcceso.Query()
            .Where(p => !p.Eliminado)
            .ToListAsync();

        foreach (var modulo in modulosDTO)
        {
            modulo.ListaAccesos = ListaAccesos.Where(p => p.ModuloId == modulo.Id)
                .Select(p => new AccesoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    ModuloId = p.ModuloId
                })
                .ToList();
        }

        var response = new ResponseFilterDTO<ModuloDTO>
        {
            Data = modulosDTO,
            Total = total
        };

        return new Response<ResponseFilterDTO<ModuloDTO>>(response);
    }
}
