using AirAstana.FlightControl.Application.DTOs.Login;
using AirAstana.FlightControl.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
    /// Авторизация пользователя (получение JWT токена)
    /// </summary>
    /// <param name="request">Логин и пароль. Пример: { "username": "Moderator", "password": "Moderator123!" }</param>
    /// <returns>JWT токен</returns>
    /// <response code="200">Успешный вход</response>
    /// <response code="401">Неверный логин или пароль</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerRequestExample(typeof(LoginDto.LoginRequest), typeof(LoginDto.AuthRequestExample))]
    [SwaggerResponse(200, "Успешный вход", typeof(LoginDto.LoginResponse))]
    [SwaggerResponse(401, "Неверный логин или пароль")]
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