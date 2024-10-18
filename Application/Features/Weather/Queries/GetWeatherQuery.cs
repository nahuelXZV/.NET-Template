using Application.Wrappers;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Weather.Queries;

public class GetWeatherQuery : IRequest<Response<ICollection<WeatherForecast>>>
{
}

public class GetWeatherQueryHandler : IRequestHandler<GetWeatherQuery, Response<ICollection<WeatherForecast>>>
{
    private readonly IDbContext _appCnxADM;
    private readonly IMediator _mediator;
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public GetWeatherQueryHandler(IDbContext appCnxADM, IMediator mediator)
    {
        _appCnxADM = appCnxADM;
        _mediator = mediator;
    }
    public async Task<Response<ICollection<WeatherForecast>>> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        List<WeatherForecast> weatherForecasts = _appCnxADM.dbContext.Set<WeatherForecast>().ToList();
        return new Response<ICollection<WeatherForecast>>(weatherForecasts);
    }
}