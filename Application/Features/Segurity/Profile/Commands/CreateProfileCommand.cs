using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;

namespace Application.Features.Segurity.Profile.Commands;

public class CreateProfileCommand : ICommand<Response<long>>
{
    public required PerfilDTO PerfilDTO { get; set; }
}

public class CreateProfileCommandHandler : ICommandHandler<CreateProfileCommand, Response<long>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Perfil> _repository;

    public CreateProfileCommandHandler(IMapper mapper, IRepository<Perfil> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Response<long>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var perfil = _mapper.Map<Perfil>(request.PerfilDTO);

        perfil = await _repository.AddAsync(perfil);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<long>(perfil.Id);
    }
}