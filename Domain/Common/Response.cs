using Domain.Exceptions;
using Domain.Interfaces.Shared;

namespace Domain.Common;

public class Response<T> : IResponse
{
    public bool Succeded { get; set; }
    public string Message { get; set; }
    public string ClientMessage { get; set; }
    public MessageError Errors { get; set; }
    public T Data { get; set; }

    public Response() { }

    public Response(T data, string message = null)
    {
        Succeded = true;
        Message = message;
        Data = data;
    }

    public Response(string message)
    {
        Succeded = false;
        Message = message;
        Errors = new MessageError(message);
    }

    public Response(Exception exception)
    {
        Succeded = false;
        Errors = new MessageError(exception.Message);
    }


}