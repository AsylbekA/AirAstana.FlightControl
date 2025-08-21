using AirAstana.FlightControl.Api.Controllers;
using AirAstana.FlightControl.Application.DTOs.Login;
using AirAstana.FlightControl.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AirAstana.FlightControl.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsValid()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto.LoginRequest>()))
            .ReturnsAsync(new LoginDto.LoginResponse("mocked-token"));

        var controller = new AuthController(authServiceMock.Object);

        var request = new LoginDto.LoginRequest("Moderator", "Moderator123!");

        // Act
        var result = await controller.Login(request);

        // Assert
        Assert.NotNull(result);
        var response = result.Value as LoginDto.LoginResponse;
        Assert.Equal("mocked-token", response.Token);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsInvalid()
    {
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto.LoginRequest>()))
            .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

        var controller = new AuthController(authServiceMock.Object);

        var request = new LoginDto.LoginRequest("Wrong", "Wrong");

        var actionResult = await controller.Login(request);
        var unauthorizedResult = actionResult.Result as UnauthorizedObjectResult;
        Assert.NotNull(unauthorizedResult);
        Assert.Equal("Invalid credentials", unauthorizedResult.Value);
    }
}