using Microsoft.EntityFrameworkCore;
using Domain.DTOs.Seguridad;
using Application.Wrappers;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Seguridad.Usuario.Queries;

public class GetUsuarioByLoginQuery : IRequest<Response<UsuarioDTO>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class GetUsuarioByLoginQueryHandler : IRequestHandler<GetUsuarioByLoginQuery, Response<UsuarioDTO>>
{
    private readonly IDbContext _appCnx;
    private readonly IMediator _mediator;

    public GetUsuarioByLoginQueryHandler(IDbContext appDbContext, IMediator mediator)
    {
        _appCnx = appDbContext;
        _mediator = mediator;
    }
    public async Task<Response<UsuarioDTO>> Handle(GetUsuarioByLoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var dbContext = _appCnx.dbContext;
            string email = request.Email;
            string password = request.Password;

            var usuarioRequest = await dbContext.Set<Domain.Entities.Seguridad.Usuario>()
                                        .Where(u => u.Email == email && u.Password == password)
                                        .Select(u => new UsuarioDTO
                                        {
                                            Id = u.Id,
                                            Nombre = u.Nombre,
                                            Apellido = u.Apellido,
                                            Email = u.Email,
                                            Rol = u.Rol
                                        })
                                        .SingleOrDefaultAsync(cancellationToken);

            if (usuarioRequest == null) return new Response<UsuarioDTO>("Usuario no encontrado.");
            return new Response<UsuarioDTO>(usuarioRequest);
        }
        catch (Exception ex)
        {
            // Captura y retorno del error para mayor visibilidad
            return new Response<UsuarioDTO>($"Error al obtener el usuario: {ex.Message}");
        }
    }
}