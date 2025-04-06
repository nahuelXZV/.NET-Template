using Domain.Interfaces.Shared;
using MediatR;

namespace Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IDbContext _appCnx;

    public TransactionBehaviour(IDbContext appCnx)
    {
        _appCnx = appCnx;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var response = default(TResponse);
        try
        {
            if (_appCnx.HasActiveTransaction) return await next();
            using (var transaction = await _appCnx.BeginTransactionAsync())
            {
                response = await next();
                await _appCnx.CommitTransactionAsync(transaction);
            }
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
