using System;
using System.Collections.Generic;
using System.Text;

namespace TiqueTac.Domain.Entities;

public class Evento
{
    public int IdEvento { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public DateTime FechaEvento { get; private set; }
    public decimal PrecioBase { get; private set; }

    private Evento() { }

    public Evento(string titulo, DateTime fechaEvento, decimal precioBase)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título del evento no puede estar vacío.", nameof(titulo));

        if (fechaEvento <= DateTime.UtcNow)
            throw new ArgumentException("La fecha del evento debe ser en el futuro.", nameof(fechaEvento));

        if (precioBase <= 0)
            throw new ArgumentException("El precio base no puede ser cero o negativo.", nameof(precioBase));

        Titulo = titulo;
        FechaEvento = fechaEvento;
        PrecioBase = precioBase;
    }
}
