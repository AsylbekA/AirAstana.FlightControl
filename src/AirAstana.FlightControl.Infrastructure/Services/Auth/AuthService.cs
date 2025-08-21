using System;
using System.Threading.Tasks;
using AirAstana.FlightControl.Application.DTOs.Login;
using AirAstana.FlightControl.Application.Interfaces.Services;
using AirAstana.FlightControl.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.FlightControl.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _db;
    private readonly IJwtTokenService _jwt;

    public AuthService(IAppDbContext db, IJwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<LoginDto.LoginResponse> LoginAsync(LoginDto.LoginRequest request)
    {
        var user = await _db.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _jwt.GenerateToken(user.Username, user.Role.Code);
        return new LoginDto.LoginResponse(token);
    }
}