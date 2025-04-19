using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers;

public class ErrorController : MainController
{
    public IActionResult Index()
    {
        var model = new ErrorViewModel(HttpContext);
        return View(model);
    }
}
