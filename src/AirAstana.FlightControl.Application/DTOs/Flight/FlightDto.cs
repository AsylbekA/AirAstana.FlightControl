using System;
using AirAstana.FlightControl.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace AirAstana.FlightControl.Application.DTOs.Flight;

public record FlightDto(int Id, string Origin, string Destination, DateTimeOffset Departure, DateTimeOffset Arrival, FlightStatus Status);

public class FlightDtoExample : IExamplesProvider<FlightDto>
{
    public FlightDto GetExamples()
    {   
        return new FlightDto(
            Id: 1,
            Origin: "Almaty",
            Destination: "Nur-Sultan",
            Departure: DateTimeOffset.UtcNow.AddHours(2),
            Arrival: DateTimeOffset.UtcNow.AddHours(4),
            Status: FlightStatus.InTime
        );
    }
}