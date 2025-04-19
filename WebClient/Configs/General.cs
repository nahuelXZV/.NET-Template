namespace WebClient.Configs;

public class General
{
    public string Nombre { get; set; }
    public string Version { get; set; }
    public string WwwRootPath { get; set; }
    public string WebUrl { get; set; }
    public string ApiUrl { get; set; }
    public int ServiceTimeout { get; set; }
    public int TiempoExpiracionCookie { get; set; }
}