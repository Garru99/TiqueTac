using System;
using System.Collections.Generic;
using System.Text;

namespace TiqueTac.Domain.Entities;

public class Asiento
{
    // Propiedades que coinciden con las columnas de tu tabla dtAsientos
    // Las dejamos con 'private set' para que nadie las modifique por error desde fuera
    public int IdAsiento { get; private set; }
    public int IdEvento { get; private set; }
    public string NumeroAsiento { get; private set; } = string.Empty;
    public string Zona { get; private set; } = string.Empty;

    // Tu columna para el token de concurrencia en PostgreSQL
    public int Primero { get; private set; }

    // Relación interna con sus reservas (Un asiento puede tener varias reservas a lo largo del tiempo)
    private readonly List<Reserva> _reservas = new();
    public IReadOnlyCollection<Reserva> Reservas => _reservas.AsReadOnly();

    // Constructor vacío obligatorio para que Entity Framework pueda leer los datos
    private Asiento() { }

    // Constructor oficial para crear un asiento nuevo de forma segura
    public Asiento(int idEvento, string numeroAsiento, string zona)
    {
        if (string.IsNullOrWhiteSpace(numeroAsiento))
            throw new ArgumentException("El número de asiento no puede estar vacío.", nameof(numeroAsiento));

        if (string.IsNullOrWhiteSpace(zona))
            throw new ArgumentException("La zona del asiento no puede estar vacía.", nameof(zona));

        IdEvento = idEvento;
        NumeroAsiento = numeroAsiento;
        Zona = zona;
    }

  // Este método determina si el asiento está libre para ser comprado
    public bool EstaDisponible()
    {
     return !_reservas.Any(r =>
            r.Estado == "Pagada" ||
            (r.Estado == "Pendiente" && r.ExpiracionReserva > DateTime.UtcNow));
    }
}
