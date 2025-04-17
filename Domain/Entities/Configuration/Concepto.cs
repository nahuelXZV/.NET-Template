using Domain.Common;

namespace Domain.Entities.Configuration;

public class Concepto : Entity
{
    public int Tipo { get; set; }
    public string Clave { get; set; }
    public string Nombre { get; set; }
    public string Valor { get; set; }
    public string Descripcion { get; set; }
    public int Secuencia { get; set; }
    public bool Editable { get; set; }
}
