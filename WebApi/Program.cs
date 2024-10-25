using Application;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureBase(builder.Configuration);

#region JWT Authentication
var configuration = builder.Configuration;
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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
