using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class CinemaCmsDomainException : Exception
{
    public CinemaCmsDomainException()
    {
    }

    public CinemaCmsDomainException(string message) : base(message)
    {
    }

    public CinemaCmsDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CinemaCmsDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
