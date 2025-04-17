using Domain.DTOs.Segurity;

namespace WebClientMVC.Models;

public class UsuarioViewModel : MainViewModel
{
    public List<UsuarioDTO> ListaUsuarios { get; set; }

    public UsuarioViewModel() : base() { }
}

