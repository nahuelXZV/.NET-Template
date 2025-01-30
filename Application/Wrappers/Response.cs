using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Wrappers;

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
    }

    public Response(Exception Exception)
    {
        Succeded = false;
        Message = Exception.Message;
    }


}