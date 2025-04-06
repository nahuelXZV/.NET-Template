
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.Segurity;

public class ModuloDTO
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public string Icono { get; set; }
    public int Secuencia { get; set; }

    [NotMapped]
    public List<AccesoDTO> ListaAccesos { get; set; }
}
