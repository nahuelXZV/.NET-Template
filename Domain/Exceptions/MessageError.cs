using System.Text.Json.Serialization;

namespace Domain.Exceptions;

public class MessageError
{
    public string ClientMessage { get; set; }
    public string DiagnosticMessage { get; set; }
    public string ServiceStackTrace { get; set; }
    public IDictionary<string, MessageError> ErrorDetails { get; }

    public MessageError(string clientMessage, string diagnosticMessage = "Sin diagnostico", string serviceStackTrace = "")
    {
        ClientMessage = clientMessage;
        DiagnosticMessage = diagnosticMessage;
        ServiceStackTrace = serviceStackTrace;
        ErrorDetails = new Dictionary<string, MessageError>();
    }

    [JsonConstructor]
    public MessageError(string clientMessage, string diagnosticMessage, string serviceStackTrace, IDictionary<string, MessageError> errorDetails)
    {
        ClientMessage = clientMessage;
        DiagnosticMessage = diagnosticMessage;
        ServiceStackTrace = serviceStackTrace;

        if (errorDetails != null && errorDetails.Any())
        {
            ErrorDetails = errorDetails;
        }
        else
        {
            ErrorDetails = new Dictionary<string, MessageError>();
        }
    }
}
