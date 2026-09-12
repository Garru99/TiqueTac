using MediatR;
using Microsoft.EntityFrameworkCore;
using TiqueTac.Application.Common.Interfaces;
//using TiqueTac.Domain.Entities;

namespace TiqueTac.Application.Features.Asientos.Queries;

public record ObtenerAsientosLibresQuery(int IdEvento) : IRequest<List<AsientoLibreResponse>>;

public class ObtenerAsientosLibresHandler : IRequestHandler<ObtenerAsientosLibresQuery, List<AsientoLibreResponse>>
{
    private readonly ITiqueTacDbContext _context;

    public ObtenerAsientosLibresHandler(ITiqueTacDbContext context)
    {
        _context = context;
    }

    public async Task<List<AsientoLibreResponse>> Handle(ObtenerAsientosLibresQuery request, CancellationToken cancellationToken)
    {
        // La lectura sigue siendo igual de óptima gracias a la interfaz
        var asientos = await _context.Asientos
            .Where(a => a.IdEvento == request.IdEvento)
            .Include(a => a.Reservas)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return asientos
            .Where(a => a.EstaDisponible())
            .Select(a => new AsientoLibreResponse(a.IdAsiento, a.NumeroAsiento, a.Zona))
            .ToList();
    }
}

public record AsientoLibreResponse(int IdAsiento, string NumeroAsiento, string Zona);
