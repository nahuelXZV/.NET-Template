using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.DTOs.Shared;
using Domain.Entities.Segurity;
using Domain.Extensions;
using Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Users.Queries;

public class GetAllUsersQuery : ICommand<Response<ResponseFilterDTO<UsuarioDTO>>>
{
    public FilterDTO? Filter { get; set; }
}

public class GetAllUsersQueryHandler : ICommandHandler<GetAllUsersQuery, Response<ResponseFilterDTO<UsuarioDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Usuario> _repository;

    public GetAllUsersQueryHandler(IMapper mapper, IRepository<Usuario> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<ResponseFilterDTO<UsuarioDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _repository.Query().Where(p => !p.Eliminado);

        var total = await baseQuery.CountAsync(cancellationToken);

        var query = baseQuery.ApplyFilter(
                request.Filter,
                p => string.IsNullOrEmpty(request.Filter.Search)
                     || p.Nombre.ToLower().Contains(request.Filter.Search.ToLower())
                     || p.Apellido.ToLower().Contains(request.Filter.Search.ToLower())
            );

        var listaUsuarios = await query.ToListAsync(cancellationToken);
        var listaUsuariosDTOs = _mapper.Map<List<UsuarioDTO>>(listaUsuarios);

        var response = new ResponseFilterDTO<UsuarioDTO>
        {
            Data = listaUsuariosDTOs,
            Total = total
        };

        return new Response<ResponseFilterDTO<UsuarioDTO>>(response);
    }
}
