using Microsoft.EntityFrameworkCore;
using TiqueTac.Application.Common.Interfaces;
using TiqueTac.Persistence.Context;
using MediatR; 
using TiqueTac.Application.Features.Asientos.Queries;

var builder = WebApplication.CreateBuilder(args);

// OBTENER LA CADENA DE CONEXIÓN
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddDbContext<TiqueTacDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITiqueTacDbContext>(provider =>
    provider.GetRequiredService<TiqueTacDbContext>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(TiqueTac.Application.Common.Interfaces.ITiqueTacDbContext).Assembly));

var app = builder.Build(); // <-- Esta línea ya la tienes, ponlo justo arriba


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

app.MapGet("/api/eventos/{idEvento:int}/asientos", async (int idEvento, IMediator mediator) =>
{
    try
    {
        var query = new ObtenerAsientosLibresQuery(idEvento);
        var resultado = await mediator.Send(query);

        return Results.Ok(resultado);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al recuperar el mapa de asientos: {ex.Message}");
    }
});

app.Run();
