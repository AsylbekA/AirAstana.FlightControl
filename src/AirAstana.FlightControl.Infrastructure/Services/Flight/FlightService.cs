using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Persistence;
using AirAstana.FlightControl.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirAstana.FlightControl.Infrastructure.Services.Flight;

public class FlightService : IFlightService
{
    private readonly IAppDbContext _context;
    private readonly ILogger<FlightService> _logger;

    public FlightService(IAppDbContext context,
        ILogger<FlightService> logger)
    {
        _context = context?? throw new ArgumentNullException(nameof(context));
        _logger = logger?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Domain.Entities.Flight>> GetFlightsAsync(string? origin, string? destination, CancellationToken ct)
    {
        _logger.LogInformation("Fetching flights with filters Origin={Origin}, Destination={Destination}", origin, destination);

        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrEmpty(origin))
            query = query.Where(f => f.Origin == origin);

        if (!string.IsNullOrEmpty(destination))
            query = query.Where(f => f.Destination == destination);

        return await query
            .OrderBy(f => f.Arrival)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Domain.Entities.Flight?> FindFlightAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching flight with Id={Id}", id);
        return await _context.Flights.FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<int> AddFlightAsync(Domain.Entities.Flight flight, CancellationToken ct)
    {
        _logger.LogInformation("Adding new flight {Flight}", flight);
        await _context.Flights.AddAsync(flight, ct);
        await _context.SaveChangesAsync(ct);
        return flight.Id;
    }

    public async Task SaveChangesAsync(string? username, CancellationToken ct)
    {
        _logger.LogInformation("Saving changes by user {Username}", username);
        await _context.SaveChangesAsync(ct);
    }
}

