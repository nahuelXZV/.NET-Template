using System.Text.Json.Serialization;

namespace WebClient.Models;

public class ResponseError
{
    public string ClientMessage { get; set; }
    public string DiagnosticMessage { get; set; }
    public string ServiceStackTrace { get; set; }
    public IDictionary<string, ResponseError> ErrorDetails { get; }

    public ResponseError(string clientMessage, string diagnosticMessage = "Sin diagnostico", string serviceStackTrace = "")
    {
        ClientMessage = clientMessage;
        DiagnosticMessage = diagnosticMessage;
        ServiceStackTrace = serviceStackTrace;
        ErrorDetails = new Dictionary<string, ResponseError>();
    }

    [JsonConstructor]
    public ResponseError(string clientMessage, string diagnosticMessage, string serviceStackTrace, IDictionary<string, ResponseError> errorDetails)
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
            ErrorDetails = new Dictionary<string, ResponseError>();
        }
    }
}

