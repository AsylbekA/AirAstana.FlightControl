using Swashbuckle.AspNetCore.Filters;

namespace AirAstana.FlightControl.Application.DTOs.Login;

public class LoginDto
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token);
    
    public class AuthRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest(
                Username: "Moderator",
                Password: "Moderator123!"
            );
        }
    }
}