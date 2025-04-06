using Microsoft.AspNetCore.Mvc;
using WebClientMVC.Models;

namespace WebClientMVC.Controllers;

public class ErrorController : MainController
{
    public IActionResult Index()
    {
        var model = new ErrorViewModel(HttpContext);
        return View(model);
    }
}
