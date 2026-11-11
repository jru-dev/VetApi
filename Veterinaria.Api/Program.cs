using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Negocio.Mapeos;
using Veterinaria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ---------- Controladores y OpenAPI ----------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------- DbContext (la cadena de conexión viene de user-secrets, no del repositorio) ----------
var cadenaConexion = builder.Configuration.GetConnectionString("Veterinaria");
if (string.IsNullOrWhiteSpace(cadenaConexion))
    throw new InvalidOperationException(
        "Falta la cadena de conexión 'ConnectionStrings:Veterinaria'. Configúrala con: " +
        "dotnet user-secrets set \"ConnectionStrings:Veterinaria\" \"<cadena>\" --project Veterinaria.Api");

builder.Services.AddDbContext<VeterinariaContext>(opciones => opciones.UseSqlServer(cadenaConexion));

// ---------- Repositorios y servicios (siempre por interfaz) ----------
builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<IMascotaService, MascotaService>();

// ---------- AutoMapper (un único MappingProfile en la capa de Negocio) ----------
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                                   // /openapi/v1.json
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1");
    });                                                 // /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
