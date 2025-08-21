using System;
using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Domain.Entities;
using AutoMapper;

namespace AirAstana.FlightControl.Application.Mapping;

public class FlightProfile : Profile
{
    public FlightProfile()
    {
        CreateMap<Flight, FlightDto>().ReverseMap();
    }
}