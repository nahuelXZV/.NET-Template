namespace Domain.DTOs.Shared;
public class ResponseFilterDTO<T>
{
    public long Total { get; set; }
    public List<T> Data { get; set; } = new List<T>();
}
