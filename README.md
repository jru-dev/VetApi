# Lab 13 – API Veterinaria en capas (ASP.NET Core .NET 10 + EF Core + AutoMapper)

Tema: clínica veterinaria. Entidades: **Propietario 1 : N Mascota**.

```
Veterinaria.slnx
├─ Database/Veterinaria.sql     Script SQL (tablas + datos de ejemplo)
├─ Veterinaria.Entidades        Entidades (scaffold) + DTOs
├─ Veterinaria.Datos            DbContext + repositorios
├─ Veterinaria.Negocio          Servicios, reglas de negocio, MappingProfile
└─ Veterinaria.Api              Controladores, Program.cs, Swagger, .http
```
Referencias: Api → Negocio → Datos → Entidades.

## Requisitos
- .NET SDK 10  (`dotnet --version` debe empezar con 10)
- SQL Server (LocalDB, Express, Developer o Docker)
- VS Code + extensiones: C# Dev Kit, REST Client (humao.rest-client), SQL Server (mssql) – opcional

## Pasos
1. Crear la base de datos: ejecutar `Database/Veterinaria.sql`
   ```
   sqlcmd -S "(localdb)\MSSQLLocalDB" -E -C -f 65001 -i Database/Veterinaria.sql
   ```
2. Guardar la cadena de conexión fuera del repo (user-secrets):
   ```
   dotnet user-secrets set "ConnectionStrings:Veterinaria" "Server=(localdb)\MSSQLLocalDB;Database=Veterinaria;Trusted_Connection=True;TrustServerCertificate=True" --project Veterinaria.Api
   ```
3. Restaurar y compilar:
   ```
   dotnet restore
   dotnet build
   ```
4. Certificado HTTPS (una sola vez): `dotnet dev-certs https --trust`
5. Ejecutar: `dotnet run --project Veterinaria.Api --launch-profile https`
6. Swagger: https://localhost:7080/swagger
7. Pruebas: abrir `Veterinaria.Api/Veterinaria.Api.http` y usar "Send Request".

Detalle de entregables (explicación del tema, sustentación de mapeos, etc.) en `docs/Entregables.md`.
