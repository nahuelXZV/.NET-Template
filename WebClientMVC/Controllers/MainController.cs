using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebClientMVC.Controllers;

[Authorize]
public class MainController : Controller
{
    public IActionResult ProcessError(Exception ex)
    {
        var match = Regex.Matches(ex.Message, @"\d+").FirstOrDefault();
        int statusCode = match != null ? int.Parse(match.ToString()) : 500;

        statusCode = statusCode > 599 || statusCode < 200 ? 500 : statusCode;

        return StatusCode(statusCode, ex.Message);
        //return StatusCode(statusCode, ex.Error.DiagnosticMessage);
    }
}
