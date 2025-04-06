namespace Domain.DTOs.Segurity;

public class PerfilAccesoDTO
{
    public long Id { get; set; }
    public long PerfilId { get; set; }
    public long AccesoId { get; set; }

    public AccesoDTO? Acceso { get; set; }
}
