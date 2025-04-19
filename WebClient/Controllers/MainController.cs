using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebClient.Services;
using WebClient.Configs;

namespace WebClient.Controllers;

[Authorize]
public class MainController : Controller
{
    protected ViewModelFactory _viewModelFactory { get; set; }
    protected readonly IAppServices _appServices;

    public MainController() { }

    public MainController(ViewModelFactory viewModelFactory, IAppServices services)
    {
        _viewModelFactory = viewModelFactory;
        _appServices = services;
    }

    public IActionResult ProcessError(Exception ex)
    {
        var match = Regex.Matches(ex.Message, @"\d+").FirstOrDefault();
        int statusCode = match != null ? int.Parse(match.ToString()) : 500;

        statusCode = statusCode > 599 || statusCode < 200 ? 500 : statusCode;

        return StatusCode(statusCode, ex.Message);
    }
}
