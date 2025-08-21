using AirAstana.FlightControl.Application.DTOs.Login;
using AirAstana.FlightControl.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirAstana.FlightControl.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginDto.LoginResponse>> Login([FromBody] LoginDto.LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}