using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.Segurity;

public class UsuarioDTO
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long PerfilId { get; set; }

    public string? Token { get; set; }

    [NotMapped]
    public PerfilDTO? Perfil { get; set; }
}
