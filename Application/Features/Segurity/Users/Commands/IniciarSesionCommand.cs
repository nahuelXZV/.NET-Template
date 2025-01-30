using Application.Features.Segurity.Users.Queries;
using Microsoft.Extensions.Configuration;
using Domain.DTOs.Segurity.request;
using Domain.DTOs.Segurity;
using Application.Wrappers;
using Application.Helpers;
using Domain.Interfaces;
using Domain.Config;
using MediatR;

namespace Application.Features.Segurity.Users.Commands;

public class IniciarSesionCommand : ICommand<Response<UsuarioDTO>>
{
    public required RequestLoginDTO RequestLoginDTO { get; set; }
}

public class IniciarSesionCommandHandler : ICommandHandler<IniciarSesionCommand, Response<UsuarioDTO>>
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuracion;

    public IniciarSesionCommandHandler(IMediator mediator, IConfiguration configuracion)
    {
        _mediator = mediator;
        _configuracion = configuracion;
    }

    public async Task<Response<UsuarioDTO>> Handle(IniciarSesionCommand request, CancellationToken cancellationToken)
    {
        UsuarioDTO usuario = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.RequestLoginDTO.Email })).Data;
        if (usuario == null) throw new Exception("Usuario no encontrado.");

        bool passwordValido = PasswordHasherHelper.VerifyPassword(usuario.Email, usuario.Password, request.RequestLoginDTO.Password);
        if (!passwordValido) throw new Exception("Usuario o Contraseña incorrecta.");

        JwtConfig jwtConfig = new JwtConfig
        {
            Key = _configuracion["Jwt:Key"] ?? "",
            Issuer = _configuracion["Jwt:Issuer"] ?? "",
            Audience = _configuracion["Jwt:Audience"] ?? "",
            ExpireTime = Convert.ToInt32(_configuracion["Jwt:ExpireTime"])
        };
        usuario.Token = JwtHelper.GenerateJwtToken(usuario, jwtConfig);
        return new Response<UsuarioDTO>(usuario);
    }
}
