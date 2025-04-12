using Microsoft.AspNetCore.Mvc;
using WebClientMVC.Models;

namespace WebClientMVC.Controllers;

[Route("Usuario")]
public class UsuarioController : MainController
{
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ViewModelFactory viewModelFactory, ILogger<UsuarioController> logger) : base(viewModelFactory)
    {
        _logger = logger;
    }

    [HttpGet("Listado")]
    public async Task<IActionResult> Listado()
    {
        var model = _viewModelFactory.Create<UsuarioViewModel>();
        model.IncluirBlazorComponents = true;
        return View(model);
    }


}
