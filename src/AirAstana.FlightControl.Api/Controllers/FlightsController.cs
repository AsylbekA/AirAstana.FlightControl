using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Application.Features.Flights.Commands;
using AirAstana.FlightControl.Application.Features.Flights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirAstana.FlightControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator) => _mediator = mediator;
    
    /// <summary>
    /// Получить список рейсов (с фильтрацией по Origin и Destination).
    /// </summary>
    [HttpGet]
    [AllowAnonymous] // Доступен всем
    public async Task<IActionResult> GetFlights([FromQuery] string? origin, [FromQuery] string? destination)
    {
        var result = await _mediator.Send(new GetFlightsQuery(origin, destination));
        return Ok(result);
    }

    /// <summary>
    /// Создать новый рейс.
    /// Только для Moderator.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Create([FromBody] FlightDto flightDto)
    {
        var id = await _mediator.Send(new CreateFlightCommand(flightDto));
        return CreatedAtAction(nameof(Create), new { id }, id);
    }
    
    /// <summary>
    /// Обновить статус рейса (InTime, Delayed, Cancelled).
    /// Только для Moderator.
    /// </summary>
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFlightStatusCommand command )
    {
        if (id != command.Id) return BadRequest("Id в URL и в теле запроса должны совпадать.");

        var success = await _mediator.Send(command);
        return success ? Ok() : NotFound();
    }
}
