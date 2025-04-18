using AutoMapper;
using Domain.Common;
using Domain.DTOs.Segurity;
using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Segurity.Profile.Commands;

public class UpdateProfileCommand : ICommand<Response<bool>>
{
    public required PerfilDTO PerfilDTO { get; set; }
}

public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, Response<bool>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Perfil> _repository;
    private readonly IRepository<PerfilAcceso> _perfilAccesoRepository;

    public UpdateProfileCommandHandler(IMapper mapper, IRepository<Perfil> repository, IRepository<PerfilAcceso> perfilAccesoRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _perfilAccesoRepository = perfilAccesoRepository;
    }

    public async Task<Response<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var perfil = await _repository.GetByIdAsync(request.PerfilDTO.Id);
        if (perfil == null) throw new Exception("Perfil no encontrado.");

        _mapper.Map(request.PerfilDTO, perfil);

        var listaAccesos = await _perfilAccesoRepository.Query().Where(pa => pa.PerfilId == perfil.Id).ToListAsync();
        if (listaAccesos.Any()) _perfilAccesoRepository.DeleteRange(listaAccesos, false);

        _repository.Update(perfil, updateRelations: true);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}
