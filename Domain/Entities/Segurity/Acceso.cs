using Domain.Common;
using Domain.Entities.General;

namespace Domain.Entities.Segurity;

public class Acceso : Entity
{
    public string Nombre { get; set; }
    public int Secuencia { get; set; }
    public string Controlador { get; set; }
    public string Vista { get; set; }
    public string Url { get; set; }
    public string Icono { get; set; }
    public string Descripcion { get; set; }
    public long ModuloId { get; set; }

    public Concepto Modulo { get; set; }
}
