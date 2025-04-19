using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using WebClient.Common.Middlewares;
using WebClient.Configs;
using WebClient.Extensions;
using WebClient.Models;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseCustomLogger();

#region Variables
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
AdminConfig Configuraciones = new AdminConfig();
configuration.Bind(Configuraciones);
#endregion


var services = builder.Services;

services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

if (environment.IsDevelopment())
{
    services.AddRazorPages().AddRazorRuntimeCompilation();
}
else
{
    services.AddRazorPages();
}

services.AddHttpContextAccessor();

#region Blazor
services.AddServerSideBlazor()
    .AddCircuitOptions(options => options.DetailedErrors = true)
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 102400000);
#endregion

#region Sesion
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\\keys\\"))
    .SetApplicationName("MiAplicacion");

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(Configuraciones.General.TiempoExpiracionCookie);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region Configuraciones
services.Configure<AdminConfig>(configuration);
services.AddServices(Configuraciones);
services.AddSingleton<ViewModelFactory>();
#endregion

#region Authentication
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Sesion/Index";
        options.LogoutPath = "/Sesion/Cerrar";
        options.AccessDeniedPath = "/Sesion/AccesoDenegado";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(Configuraciones.General.TiempoExpiracionCookie);
    });
#endregion



var app = builder.Build();

if (environment.IsDevelopment())
    app.UseExceptionHandler("/Error");
else
    app.UseExceptionHandler("/Error");

app.UseRequestLocalization("es-ES");
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sesion}/{action=Index}/{id?}")
    .RequireAuthorization();

app.MapRazorPages();
app.MapBlazorHub(options => options.CloseOnAuthenticationExpiration = true)
    .RequireAuthorization();

Configuraciones.General.WwwRootPath = environment.WebRootPath;

app.Run();
