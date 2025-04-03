
using Domain.Common;

namespace Domain.Entities.Segurity;

public class PerfilAcceso : Entity
{
    public long PerfilId { get; set; }
    public long AccesoId { get; set; }

    public Acceso? Acceso { get; set; }
}
