using AirAstana.FlightControl.Application.DTOs.Flight;
using AirAstana.FlightControl.Application.Features.Flights.Commands;
using AirAstana.FlightControl.Application.Features.Flights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
    /// <param name="origin">Город отправления</param>
    /// <param name="destination">Город назначения</param>
    /// <response code="200">Список рейсов</response>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerResponse(200, "Список рейсов", typeof(List<FlightDto>))]
    public async Task<IActionResult> GetFlights([FromQuery] string? origin, [FromQuery] string? destination)
    {
        var result = await _mediator.Send(new GetFlightsQuery(origin, destination));
        return Ok(result);
    }

    
    /// <summary>
    /// Создать новый рейс.
    /// Только для Moderator.
    /// </summary>
    /// <param name="flightDto">Данные рейса</param>
    /// <response code="201">Рейс успешно создан</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Требуется авторизация</response>
    [HttpPost]
    [Authorize(Roles = "Moderator")]
    [SwaggerRequestExample(typeof(FlightDto), typeof(FlightDtoExample))]
    [SwaggerResponse(201, "Рейс создан", typeof(int))]
    [SwaggerResponse(400, "Неверные данные")]
    [SwaggerResponse(401, "Требуется авторизация")]
    public async Task<IActionResult> Create([FromBody] FlightDto flightDto)
    {
        var id = await _mediator.Send(new CreateFlightCommand(flightDto));
        return CreatedAtAction(nameof(Create), new { id }, id);
    }
    
    /// <summary>
    /// Обновить статус рейса (InTime, Delayed, Cancelled).
    /// Только для Moderator.
    /// </summary>
    /// <param name="id">ID рейса</param>
    /// <param name="command">Новый статус</param>
    /// <response code="200">Статус успешно обновлён</response>
    /// <response code="400">Id в URL и теле не совпадают</response>
    /// <response code="401">Требуется авторизация</response>
    /// <response code="404">Рейс не найден</response>
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Moderator")]
    [SwaggerResponse(200, "Статус обновлён")]
    [SwaggerResponse(400, "Id в URL и теле не совпадают")]
    [SwaggerResponse(401, "Требуется авторизация")]
    [SwaggerResponse(404, "Рейс не найден")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFlightStatusCommand command )
    {
        if (id != command.Id) return BadRequest("Id в URL и в теле запроса должны совпадать.");

        var success = await _mediator.Send(command);
        return success ? Ok() : NotFound();
    }
}
