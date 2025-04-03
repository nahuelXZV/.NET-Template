namespace Domain.DTOs.Segurity.Request;

public class RequestRegisterDTO
{
    public string Username { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long PerfilId { get; set; }

    public PerfilDTO? Perfil { get; set; }
}
