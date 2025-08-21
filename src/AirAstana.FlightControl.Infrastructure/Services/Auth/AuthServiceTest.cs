using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Login;
using AirAstana.FlightControl.Application.Interfaces.Services;

namespace AirAstana.FlightControl.Infrastructure.Services.Auth;


public class AuthServiceTest : IAuthService
{
    private readonly IJwtTokenService _jwtService;

    // статический список пользователей
    private readonly List<(string Username, string Password, string Role)> _users = new()
    {
        ("Moderator", "Moderator123!", "Moderator"),
        ("Client", "Client123!", "Client")
    };

    public AuthServiceTest(IJwtTokenService jwtService)
    {
        _jwtService = jwtService;
    }
    public Task<LoginDto.LoginResponse> LoginAsync(LoginDto.LoginRequest request)
    {
        var user = _users.FirstOrDefault(u => 
            u.Username == request.Username && u.Password == request.Password);

        if (user == default)
            return Task.FromResult<LoginDto.LoginResponse>(null);

        string token = _jwtService.GenerateToken(user.Username, user.Role);
        return Task.FromResult(new LoginDto.LoginResponse(token));
    }
}