using Microsoft.EntityFrameworkCore;
using Domain.DTOs.Segurity;
using Application.Wrappers;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Segurity.Users.Queries;

public class GetUsuarioByEmailQuery : IRequest<Response<UsuarioDTO>>
{
    public string Email { get; set; }
}

public class GetUsuarioByEmailQueryHandler : IRequestHandler<GetUsuarioByEmailQuery, Response<UsuarioDTO>>
{
    private readonly IDbContext _appCnx;
    private readonly IMediator _mediator;

    public GetUsuarioByEmailQueryHandler(IDbContext appDbContext, IMediator mediator)
    {
        _appCnx = appDbContext;
        _mediator = mediator;
    }
    public async Task<Response<UsuarioDTO>> Handle(GetUsuarioByEmailQuery request, CancellationToken cancellationToken)
    {
        var dbContext = _appCnx.dbContext;
        string email = request.Email;

        var usuarioRequest = await dbContext.Set<Domain.Entities.Segurity.Usuario>()
                                        .Where(u => u.Email == email)
                                        .Select(u => new UsuarioDTO
                                        {
                                            Id = u.Id,
                                            Nombre = u.Nombre,
                                            Apellido = u.Apellido,
                                            Password = u.Password,
                                            Email = u.Email,
                                            Rol = u.Rol
                                        })
                                        .SingleOrDefaultAsync(cancellationToken);

        if (usuarioRequest == null) return new Response<UsuarioDTO>("Usuario no encontrado.");

        return new Response<UsuarioDTO>(usuarioRequest);
    }
}