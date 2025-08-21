namespace AirAstana.FlightControl.Application.DTOs.Login;

public class LoginDto
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token);
}