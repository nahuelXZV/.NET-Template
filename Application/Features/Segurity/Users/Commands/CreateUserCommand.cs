using Application.Features.Segurity.Users.Queries;
using Domain.DTOs.Segurity.Request;
using Domain.Entities.Segurity;
using Application.Wrappers;
using Application.Helpers;
using Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Segurity.Users.Commands;
public class CreateUserCommand : ICommand<Response<bool>>
{
    public RequestRegisterDTO RequestRegisterDTO { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Response<bool>>
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

    public async Task<Response<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existEmail = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.RequestRegisterDTO.Email })).Data;
        if (existEmail != null) throw new ArgumentException("El email ya se encuentra registrado.");

        var existUsuario = (await _mediator.Send(new GetUserByUsernameQuery { Username = request.RequestRegisterDTO.Username })).Data;
        if (existUsuario != null) throw new ArgumentException("El usuario ya se encuentra registrado.");

        string passwordHashed = PasswordHasherHelper.HashPassword(request.RequestRegisterDTO.Username, request.RequestRegisterDTO.Password);
        request.RequestRegisterDTO.Password = passwordHashed;
        Usuario usuario = _mapper.Map<Usuario>(request.RequestRegisterDTO);
        usuario.FechaCreacion = DateTime.Now;
        await _repository.AddAsync(usuario);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}