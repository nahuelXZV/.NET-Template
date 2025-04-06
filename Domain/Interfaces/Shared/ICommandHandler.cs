using MediatR;

namespace Domain.Interfaces.Shared;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
}

