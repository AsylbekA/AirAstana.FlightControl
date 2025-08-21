using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.Interfaces.DistributedCache;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Persistence;
using AirAstana.FlightControl.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AirAstana.FlightControl.Infrastructure.Services.Flight;

public class FlightService : IFlightService
{
    private readonly IAppDbContext _context;
    private readonly ILogger<FlightService> _logger;
    private readonly IDistributedCacheService _distributedCache;
    private readonly RedisKeysOptions _redisKeys;

    public FlightService(IAppDbContext context,
        ILogger<FlightService> logger,
        IDistributedCacheService distributedCache,
        IOptions<RedisKeysOptions> options)
    {
        _context = context?? throw new ArgumentNullException(nameof(context));
        _logger = logger?? throw new ArgumentNullException(nameof(logger));
        _distributedCache = distributedCache??throw new ArgumentNullException(nameof(distributedCache));
        _redisKeys = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IEnumerable<Domain.Entities.Flight>> GetFlightsAsync(string? origin, string? destination, CancellationToken ct)
    {
        var cacheKey = $"{_redisKeys.FlightsCacheKey}:{origin ?? "any"}:{destination ?? "any"}";
        
        var cached = await _distributedCache.GetDataAsync<IEnumerable<Domain.Entities.Flight>>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Returning flights from Redis (key={CacheKey})", cacheKey);
            return cached;
        }
        _logger.LogInformation("Fetching flights from DB");

        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrEmpty(origin))
            query = query.Where(f => f.Origin == origin);

        if (!string.IsNullOrEmpty(destination))
            query = query.Where(f => f.Destination == destination);

        var flights = await query
            .OrderBy(f => f.Arrival)
            .ToListAsync(ct);

        await _distributedCache.SetDataWithAbsExpTimeAsync(cacheKey, flights, TimeSpan.FromMinutes(5));

        return flights;
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

