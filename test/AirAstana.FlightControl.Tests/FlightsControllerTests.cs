using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Application.Features.Flights.Commands;
using AirAstana.FlightControl.Application.Features.Flights.Queries;
using AirAstana.FlightControl.Domain.Entities;
using AirAstana.FlightControl.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirAstana.FlightControl.Tests;

public class FlightsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly FlightsController _controller;

    public FlightsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new FlightsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetFlights_ReturnsOkWithFlights()
    {
        var flights = new List<FlightDto>
        {
            new FlightDto(1, "Almaty", "Nur-Sultan", System.DateTimeOffset.Now, System.DateTimeOffset.Now.AddHours(2), FlightStatus.InTime)
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFlightsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(flights);

        var result = await _controller.GetFlights(null, null) as OkObjectResult;

        Assert.NotNull(result);
        var returned = result.Value as List<FlightDto>;
        Assert.Single(returned);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var flightDto = new FlightDto(1, "Almaty", "Nur-Sultan", System.DateTimeOffset.Now, System.DateTimeOffset.Now.AddHours(2), FlightStatus.InTime);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateFlightCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(flightDto.Id);

        var result = await _controller.Create(flightDto) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal(flightDto.Id, result.Value);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsOk_WhenSuccess()
    {
        var command = new UpdateFlightStatusCommand (1,  FlightStatus.Delayed );

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var result = await _controller.UpdateStatus(1, command);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsBadRequest_WhenIdMismatch()
    {
        var command = new UpdateFlightStatusCommand (1,  FlightStatus.Delayed );

        var result = await _controller.UpdateStatus(2, command) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Id в URL и в теле запроса должны совпадать.", result.Value);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsNotFound_WhenMediatorReturnsFalse()
    {
        var command = new UpdateFlightStatusCommand(1,  FlightStatus.Delayed );

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

        var result = await _controller.UpdateStatus(1, command);

        Assert.IsType<NotFoundResult>(result);
    }
}
