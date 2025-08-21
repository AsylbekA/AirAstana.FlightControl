using AirAstana.FlightControl.Application.Features.Flights.Commands;
using FluentValidation;

namespace AirAstana.FlightControl.Application.Features.Flights.Validators;


public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(f => f.flightDto.Origin)
            .NotEmpty().WithMessage("Origin is required")
            .MaximumLength(256);

        RuleFor(f => f.flightDto.Destination)
            .NotEmpty().WithMessage("Destination is required")
            .MaximumLength(256);

        RuleFor(f => f.flightDto.Departure)
            .LessThan(f => f.flightDto.Arrival).WithMessage("Departure must be earlier than Arrival");
    }
}