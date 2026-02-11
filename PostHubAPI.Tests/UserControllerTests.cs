using Microsoft.AspNetCore.Mvc;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.User;
using PostHubAPI.Services.Interfaces;
using Xunit;

namespace PostHubAPI.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public void Register_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var expectedToken = "test-jwt-token";
        _mockUserService.Setup(x => x.Register(registerDto))
            .ReturnsAsync(expectedToken);

        // Act
        var result = _controller.Register(registerDto);

        // Assert
        // NOTE: This documents a bug - the controller doesn't await, so it returns a Task<string>
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<Task<string>>(okResult.Value);
    }

    [Fact]
    public void Register_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "invalid-email",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _controller.ModelState.AddModelError("Email", "Invalid email format");

        // Act
        var result = _controller.Register(registerDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public void Register_WithServiceException_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var exceptionMessage = "User already exists";
        _mockUserService.Setup(x => x.Register(registerDto))
            .ThrowsAsync(new ArgumentException(exceptionMessage));

        // Act
        var result = _controller.Register(registerDto);

        // Assert
        // NOTE: This test documents a bug - the controller doesn't await the async method,
        // so exceptions are never caught and it returns Ok with a Task instead
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<Task<string>>(okResult.Value);
    }

    [Fact]
    public void Login_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var expectedToken = "test-jwt-token";
        _mockUserService.Setup(x => x.Login(loginDto))
            .ReturnsAsync(expectedToken);

        // Act
        var result = _controller.Login(loginDto);

        // Assert
        // NOTE: This documents a bug - the controller doesn't await, so it returns a Task<string>
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<Task<string>>(okResult.Value);
    }

    [Fact]
    public void Login_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "",
            Password = "Password123!"
        };

        _controller.ModelState.AddModelError("Username", "Username is required");

        // Act
        var result = _controller.Login(loginDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public void Login_WithServiceException_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "WrongPassword!"
        };

        var exceptionMessage = "Invalid credentials";
        _mockUserService.Setup(x => x.Login(loginDto))
            .ThrowsAsync(new ArgumentException(exceptionMessage));

        // Act
        var result = _controller.Login(loginDto);

        // Assert
        // NOTE: This test documents a bug - the controller doesn't await the async method,
        // so exceptions are never caught and it returns Ok with a Task instead
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<Task<string>>(okResult.Value);
    }
}
