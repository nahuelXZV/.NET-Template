using Domain.Interfaces;
using MediatR;

namespace Domain.Common.Interfaces;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
}

