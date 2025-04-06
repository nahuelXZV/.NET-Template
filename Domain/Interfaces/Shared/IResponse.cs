using Domain.Exceptions;

namespace Domain.Interfaces.Shared;
public interface IResponse
{
    bool Succeded { get; set; }
    string Message { get; set; }
    string ClientMessage { get; set; }
    MessageError Errors { get; set; }
}
