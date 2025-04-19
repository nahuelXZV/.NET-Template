using Application.Features.Segurity.Users.Queries;
using Domain.Entities.Segurity;
using Application.Helpers;
using AutoMapper;
using MediatR;
using Domain.Interfaces.Shared;
using Domain.Common;
using Domain.DTOs.Segurity;

namespace Application.Features.Segurity.Users.Commands;
public class UpdateUserCommand : ICommand<Response<bool>>
{
    public UsuarioDTO UsuarioDTO { get; set; }
}

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Response<bool>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IRepository<Usuario> _repository;

    public UpdateUserCommandHandler(IMediator mediator, IMapper mapper, IRepository<Usuario> repository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _repository.GetByIdAsync(request.UsuarioDTO.Id);
        if (usuario == null) throw new ArgumentException("El usuario no existe.");

        var existEmail = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.UsuarioDTO.Email })).Data;
        if (existEmail != null && existEmail.Id != request.UsuarioDTO.Id)
            throw new ArgumentException("El email ya se encuentra registrado por otro usuario.");

        var existUsuario = (await _mediator.Send(new GetUserByUsernameQuery { Username = request.UsuarioDTO.Username })).Data;
        if (existUsuario != null && existUsuario.Id != request.UsuarioDTO.Id)
            throw new ArgumentException("El usuario ya se encuentra registrado por otro usuario.");

        _mapper.Map(request.UsuarioDTO, usuario);

        // Si la contraseña fue modificada, la re-hashamos
        if (!string.IsNullOrEmpty(request.UsuarioDTO.Password))
        {
            string passwordHashed = PasswordHasherHelper.HashPassword(request.UsuarioDTO.Username, request.UsuarioDTO.Password);
            usuario.Password = passwordHashed;
        }

        _repository.Update(usuario);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}
