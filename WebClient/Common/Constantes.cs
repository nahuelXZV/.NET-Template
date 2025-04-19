namespace WebClient.Common;

public class Constantes
{
    public static class HttpClientNames
    {
        public const string ServicioClient = "ServicioClient";
    }

    public static class ClaimTypes
    {
        public const string UsuarioId = "UsuarioId";
        public const string PerfilId = "PerfilId";
        public const string NombreUsuario = "NombreUsuario";
        public const string ApellidoUsuario = "ApellidoUsuario";
        public const string NombreCompleto = "NombreCompleto";
        public const string Token = "Token";
        public const string Correo = "Correo";
        public const string AccesosPermitidos = "AccesosPermitidos";
        public const string IdsAccesosPermitidos = "idsAccesosPermitidos";
        public const string IpSesion = "ipSesion";
        public const string ListaAccesos = "ListaAccesos";
        //public const string { get; set; }

    }

    public static class CodigoConceptos
    {
        public const int Modulos = 1;
    }
}
public enum MessageType
{
    Success = 0,
    Error = 1,
    Warning = 2,
    Information = 3
}

public enum MenuItemId
{
    Editar = 1,
    Eliminar = 2,
    Habilitar = 3,
    Configurar = 4,
    Detalles = 5
}

public enum MenuItemActionType
{
    GET = 0,
    POST = 1,
}
