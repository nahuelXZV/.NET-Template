using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.Segurity;

[Table("usuario", Schema = "Seguridad")]
public class Usuario : Entity
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; }
}
