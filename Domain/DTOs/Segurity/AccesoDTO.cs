namespace Domain.DTOs.Segurity;

public class AccesoDTO
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public int Secuencia { get; set; }
    public string Controlador { get; set; }
    public string Vista { get; set; }
    public string Url { get; set; }
    public string Icono { get; set; }
    public string Descripcion { get; set; }
    public long ModuloId { get; set; }

    public ModuloDTO Modulo { get; set; }
}
