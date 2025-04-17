namespace Domain.DTOs.Shared;

public class FilterDTO
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public string? Search { get; set; }
}
