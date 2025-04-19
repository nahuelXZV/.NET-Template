using Application.Features.Segurity.Users.Queries;
using Microsoft.Extensions.Configuration;
using Domain.DTOs.Segurity.request;
using Domain.DTOs.Segurity;
using Application.Helpers;
using Domain.Config;
using MediatR;
using Domain.Interfaces.Shared;
using Domain.Common;

namespace Application.Features.Segurity.Auth.Commands;

public class LoginCommand : ICommand<Response<UsuarioDTO>>
{
    public required RequestLoginDTO RequestLoginDTO { get; set; }
}

public class IniciarSesionCommandHandler : ICommandHandler<LoginCommand, Response<UsuarioDTO>>
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuracion;

    public IniciarSesionCommandHandler(IMediator mediator, IConfiguration configuracion)
    {
        _mediator = mediator;
        _configuracion = configuracion;
    }

    public async Task<Response<UsuarioDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        UsuarioDTO usuario = (await _mediator.Send(new GetUserByUsernameQuery { Username = request.RequestLoginDTO.Username })).Data;
        if (usuario == null) throw new Exception("Usuario no encontrado.");

        bool passwordValido = PasswordHasherHelper.VerifyPassword(usuario.Username, usuario.Password, request.RequestLoginDTO.Password);
        if (!passwordValido) throw new Exception("Usuario o Contraseña incorrecta.");

        JwtConfig jwtConfig = new JwtConfig
        {
            Key = _configuracion["Jwt:Key"] ?? "",
            Issuer = _configuracion["Jwt:Issuer"] ?? "",
            Audience = _configuracion["Jwt:Audience"] ?? "",
            ExpireTime = Convert.ToInt32(_configuracion["Jwt:ExpireTime"])
        };
        usuario.Token = JwtHelper.GenerateJwtToken(usuario, jwtConfig);
        usuario.Password = string.Empty;
        return new Response<UsuarioDTO>(usuario);
    }
}
