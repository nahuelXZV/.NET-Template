using MediatR;

namespace Domain.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
