using Domain.DTOs.Segurity;

namespace WebClientMVC.Models;

public class PerfilViewModel : MainViewModel
{
    public List<ModuloDTO> ListaModulos { get; set; }
    public PerfilDTO Perfil { get; set; }

    public PerfilViewModel() : base() { }
}

