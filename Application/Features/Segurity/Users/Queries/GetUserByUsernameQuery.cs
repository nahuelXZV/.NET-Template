using Application.Features.Segurity.Profile.Queries;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.Interfaces.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Users.Queries;

public class GetUserByUsernameQuery : IRequest<Response<UsuarioDTO>>
{
    public string Username { get; set; }
}

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, Response<UsuarioDTO>>
{
    private readonly IDbContext _appCnx;
    private readonly IMediator _mediator;

    public GetUserByUsernameQueryHandler(IDbContext appDbContext, IMediator mediator)
    {
        _appCnx = appDbContext;
        _mediator = mediator;
    }
    public async Task<Response<UsuarioDTO>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var dbContext = _appCnx.dbContext;

        var usuarioRequest = await dbContext.Set<Domain.Entities.Segurity.Usuario>()
                                        .Where(u => u.Username == request.Username)
                                        .Select(u => new UsuarioDTO
                                        {
                                            Id = u.Id,
                                            Username = u.Username,
                                            Nombre = u.Nombre,
                                            Apellido = u.Apellido,
                                            Password = u.Password,
                                            Email = u.Email,
                                            PerfilId = u.PerfilId
                                        })
                                        .SingleOrDefaultAsync(cancellationToken);

        if (usuarioRequest == null) return new Response<UsuarioDTO>("Usuario no encontrado.");

        var perfil = (await _mediator.Send(new GetProfileByIdQuery { Id = usuarioRequest.PerfilId })).Data;
        if (perfil == null) throw new Exception("El usuario no tiene un perfil asignado.");

        usuarioRequest.Perfil = perfil;
        return new Response<UsuarioDTO>(usuarioRequest);
    }
}
