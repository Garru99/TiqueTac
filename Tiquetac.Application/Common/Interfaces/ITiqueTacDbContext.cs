using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TiqueTac.Domain.Entities;

namespace TiqueTac.Application.Common.Interfaces;

public interface ITiqueTacDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Evento> Eventos { get; }
    DbSet<Asiento> Asientos { get; }
    DbSet<Reserva> Reservas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

