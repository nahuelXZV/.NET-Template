using Application;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Common.Extensions;
using Domain.Constants;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion


// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureBase(builder.Configuration);
var configuration = builder.Configuration;

#region JWT Authentication
string jwtIssuer = configuration["Jwt:Issuer"];
string jwtAudience = configuration["Jwt:Audience"];
string jwtKey = configuration["Jwt:Key"];

// Agregar autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validar el emisor del token (Issuer)
        ValidateAudience = true, // Validar la audiencia del token (Audience)
        ValidateLifetime = true, // Validar que no haya expirado
        ValidateIssuerSigningKey = true, // Validar la clave de firma
        ValidIssuer = jwtIssuer, // El emisor del token
        ValidAudience = jwtAudience, // La audiencia del token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // La clave secreta para firmar los tokens
    };
});
builder.Services.AddAuthorization();
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(Constantes.CorsPolicies.ClienteWeb, builder =>
    {
        builder
         //.WithOrigins(Configuraciones.CinemaCMSServicesConfigs.CorsClients.ToArray())
         // Problema de CORS al registrar dispositivo
         // Se puso el allow Any solo para pruebas, luego debería usarse como todo un servicio del apiExterno
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });

    options.AddPolicy(Constantes.CorsPolicies.AllowOrigin, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
#endregion

#region Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API REST", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
        Scheme = "Bearer",
        Name = "Bearer",
        In = ParameterLocation.Header
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { securityScheme, new List<string>() }
            });
    options.CustomSchemaIds(type => type.FullName); // Usa el namespace completo para evitar conflictos
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseCors(Constantes.CorsPolicies.AllowOrigin);
app.UseApplicationMiddlewares();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando la aplicación...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
