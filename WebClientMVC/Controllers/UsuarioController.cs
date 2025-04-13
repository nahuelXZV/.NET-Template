using Domain.DTOs.Segurity;
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


    // metodo fake para obtener un listado, filtrado y paginado
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll([FromQuery] string search = "", [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        var usuarios = new List<UsuarioDTO>
        {
            new UsuarioDTO { Id = 1, Username = "jperez", Nombre = "Juan", Apellido = "Perez", Email = "juan@example.com", PerfilId = 1, Perfil = new PerfilDTO { Id = 1, Nombre = "Admin" } },
            new UsuarioDTO { Id = 2, Username = "mlopez", Nombre = "Maria", Apellido = "Lopez", Email = "maria@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 3, Username = "pgonzalez", Nombre = "Pedro", Apellido = "Gonzalez", Email = "pedro@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 4, Username = "lramos", Nombre = "Lucia", Apellido = "Ramos", Email = "lucia@example.com", PerfilId = 1, Perfil = new PerfilDTO { Id = 1, Nombre = "Admin" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } },
            new UsuarioDTO { Id = 5, Username = "cdiaz", Nombre = "Carlos", Apellido = "Diaz", Email = "carlos@example.com", PerfilId = 2, Perfil = new PerfilDTO { Id = 2, Nombre = "Usuario" } }
        };

        var filtrados = string.IsNullOrWhiteSpace(search)
            ? usuarios
            : usuarios.Where(u =>
                u.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.Apellido.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var resultado = filtrados.Skip(offset).Take(limit).ToList();

        return Ok(new
        {
            Total = filtrados.Count,
            Data = resultado
        });
    }




}
