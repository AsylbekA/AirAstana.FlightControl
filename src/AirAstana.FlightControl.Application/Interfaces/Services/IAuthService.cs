using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Login;

namespace AirAstana.FlightControl.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginDto.LoginResponse> LoginAsync(LoginDto.LoginRequest request);
}