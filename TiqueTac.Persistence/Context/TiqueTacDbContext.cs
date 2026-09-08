using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TiqueTac.Domain.Entities;

namespace TiqueTac.Persistence.Context;

public class TiqueTacDbContext : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Asiento> Asientos => Set<Asiento>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    public TiqueTacDbContext(DbContextOptions<TiqueTacDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("dtusuarios"); // Tu nombre de tabla en Postgres
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.IdUsuario).HasColumnName("idusuario");
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(150);
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("dteventos");
            entity.HasKey(e => e.IdEvento);
            entity.Property(e => e.IdEvento).HasColumnName("idevento");
            entity.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(200);
            entity.Property(e => e.FechaEvento).HasColumnName("fechaevento");
            entity.Property(e => e.PrecioBase).HasColumnName("preciobase");
        });

        modelBuilder.Entity<Asiento>(entity =>
        {
            entity.ToTable("dtasientos");
            entity.HasKey(e => e.IdAsiento);
            entity.Property(e => e.IdAsiento).HasColumnName("idasiento");
            entity.Property(e => e.IdEvento).HasColumnName("idevento");
            entity.Property(e => e.NumeroAsiento).HasColumnName("numero_asiento").HasMaxLength(20);
            entity.Property(e => e.Zona).HasColumnName("zona").HasMaxLength(50);

            entity.Property(e => e.Primero)
                  .HasColumnName("primero")
                  .IsConcurrencyToken();
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("dtreservas");
            entity.HasKey(e => e.IdReserva);
            entity.Property(e => e.IdReserva).HasColumnName("idreserva");
            entity.Property(e => e.IdAsiento).HasColumnName("idasiento");
            entity.Property(e => e.IdUsuario).HasColumnName("idusuario");
            entity.Property(e => e.FechaReserva).HasColumnName("fechareserva");
            entity.Property(e => e.ExpiracionReserva).HasColumnName("expiracionreserva");

            entity.Property(e => e.Estado)
                  .HasColumnName("estado")
                  .HasMaxLength(20)
                  .HasConversion<string>();
        });
    }
}
