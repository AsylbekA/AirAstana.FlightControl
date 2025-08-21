namespace AirAstana.FlightControl.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(string username, string role);
}