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
    
    /// <summary>
    /// Авторизация пользователя (получение JWT токена).
    /// </summary>
    /// <param name="request">Логин и пароль. 
    /// Пример:
    /// {
    ///   "username": "Moderator",
    ///   "password": "Moderator123!"
    /// }
    /// </param>
    /// <returns>JWT токен + роль</returns>
    /// <response code="200">Успешный вход</response>
    /// <response code="401">Неверный логин или пароль</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginDto.LoginResponse), 200)]
    [ProducesResponseType(401)]
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