using Domain.Entities;

namespace Domain.Interfaces;

public interface IClimaService
{
    Task<List<WeatherForecast>> GetAll();
}
