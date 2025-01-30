using Application.Features.Segurity.Users.Queries;
using Domain.DTOs.Segurity.Request;
using Domain.Entities.Segurity;
using Application.Wrappers;
using Application.Helpers;
using Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Segurity.Users.Commands;
public class CrearUsuarioCommand : ICommand<Response<bool>>
{
    public required RequestRegisterDTO RequestRegisterDTO { get; set; }
}

public class CrearUsuarioCommandHandler : ICommandHandler<CrearUsuarioCommand, Response<bool>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IRepository<Usuario> _repository;

    public CrearUsuarioCommandHandler(IMediator mediator, IMapper mapper, IRepository<Usuario> repository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existEmail = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.RequestRegisterDTO.Email })).Data;
            if (existEmail != null) throw new ArgumentException("El email ya se encuentra registrado.");

            string passwordHashed = PasswordHasherHelper.HashPassword(request.RequestRegisterDTO.Email, request.RequestRegisterDTO.Password);
            request.RequestRegisterDTO.Password = passwordHashed;
            Usuario usuario = _mapper.Map<Usuario>(request.RequestRegisterDTO);
            await _repository.AddAsync(usuario);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al crear el usuario: " + ex.Message);
        }
    }
}