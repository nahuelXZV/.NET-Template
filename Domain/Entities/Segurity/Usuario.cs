using Domain.Common;

namespace Domain.Entities.Segurity;

public class Usuario : Entity
{
    public string Username { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime FechaCreacion { get; set; }
    public long PerfilId { get; set; }

    public Perfil Perfil { get; set; }
}
