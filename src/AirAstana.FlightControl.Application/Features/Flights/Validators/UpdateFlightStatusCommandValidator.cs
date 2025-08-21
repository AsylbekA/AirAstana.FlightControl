using AirAstana.FlightControl.Application.Features.Flights.Commands;
using FluentValidation;

namespace AirAstana.FlightControl.Application.Features.Flights.Validators;

public class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        RuleFor(f => f.Id)
            .GreaterThan(0).WithMessage("Flight Id must be greater than 0");

        RuleFor(f => f.Status)
            .IsInEnum().WithMessage("Invalid flight status");
    }
}