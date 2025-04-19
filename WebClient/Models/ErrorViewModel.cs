using Microsoft.AspNetCore.Diagnostics;

namespace WebClient.Models;

public class ErrorViewModel : MainViewModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public Exception Error { get; set; }
    public string Path { get; set; }

    public ErrorViewModel()
    {
    }

    public ErrorViewModel(HttpContext context) : base(context)
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature != null)
        {
            Error = exceptionFeature.Error;
            Path = exceptionFeature.Path;
        }
    }
}
