﻿using Application.Features.Segurity.Users.Queries;
using Domain.DTOs.Segurity.Request;
using Domain.Common.Interfaces;
using Application.Wrappers;
using Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Segurity.Users.Command;
public class CrearUsuarioCommand : ICommand<Response<long>>
{
    public RequestRegisterDTO RequestRegisterDTO { get; set; }
}

public class CrearUsuarioCommandHandler : ICommandHandler<CrearUsuarioCommand, Response<long>>
{
    private readonly IDbContext _appCnx;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CrearUsuarioCommandHandler(IDbContext appDbContext, IMediator mediator, IMapper mapper)
    {
        _appCnx = appDbContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Response<long>> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dbContext = _appCnx.dbContext;

            var existEmail = (await _mediator.Send(new GetUsuarioByEmailQuery { Email = request.RequestRegisterDTO.Email })).Data;
            if (existEmail != null) throw new ArgumentException("El email ya se encuentra registrado.");

            Domain.Entities.Segurity.Usuario usuario = _mapper.Map<Domain.Entities.Segurity.Usuario>(request.RequestRegisterDTO);
            await dbContext.Set<Domain.Entities.Segurity.Usuario>().AddAsync(usuario, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response<long>(usuario.Id);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al crear el usuario: " + ex.Message);
        }
    }
}