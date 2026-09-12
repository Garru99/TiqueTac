using Microsoft.EntityFrameworkCore;
using TiqueTac.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// OBTENER LA CADENA DE CONEXIÓN
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddDbContext<TiqueTacDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configurar el pipeline de peticiones HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/test-db", async (TiqueTacDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        return canConnect
            ? Results.Ok(new { Mensaje = "Oleeeee" })
            : Results.BadRequest(new { Mensaje = "Error al conectar a la base de datos." });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error no controlado de base de datos: {ex.Message}");
    }
});

app.Run();
