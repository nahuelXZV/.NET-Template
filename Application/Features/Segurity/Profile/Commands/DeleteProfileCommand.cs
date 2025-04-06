using Domain.Common;
using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;

namespace Application.Features.Segurity.Profile.Commands;

public class DeleteProfileCommand : ICommand<Response<bool>>
{
    public required long Id { get; set; }
}

public class DeleteProfileCommandHandler : ICommandHandler<DeleteProfileCommand, Response<bool>>
{
    private readonly IRepository<Perfil> _repository;

    public DeleteProfileCommandHandler(IRepository<Perfil> repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var perfil = await _repository.GetByIdAsync(request.Id);
        if (perfil == null) throw new Exception("Perfil no encontrado.");

        perfil.Eliminado = true;
        _repository.Update(perfil);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}
