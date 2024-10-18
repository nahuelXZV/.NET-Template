using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("weather")]
public class WeatherForecast
{
    [Key]
    public int id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    [NotMapped]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
