using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class CinemaCmsResourceNotFoundException : Exception
{
    public CinemaCmsResourceNotFoundException()
    {
    }

    public CinemaCmsResourceNotFoundException(string message) : base(message)
    {
    }

    public CinemaCmsResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CinemaCmsResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
