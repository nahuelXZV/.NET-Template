using Domain.Entities.Segurity;
using Domain.Interfaces.Shared;
using Domain.Common;

namespace Application.Features.Segurity.Users.Commands;

public class DeleteUserCommand : ICommand<Response<bool>>
{
    public long Id { get; set; }
}

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Response<bool>>
{
    private readonly IRepository<Usuario> _repository;

    public DeleteUserCommandHandler(IRepository<Usuario> repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _repository.GetByIdAsync(request.Id);
        if (usuario == null) throw new ArgumentException("El usuario no existe.");

        usuario.Eliminado = true;

        _repository.Update(usuario);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}
