using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Domain.Entities;

namespace AirAstana.FlightControl.Application.Interfaces.Services;

public interface IFlightService
{
    Task<IEnumerable<Flight>> GetFlightsAsync(string? origin, string? destination, CancellationToken ct);
    Task<Flight?> FindFlightAsync(int id, CancellationToken ct);
    Task<int> AddFlightAsync(Flight flight, CancellationToken ct);
    Task SaveChangesAsync(string? username, CancellationToken ct);
}
