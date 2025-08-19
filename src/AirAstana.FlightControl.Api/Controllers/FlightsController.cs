using Microsoft.AspNetCore.Mvc;

namespace AirAstana.FlightControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetFlights()
    {
        return Ok(new [] { new { Id = 1, Origin = "ALA", Destination = "NQZ", Status = "InTime" } });
    }
}
