using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClient.Models;
using WebClient.Services;
using WebClient.Models;

namespace WebClient.Controllers;

public class HomeController : MainController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ViewModelFactory viewModelFactory, ILogger<HomeController> logger, IAppServices services)
        : base(viewModelFactory, services)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var model = _viewModelFactory.Create<HomeViewModel>();
        model.IncluirBlazorComponents = true;
        return View(model);
    }

    public IActionResult SystemDesign()
    {
        var model = _viewModelFactory.Create<HomeViewModel>();
        model.IncluirBlazorComponents = true;
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
