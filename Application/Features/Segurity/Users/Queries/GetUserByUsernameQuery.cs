using Application.Wrappers;
using Domain.DTOs.Segurity;
using Domain.Interfaces;
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

    public GetUserByUsernameQueryHandler(IDbContext appDbContext)
    {
        _appCnx = appDbContext;
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
                                        })
                                        .SingleOrDefaultAsync(cancellationToken);

        if (usuarioRequest == null) return new Response<UsuarioDTO>("Usuario no encontrado.");

        return new Response<UsuarioDTO>(usuarioRequest);
    }
}
