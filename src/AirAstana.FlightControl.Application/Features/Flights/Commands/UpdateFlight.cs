using System;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Entities;
using MediatR;

namespace AirAstana.FlightControl.Application.Features.Flights.Commands;

public record UpdateFlightStatusCommand(int Id, FlightStatus Status) : IRequest<bool>;

public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand, bool>
{
    private readonly IFlightService _flightService;

    public UpdateFlightStatusCommandHandler(IFlightService flightService)
    {
        _flightService = flightService;
    }

    public async Task<bool> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var flight = await _flightService.FindFlightAsync(request.Id, cancellationToken);
        if (flight == null) return false;

        flight.Status = request.Status;

        await _flightService.SaveChangesAsync("system", cancellationToken);
        return true;
    }
}