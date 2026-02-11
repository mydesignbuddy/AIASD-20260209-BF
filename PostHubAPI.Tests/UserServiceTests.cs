using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using PostHubAPI.Dtos.User;
using PostHubAPI.Models;
using PostHubAPI.Services.Implementations;
using Xunit;

namespace PostHubAPI.Tests;

public class UserServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockUserManager = MockUserManager();
        
        // Setup JWT configuration
        _mockConfiguration.Setup(c => c["JWT:Secret"]).Returns("ThisIsASecretKeyForJWTTokenGenerationWithAtLeast32Characters");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("TestAudience");
        
        _userService = new UserService(_mockConfiguration.Object, _mockUserManager.Object);
    }

    private Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsToken()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User)null);
        
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Success);
        
        _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.Username))
            .ReturnsAsync(new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Username
            });
        
        _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.Register(registerDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password), Times.Once);
    }

    [Fact]
    public async Task Register_WithExistingEmail_ThrowsArgumentException()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "existing@example.com",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(new User { Email = registerDto.Email });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _userService.Register(registerDto));
        
        Assert.Contains(registerDto.Email, exception.Message);
        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task Register_WithFailedCreation_ThrowsArgumentException()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "weak",
            ConfirmPassword = "weak"
        };

        var errors = new[]
        {
            new IdentityError { Description = "Password is too weak" }
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User)null);
        
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Failed(errors));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _userService.Register(registerDto));
        
        Assert.Contains("Unable to register user", exception.Message);
        Assert.Contains("Password is too weak", exception.Message);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);
        
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.Login(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ThrowsArgumentException()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "nonexistent",
            Password = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync((User)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _userService.Login(loginDto));
        
        Assert.Contains(loginDto.Username, exception.Message);
        Assert.Contains("is not registered", exception.Message);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ThrowsArgumentException()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "WrongPassword!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);
        
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _userService.Login(loginDto));
        
        Assert.Contains("Unable to authenticate user", exception.Message);
        Assert.Contains(loginDto.Username, exception.Message);
    }
}
