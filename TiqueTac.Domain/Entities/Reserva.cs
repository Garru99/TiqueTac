using System;
using System.Collections.Generic;
using System.Text;
using TiqueTac.Domain.Enums;

namespace TiqueTac.Domain.Entities;

public class Reserva
{
    public Guid IdReserva { get; private set; }
    public int IdAsiento { get; private set; }
    public int IdUsuario { get; private set; }
    public DateTime FechaReserva { get; private set; }
    public DateTime ExpiracionReserva { get; private set; }
    public EstadoReserva Estado { get; private set; }

    public Asiento Asiento { get; private set; } = null!;

    private Reserva() { }

    public Reserva(int idAsiento, int idUsuario)
    {
        IdReserva = Guid.NewGuid();
        IdAsiento = idAsiento;
        IdUsuario = idUsuario;
        FechaReserva = DateTime.UtcNow;
        ExpiracionReserva = FechaReserva.AddMinutes(10);
        Estado = EstadoReserva.Pendiente; 
    }

    public void ConfirmarPago()
    {
        if(Estado == EstadoReserva.Pagada)
        {
            return;
        }

        if (DateTime.UtcNow > ExpiracionReserva)
        {
            Estado = EstadoReserva.Expirada;
            throw new InvalidOperationException("El tiempo límite de 10 minutos para pagar ha terminado.");
        }

        Estado = EstadoReserva.Pagada;
    }

    public void MarcarComoExpirada()
    {
        if (Estado == EstadoReserva.Pendiente)
        {
            Estado = EstadoReserva.Expirada;
        }
    }
}
