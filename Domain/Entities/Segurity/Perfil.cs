
using Domain.Common;

namespace Domain.Entities.Segurity;

public class Perfil : Entity
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }

    public List<PerfilAcceso> ListaAccesos { get; set; }
}
