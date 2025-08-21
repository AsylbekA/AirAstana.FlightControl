using System;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AirAstana.FlightControl.Application.Features.Flights.Commands;

public record CreateFlightCommand(FlightDto flightDto) : IRequest<int>;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
{
    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;

    public CreateFlightCommandHandler(
        IFlightService flightService,
        IMapper mapper
    )
    {
        _flightService = flightService ?? throw new ArgumentNullException(nameof(flightService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<int> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = _mapper.Map<Flight>(request.flightDto);
        return await _flightService.AddFlightAsync(flight, cancellationToken);
    }
}