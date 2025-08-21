using System;
using AirAstana.FlightControl.Domain.Entities;

namespace AirAstana.FlightControl.Application.DTOs.Flight;

public record FlightDto(int Id, string Origin, string Destination, DateTimeOffset Departure, DateTimeOffset Arrival, FlightStatus Status);