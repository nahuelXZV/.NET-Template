using MediatR;

namespace Domain.Interfaces.Shared;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
