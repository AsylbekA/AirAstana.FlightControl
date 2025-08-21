using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace AirAstana.FlightControl.Application.Features.Flights.Queries;

public record GetFlightsQuery(string? Origin = null, string? Destination = null) : IRequest<IEnumerable<FlightDto>>;
public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, IEnumerable<FlightDto>>
{
    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;

    public GetFlightsQueryHandler(IFlightService flightService, IMapper mapper)
    {
        _flightService = flightService;
        _mapper = mapper;
    }

    public  async Task<IEnumerable<FlightDto>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetFlightsAsync(request.Origin,request.Destination,cancellationToken);
        return _mapper.Map<List<FlightDto>>(flights);
    }
}