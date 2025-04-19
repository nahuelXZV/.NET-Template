using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Users.Queries;

public class GetUserByIdQuery : ICommand<Response<UsuarioDTO>>
{
    public required long Id { get; set; }
}

public class GetUserByIdQueryHandler : ICommandHandler<GetUserByIdQuery, Response<UsuarioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Usuario> _repository;

    public GetUserByIdQueryHandler(IMapper mapper, IRepository<Usuario> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<UsuarioDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query()
            .Where(p => !p.Eliminado)
            .Where(p => p.Id == request.Id)
            .Include(p => p.Perfil);

        var usuario = await query.FirstOrDefaultAsync();
        if (usuario == null) throw new Exception("Usuario no encontrado.");

        var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
        return new Response<UsuarioDTO>(usuarioDTO);
    }
}
