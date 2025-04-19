using Application.Features.Segurity.Users.Queries;
using Domain.Entities.Segurity;
using Application.Helpers;
using AutoMapper;
using MediatR;
using Domain.Interfaces.Shared;
using Domain.Common;
using Domain.DTOs.Segurity;

namespace Application.Features.Segurity.Users.Commands;
public class CreateUserCommand : ICommand<Response<long>>
{
    public UsuarioDTO UsuarioDTO { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Response<long>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IRepository<Usuario> _repository;

    public CreateUserCommandHandler(IMediator mediator, IMapper mapper, IRepository<Usuario> repository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existEmail = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.UsuarioDTO.Email })).Data;
        if (existEmail != null) throw new ArgumentException("El email ya se encuentra registrado.");

        var existUsuario = (await _mediator.Send(new GetUserByUsernameQuery { Username = request.UsuarioDTO.Username })).Data;
        if (existUsuario != null) throw new ArgumentException("El usuario ya se encuentra registrado.");

        string passwordHashed = PasswordHasherHelper.HashPassword(request.UsuarioDTO.Username, request.UsuarioDTO.Password);
        request.UsuarioDTO.Password = passwordHashed;
        Usuario usuario = _mapper.Map<Usuario>(request.UsuarioDTO);
        usuario.FechaCreacion = DateTime.Now;
        usuario = await _repository.AddAsync(usuario);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<long>(usuario.Id);
    }
}