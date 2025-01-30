﻿using System.Runtime.Serialization;

namespace Domain.Exceptions;
[Serializable]
public class CinemaCmsServiceException : Exception
{
    public MessageError Error { get; set; }


    public CinemaCmsServiceException()
    {
    }

    public CinemaCmsServiceException(string message) : base(message)
    {
        Error = new MessageError(message);
    }

    public CinemaCmsServiceException(string message, Exception innerException) : base(message, innerException)
    {
        Error = new MessageError(message);
        var errorDetail = new MessageError(innerException.Message, innerException.Message, innerException.StackTrace);
        Error.ErrorDetails.Add(innerException.GetType().Name, errorDetail);
    }

    public CinemaCmsServiceException(string message, MessageError error) : base(message)
    {
        Error = error;
    }

    public CinemaCmsServiceException(string message, MessageError error, Exception innerException) : base(message, innerException)
    {
        Error = error;
        var errorDetail = new MessageError(innerException.Message, innerException.Message, innerException.StackTrace);
        Error.ErrorDetails.Add(innerException.GetType().Name, errorDetail);
    }

    protected CinemaCmsServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Error = info.GetValue(nameof(Error), typeof(MessageError)) as MessageError;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        info.AddValue(nameof(Error), Error);
    }
}
