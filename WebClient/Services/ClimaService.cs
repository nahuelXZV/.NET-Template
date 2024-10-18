using Domain.Entities;
using Domain.Interfaces;

namespace WebClient.Services;

public class ClimaService : BaseService, IClimaService
{
    public ClimaService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        : base("/WeatherForecast", httpClientFactory, contextAccessor)
    {
    }

    public async Task<List<WeatherForecast>> GetAll()
    {
        return await GetAsync<List<WeatherForecast>>("");
    }

}
