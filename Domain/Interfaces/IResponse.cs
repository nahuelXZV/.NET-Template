namespace Domain.Interfaces
{
    public interface IResponse
    {
        bool Succeded { get; set; }
        string Message { get; set; }
        string ClientMessage { get; set; }
        IDictionary<string, string[]> Errors { get; set; }

    }
}
